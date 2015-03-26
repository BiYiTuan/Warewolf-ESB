module EvalTools
open DataAST
open LanguageAST
open LanguageEval
open ToolAst
open Unlimited.Applications.BusinessDesignStudio.Activities
open System.Activities.Statements
open Dev2.Data.SystemTemplates.Models
open Newtonsoft.Json;
//let Eval (env:Environment) (Tools: Tool list)
let AddToScalars (env:Environment) (name:string) (value:Atom)  =
    let rem = Map.remove name env.Scalar |> Map.add name value 
    {       Scalar=rem;
            RecordSets = env.RecordSets
    }

let AddToRecordSets (env:Environment) (name:string) (value:Recordset) =
    let rem = Map.remove name env.RecordSets |> Map.add name value 
    {       Scalar=env.Scalar;
            RecordSets = rem
    }

let AddColumnValueToRecordset (destination:Recordset) (name:string) (values: Atom list) =
     let records = List.zip values [0..(List.length values)] 
     Map.add name (records,(List.length values)) destination

let AddAtomToRecordSet (destination:Environment)(rsetName:string) (rset:Recordset) (name:string) (value: Atom) (position:int) =
    let rsAdded = if Map.containsKey name rset 
                  then rset
                  else Map.add name ([],0) rset
    if position > (snd rsAdded.[name])
    then
        let col =( (value,position)::(fst rsAdded.[name]),position)
        let recset = Map.remove name rsAdded |> Map.add name col
        AddToRecordSets destination rsetName recset 
    else
        let col = (value,position)::List.filter (fun a -> position <>  (snd a)) (fst rsAdded.[name])
        let recset = Map.remove name rsAdded |> Map.add name (col, 1+snd rsAdded.[name])
        AddToRecordSets destination rsetName recset 

let maxColumn (col:ColumnData) =
    snd col

let EvalAssign  (env:Environment) (theAssign:ToolAst.Assign) =
    let left = LanguageEval.Parse env theAssign.Name 
    let right = LanguageEval.Eval env theAssign.Value
    let x = match right with 
            | AtomResult a -> a
            | AtomListresult b -> failwith "recset"
    let ind (col:string) (x:Index) (recSet:Recordset) =
        let rsAdded = 
                  if Map.containsKey col recSet 
                  then recSet
                  else Map.add col ([],0) recSet 
        match x with
                |IntIndex a -> a
                |Star  -> failwith  "invalid"
                |Last -> rsAdded.[col] |> maxColumn |> (fun a ->1+ a)   
    match left with 
        | RecordSetExpression r ->  AddAtomToRecordSet env r.Name env.RecordSets.[r.Name] r.Column x (ind r.Column r.Index env.RecordSets.[r.Name])  
        | ScalarExpression a -> AddToScalars env a x 
        | _  -> failwith "moocow"

let rec EvalMultiAssign (a:ToolAst.Assign list) (env:Environment) =
    match a with 
    | [t] -> EvalAssign env t
    | [] -> env
    | h::t ->  EvalAssign (EvalMultiAssign t env) h



let rec EvalForEach (fore:ForEach) (env:Environment) =
    let rec evalNumberOfExecutes  (iter:int) (t:Tool) (env:Environment) =
        match iter with 
            | 0 -> env
            | a -> evalNumberOfExecutes (a - 1) (t) (EvalTool t env)
    let rec evalInRange (start:int) (endi:int)  (t:Tool) (env:Environment) =
        match start with 
            | a when start < endi -> env
            | _ -> evalInRange (start+1) endi t env
    let rec EvalInRecordSet (r:RecordSetName) (t:Tool) (env:Environment) =
        env.RecordSets.[r.Name] |>  Map.toList  |> List.map snd |> List.map snd |> List.max |> fun ax-> evalNumberOfExecutes ax t env
    match fore.Options with
            | NumberOfExecutes a -> evalNumberOfExecutes a fore.ExecutionAST env
            | InRange (a,b) -> evalInRange a b fore.ExecutionAST env
            | RecordsSet a ->EvalInRecordSet a fore.ExecutionAST env

and EvalTool    (t:Tool) (env:Environment) =
        match t with
        | MultiAssignTool a -> EvalMultiAssign a.Assigns env |> (EvalTool a.ExecutionASTTrue)
        | ForEachTool a ->EvalForEach a env
        | DecisionTool a -> EvalDecision a env
        | NOPTool ->env

and EvalDecision (a:Decision) (env:Environment) =
    let stack = Seq.map (ParseDecisionValue env) a.Conditions.TheStack
    let resolvedDecision = new Dev2DecisionStack(TheStack = new System.Collections.Generic.List<Dev2Decision>(stack))
    let dec = {  Conditions=resolvedDecision;
                 ExecutionASTTrue=a.ExecutionASTTrue;
                 ExecutionASTFalse=a.ExecutionASTFalse;
                
              }:Decision
    let factory = Dev2.Data.Decisions.Operations.Dev2DecisionFactory.Instance();
    let res = Seq.map (fun (a:Dev2Decision) -> (factory.FetchDecisionFunction( a.EvaluationFn):Dev2.Data.Decisions.Operations.IDecisionOperation).Invoke([|a.Col1;a.Col2;a.Col3|]) ) stack
    let resval = Seq.fold (&&) true res
    match resval with
    | true-> EvalTool a.ExecutionASTTrue env
    | false -> EvalTool a.ExecutionASTFalse env

and ParseDecisionValue (env:Environment) (decision:Dev2Decision)  =
   
    let col1 = LanguageEval.Eval env decision.Col1 |> LanguageEval.EvalResultToString
    let col2 = LanguageEval.Eval env decision.Col2 |> LanguageEval.EvalResultToString
    let col3 = LanguageEval.Eval env decision.Col3 |> LanguageEval.EvalResultToString
    new Dev2Decision( Col1=col1 , Col2=col2, Col3 = col3 , EvaluationFn = decision.EvaluationFn)

let ReturnAssign (assign:DsfMultiAssignActivity) =
    let coll = assign.FieldsCollection
    Seq.map (fun (a:ActivityDTO) -> {
                                        Name= a.FieldName;
                                        Value = a.FieldValue;
                                        IsCalc =false;
                    } ) coll |>  Seq.filter (fun a -> not (a.Name.Equals "") ) |>List.ofSeq |>List.rev 

let rec ParseFlowStep (tool:FlowStep) =
    match tool.Action with
        | :? DsfMultiAssignActivity as assign -> {  Assigns= ReturnAssign assign;ExecutionASTTrue = ParseTool tool.Next} |> Tool.MultiAssignTool
        | _  -> {  Assigns= [ {
                                  Name= "";
                                 Value = "";
                                 IsCalc =false;
                                }
                            ];
                  ExecutionASTTrue = ParseTool tool.Next
                  }|> Tool.MultiAssignTool
and  Parse  (flowchart:Flowchart) (resourceID:System.Guid)=
    let start = (flowchart.StartNode) :?> FlowStep
    ParseTool (start.Next :?> FlowStep)

and ParseTool (tool:FlowNode)  = 
    match tool with
    | :? FlowStep as flowStepTool -> ParseFlowStep flowStepTool
    | :? FlowDecision as decisionTool -> ParseDecisionStep decisionTool 
    | _ ->NOPTool
and ParseDecisionStep (tool:FlowDecision) =   
    let a = tool.Condition;
    match a with 
    | :? DsfFlowDecisionActivity as decision  -> ParseDecision tool decision 
    |_ -> failwith "moocow"


and ParseDecision (tool:FlowDecision) (activity:DsfFlowDecisionActivity) =
    let rawText = activity.ExpressionText;
    let activityTextjson = rawText.Substring(rawText.IndexOf("{")).Replace(@""",AmbientDataList)","").Replace("\"","!")
    let activityText = Dev2DecisionStack.FromVBPersitableModelToJSON(activityTextjson)
    let decisionStack =  JsonConvert.DeserializeObject<Dev2DecisionStack>(activityText)
    {
            Conditions = decisionStack;
            ExecutionASTTrue = ParseTool tool.True;
            ExecutionASTFalse =  ParseTool tool.False;
    }:Decision |> Tool.DecisionTool
 


          


let NewEnv  = 
    {
       RecordSets=Map.empty;
       Scalar=Map.empty;
    }
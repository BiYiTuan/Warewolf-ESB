﻿module PublicFunctions

open LanguageAST
//open LanguageEval
open Microsoft.FSharp.Text.Lexing
open DataASTMutable
open WarewolfDataEvaluationCommon

let PositionColumn = "WarewolfPositionColumn#"

let CreateDataSet (a:string) =
    let col = new System.Collections.Generic.List<WarewolfAtomRecord>()
    {
        Data = [(PositionColumn,col) ] |> Map.ofList
        Optimisations = Ordinal;
        LastIndex=0;
        Count=0;
        Frame=0;
    }

let CreateEnv (vals:string) = 
     {
       RecordSets =Map.empty;
       Scalar =Map.empty;
     }

let AddRecsetToEnv (name:string) (env:WarewolfEnvironment) = 
    if env.RecordSets.ContainsKey name
    then
       env
    else
       let b = CreateDataSet ""
       let a = {env with RecordSets= (Map.add name b env.RecordSets);}
       a
let EvalEnvExpression (exp:string) (env:WarewolfEnvironment) =
     WarewolfDataEvaluationCommon.Eval env exp

let AddToScalars (env:WarewolfEnvironment) (name:string) (value:WarewolfAtom)  =
    let rem = Map.remove name env.Scalar |> Map.add name value 
    {       Scalar=rem;
            RecordSets = env.RecordSets
    }

let rec AddToRecordSet (env:WarewolfEnvironment) (name:RecordSetIdentifier) (value:WarewolfAtom)  =
    if(env.RecordSets.ContainsKey name.Name)
    then
        let recordset = env.RecordSets.[name.Name]
        let recsetAdded = match name.Index with
                          | IntIndex a -> AddAtomToRecordSet recordset name.Column value a
                          | Star -> UpdateColumnWithValue recordset name.Column value 
                          | Last -> AddAtomToRecordSet recordset name.Column value (recordset.LastIndex+1)
                          | IndexExpression a -> AddAtomToRecordSet recordset name.Column value (EvalIndex env ( LanguageExpressionToString a ))
        let recsets = Map.remove name.Name env.RecordSets |> fun a-> Map.add name.Name recsetAdded a
        { env with RecordSets = recsets}
    else
        let envwithRecset = AddRecsetToEnv name.Name env
        AddToRecordSet envwithRecset name value
    
let EvalAssign (exp:string) (value:string) (env:WarewolfEnvironment) =
    let left = WarewolfDataEvaluationCommon.ParseLanguageExpression exp 
    let right = WarewolfDataEvaluationCommon.Eval env value
    let x = match right with 
            | WarewolfAtomResult a -> a
            | WarewolfAtomListresult b -> failwith "recset"
    match left with 
    |   ScalarExpression a -> AddToScalars env a x
    |   RecordSetExpression b -> AddToRecordSet env b x
    |   AtomExpression a -> env
    |   _ -> failwith "input must be recordset or value"

let rec AddToRecordSetFramed (env:WarewolfEnvironment) (name:RecordSetIdentifier) (value:WarewolfAtom)  =
    if(env.RecordSets.ContainsKey name.Name)
    then
        let recordset = env.RecordSets.[name.Name]
        let recsetAdded = match name.Index with
                          | IntIndex a -> AddAtomToRecordSetWithFraming recordset name.Column value a false
                          | Star -> UpdateColumnWithValue recordset name.Column value 
                          | Last -> AddAtomToRecordSetWithFraming recordset name.Column value (getPositionFromRecset recordset  name.Column) true
                          | IndexExpression a -> AddAtomToRecordSetWithFraming recordset name.Column value (EvalIndex env ( LanguageExpressionToString a )) false
        let recsets = Map.remove name.Name env.RecordSets |> fun a-> Map.add name.Name recsetAdded a
        { env with RecordSets = recsets}
    else
        let envwithRecset = AddRecsetToEnv name.Name env
        AddToRecordSetFramed envwithRecset name value

let EvalMultiAssignOp  (env:WarewolfEnvironment)  (value :WarewolfParserInterop.IAssignValue ) =
    let left = WarewolfDataEvaluationCommon.ParseLanguageExpression value.Name 
    let right = WarewolfDataEvaluationCommon.Eval env value.Value
    let x = match right with 
            | WarewolfAtomResult a -> a
            | WarewolfAtomListresult b -> failwith "recset on right"
    match left with 
    |   ScalarExpression a -> AddToScalars env a x
    |   RecordSetExpression b -> AddToRecordSetFramed env b x
    |   AtomExpression a -> env
    |   _ -> failwith "input must be recordset or value"


let EvalMultiAssign (values :WarewolfParserInterop.IAssignValue seq) (env:WarewolfEnvironment) =
        let env = Seq.fold EvalMultiAssignOp env values
        let recsets = Map.map (fun a b -> {b with Frame = 0 }) env.RecordSets
        {env with RecordSets = recsets}
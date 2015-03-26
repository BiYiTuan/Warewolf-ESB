module LanguageAST
open DataAST

type ScalarId = string
type Index = 
    | IntIndex of int
    | Star
    | Last
type RecordSetIdentifier = 
    {
        Name : string;
        Column :string;
        Index: Index;
    }
type RecordSetName = 
    {
        Name : string;
    }
type ScalarIdentifier = string
type LanguageExpression = 
    | RecordSetExpression of RecordSetIdentifier
    | ScalarExpression of ScalarIdentifier
    | AtomExpression of Atom
    | ComplexExpression of LanguageExpression list
    | RecordSetNameExpression of RecordSetName

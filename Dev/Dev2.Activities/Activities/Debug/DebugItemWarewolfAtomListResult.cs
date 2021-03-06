using System;
using System.Collections.Generic;
using System.Globalization;
using Dev2.Common.Interfaces.Diagnostics.Debug;
using Dev2.Diagnostics;
using Dev2.Data.Util;
using Dev2.DataList.Contract;

namespace Dev2.Activities.Debug
{
    public class DebugItemWarewolfAtomListResult : DebugOutputBase
    {
        readonly WarewolfDataEvaluationCommon.WarewolfEvalResult.WarewolfAtomListresult _warewolfAtomListresult;
        readonly string _labelText;
        readonly string _operand;
        readonly string _variable;
        readonly DebugItemResultType _type;
        readonly string _rightLabel;
        readonly string _leftLabel;
        readonly WarewolfDataEvaluationCommon.WarewolfEvalResult _oldValue;
        readonly string _assignedToVariableName;
        readonly string _newValue;

        public DebugItemWarewolfAtomListResult(WarewolfDataEvaluationCommon.WarewolfEvalResult.WarewolfAtomListresult warewolfAtomListresult, WarewolfDataEvaluationCommon.WarewolfEvalResult oldResult, string assignedToVariableName, string variable, string leftLabelText, string rightLabelText, string operand)
        {
            _labelText = "";
            _operand = operand;
            _variable = variable;
            _type = DebugItemResultType.Variable;
            _rightLabel = rightLabelText;
            _leftLabel = leftLabelText;
            _warewolfAtomListresult = warewolfAtomListresult;
            _oldValue = oldResult;
            _assignedToVariableName = assignedToVariableName;
        }

        public DebugItemWarewolfAtomListResult(WarewolfDataEvaluationCommon.WarewolfEvalResult.WarewolfAtomListresult warewolfAtomListresult, string newValue, string assignedToVariableName, string variable, string leftLabelText, string rightLabelText, string operand)
        {
            _labelText = "";
            _operand = operand;
            _variable = variable;
            _type = DebugItemResultType.Variable;
            _rightLabel = rightLabelText;
            _leftLabel = leftLabelText;
            _warewolfAtomListresult = warewolfAtomListresult;
            _newValue = newValue;
            _oldValue = null;
            _assignedToVariableName = assignedToVariableName;
        }


        public override string LabelText
        {
            get
            {
                return _labelText;
            }
        }

        public string Variable
        {
            get
            {
                return _variable;
            }
        }
        public DebugItemResultType Type
        {
            get
            {
                return _type;
            }
        }

        public override List<IDebugItemResult> GetDebugItemResult()
        {

            var debugItemsResults = BuildDebugItemFromAtomList();
            return debugItemsResults;
        }

        List<IDebugItemResult> BuildDebugItemFromAtomList()
        {
            var results = new List<IDebugItemResult>();
            if (!string.IsNullOrEmpty(_leftLabel))
            {
                string groupName = null;
                int grpIdx = 0;
                if(_warewolfAtomListresult != null)
                {
                    foreach (var atomItem in _warewolfAtomListresult.Item)
                    {
                        string displayExpression = _variable;
                        string rawExpression = _variable;
                        var item = atomItem.ToString();
                        if (displayExpression.Contains("().") || displayExpression.Contains("(*)."))
                        {
                            grpIdx++;
                            string index = grpIdx.ToString(CultureInfo.InvariantCulture);
                            if (rawExpression.Contains(".WarewolfPositionColumn"))
                            { 
                                index = item;
                                item = "";
                            }
                            groupName = rawExpression.Replace(".WarewolfPositionColumn","");
                            // ReSharper disable EmptyStatement
                            displayExpression = DataListUtil.AddBracketsToValueIfNotExist(DataListUtil.CreateRecordsetDisplayValue(DataListUtil.ExtractRecordsetNameFromValue(_variable), DataListUtil.ExtractFieldNameFromValue(_variable), index)).Replace(".WarewolfPositionColumn", ""); ;
                            // ReSharper restore EmptyStatement
                        }
                        else
                        {

                            string indexRegionFromRecordset = DataListUtil.ExtractIndexRegionFromRecordset(displayExpression);
                            int indexForRecset;
                            int.TryParse(indexRegionFromRecordset, out indexForRecset);

                            if (indexForRecset > 0)
                            {
                                int indexOfOpenningBracket = displayExpression.IndexOf("(", StringComparison.Ordinal) + 1;
                                string group = displayExpression.Substring(0, indexOfOpenningBracket) + "*" + displayExpression.Substring(indexOfOpenningBracket + indexRegionFromRecordset.Length);
                                grpIdx++;
                                groupName = @group;
                            }
                        }

                        var debugOperator = "";
                        var debugType = DebugItemResultType.Value;
                        if (DataListUtil.IsEvaluated(displayExpression))
                        {
                            debugOperator = String.IsNullOrEmpty(item)?"": "=";
                            debugType = DebugItemResultType.Variable;
                        }
                        else
                        {
                            displayExpression = null;
                        }
                        results.Add(new DebugItemResult
                        {
                            Type = debugType,
                            Label = _leftLabel,
                            Variable = DataListUtil.IsEvaluated(displayExpression) ? displayExpression : null,
                            Operator = debugOperator,
                            GroupName = groupName,
                            Value = item,
                            GroupIndex = grpIdx
                        });
                    }
                }
                else
                {
                    results.Add(new DebugItemResult
                    {
                        Type = DebugItemResultType.Variable,
                        Label = _leftLabel,
                        Variable = DataListUtil.IsEvaluated(Variable) ? Variable : null,
                        Operator = _operand,
                        GroupName = null,
                        Value = "",
                        GroupIndex = grpIdx
                    });
                }
            }

            if (!string.IsNullOrEmpty(_rightLabel))
            {
                if (_oldValue != null)
                {
                    if (_oldValue.IsWarewolfAtomResult)
                    {
                        var scalarResult = _oldValue as WarewolfDataEvaluationCommon.WarewolfEvalResult.WarewolfAtomResult;
                        if (scalarResult != null)
                        {
                            results.Add(new DebugItemResult
                            {
                                Type = DebugItemResultType.Variable,
                                Label = _rightLabel,
                                Variable = DataListUtil.IsEvaluated(_assignedToVariableName) ? _assignedToVariableName : null,
                                Operator = string.IsNullOrEmpty(_operand) ? "" : "=",
                                GroupName = null,
                                Value = Warewolf.Storage.ExecutionEnvironment.WarewolfAtomToString(scalarResult.Item),
                                GroupIndex = 0
                            });
                        }
                    }
                    else if (_oldValue.IsWarewolfAtomListresult)
                    {
                        var recSetResult = _oldValue as WarewolfDataEvaluationCommon.WarewolfEvalResult.WarewolfAtomListresult;
                        string groupName = null;
                        int grpIdx = 0;
                        if (recSetResult != null)
                        {
                            foreach (var item in recSetResult.Item)
                            {
                                string displayExpression = _assignedToVariableName;
                                string rawExpression = _assignedToVariableName;
                                if (displayExpression.Contains("().") || displayExpression.Contains("(*)."))
                                {
                                    grpIdx++;
                                    groupName = rawExpression;
                                    displayExpression = DataListUtil.AddBracketsToValueIfNotExist(DataListUtil.CreateRecordsetDisplayValue(DataListUtil.ExtractRecordsetNameFromValue(_assignedToVariableName), DataListUtil.ExtractFieldNameFromValue(_assignedToVariableName), grpIdx.ToString()));
                                }
                                else
                                {

                                    string indexRegionFromRecordset = DataListUtil.ExtractIndexRegionFromRecordset(displayExpression);
                                    int indexForRecset;
                                    int.TryParse(indexRegionFromRecordset, out indexForRecset);

                                    if (indexForRecset > 0)
                                    {
                                        int indexOfOpenningBracket = displayExpression.IndexOf("(", StringComparison.Ordinal) + 1;
                                        string group = displayExpression.Substring(0, indexOfOpenningBracket) + "*" + displayExpression.Substring(indexOfOpenningBracket + indexRegionFromRecordset.Length);
                                        grpIdx++;
                                        groupName = @group;
                                    }
                                }

                                var debugOperator = "";
                                var debugType = DebugItemResultType.Value;
                                if (DataListUtil.IsEvaluated(displayExpression))
                                {
                                    debugOperator = "=";
                                    debugType = DebugItemResultType.Variable;
                                }
                                else
                                {
                                    displayExpression = null;
                                }
                                var debugItemResult = new DebugItemResult
                                {
                                    Type = debugType,
                                    Label = _rightLabel,
                                    Variable = DataListUtil.IsEvaluated(displayExpression) ? displayExpression : null,
                                    Operator = debugOperator,
                                    GroupName = groupName,
                                    Value = Warewolf.Storage.ExecutionEnvironment.WarewolfAtomToString(item),
                                    GroupIndex = grpIdx
                                };
                                results.Add(debugItemResult);
                            }
                        }
                    }
                }
                if (_oldValue == null)
                {
                    var debugItemResult = new DebugItemResult
                    {
                        Type = DebugItemResultType.Variable,
                        Label = _rightLabel,
                        Variable = null,
                        Operator = "=",
                        GroupName = null,
                        Value = _newValue,
                        GroupIndex = 0
                    };
                    results.Add(debugItemResult);
                }
            }

            if (string.IsNullOrEmpty(_rightLabel) && string.IsNullOrEmpty(_leftLabel))
            {
                string groupName = null;
                int grpIdx = 0;
                if (_warewolfAtomListresult != null)
                {
                    foreach (var item in _warewolfAtomListresult.Item)
                    {
                        string displayExpression = _variable;
                        string rawExpression = _variable;
                        if (displayExpression.Contains("().") || displayExpression.Contains("(*)."))
                        {
                            grpIdx++;
                            groupName = rawExpression;
                            displayExpression = DataListUtil.AddBracketsToValueIfNotExist(DataListUtil.CreateRecordsetDisplayValue(DataListUtil.ExtractRecordsetNameFromValue(_variable), DataListUtil.ExtractFieldNameOnlyFromValue(DataListUtil.AddBracketsToValueIfNotExist(_variable)), grpIdx.ToString()));

                            if (DataListUtil.GetRecordsetIndexType(_variable) == enRecordsetIndexType.Star)
                            {
                                displayExpression += _variable.Replace(DataListUtil.ReplaceRecordsetIndexWithStar(displayExpression), "");
                            }
                            else if (DataListUtil.GetRecordsetIndexType(_variable) == enRecordsetIndexType.Blank)
                            {
                                displayExpression += _variable.Replace(DataListUtil.ReplaceRecordsetIndexWithBlank(displayExpression), "");
                            }
                        }
                        else
                        {

                            string indexRegionFromRecordset = DataListUtil.ExtractIndexRegionFromRecordset(displayExpression);
                            int indexForRecset;
                            int.TryParse(indexRegionFromRecordset, out indexForRecset);

                            if (indexForRecset > 0)
                            {
                                int indexOfOpenningBracket = displayExpression.IndexOf("(", StringComparison.Ordinal) + 1;
                                string group = displayExpression.Substring(0, indexOfOpenningBracket) + "*" + displayExpression.Substring(indexOfOpenningBracket + indexRegionFromRecordset.Length);
                                grpIdx++;
                                groupName = @group;
                            }
                        }

                        var debugOperator = "";
                        var debugType = DebugItemResultType.Value;
                        if (DataListUtil.IsEvaluated(displayExpression))
                        {
                            debugOperator = "=";
                            debugType = DebugItemResultType.Variable;
                        }
                        else
                        {
                            displayExpression = null;
                        }
                        results.Add(new DebugItemResult
                        {
                            Type = debugType,
                            Variable = DataListUtil.IsEvaluated(displayExpression) ? displayExpression : null,
                            Operator = debugOperator,
                            GroupName = groupName,
                            Value = Warewolf.Storage.ExecutionEnvironment.WarewolfAtomToString(item),
                            GroupIndex = grpIdx
                        });
                    }
                }
                else
                {
                    results.Add(new DebugItemResult
                    {
                        Type = DebugItemResultType.Variable,
                        Variable = DataListUtil.IsEvaluated(Variable) ? Variable : null,
                        Operator = _operand,
                        GroupName = null,
                        Value = "",
                        GroupIndex = grpIdx
                    });
                }
            }
            return results;
        }
    }
}
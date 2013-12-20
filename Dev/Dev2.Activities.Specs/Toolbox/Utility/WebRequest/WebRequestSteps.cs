﻿using System;
using System.Activities.Statements;
using System.Collections.Generic;
using Dev2.Activities.Specs.BaseTypes;
using Dev2.Data.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace Dev2.Activities.Specs.Toolbox.Utility.WebRequest
{
    [Binding]
    public class WebRequestSteps : RecordSetBases
    {
        protected override void BuildDataList()
        {
            List<Tuple<string, string>> variableList;
            ScenarioContext.Current.TryGetValue("variableList", out variableList);

            if (variableList == null)
            {
                variableList = new List<Tuple<string, string>>();
                ScenarioContext.Current.Add("variableList", variableList);
            }

            variableList.Add(new Tuple<string, string>(ResultVariable, ""));
            BuildShapeAndTestData();

            string header;
                ScenarioContext.Current.TryGetValue("header", out header);
            string url;
                ScenarioContext.Current.TryGetValue("url", out url);

            var webGet = new DsfWebGetRequestActivity
                {
                    Result = ResultVariable,
                    Url = url ?? "",
                    Headers = header ?? ""
                };

            TestStartNode = new FlowStep
                {
                    Action = webGet
                };
        }

        [Given(@"I have the url ""(.*)""")]
        public void GivenIHaveTheUrl(string url)
        {
            ScenarioContext.Current.Add("url", url);
        }

        [When(@"the web request tool is executed")]
        public void WhenTheWebRequestToolIsExecuted()
        {
            BuildDataList();
            IDSFDataObject result = ExecuteProcess(throwException:false);
            ScenarioContext.Current.Add("result", result);
        }

        [Given(@"I have a web request variable ""(.*)"" equal to ""(.*)""")]
        public void GivenIHaveAWebRequestVariableEqualTo(string variable, string value)
        {
            List<Tuple<string, string>> variableList;
            ScenarioContext.Current.TryGetValue("variableList", out variableList);

            if (variableList == null)
            {
                variableList = new List<Tuple<string, string>>();
                ScenarioContext.Current.Add("variableList", variableList);
            }
            variableList.Add(new Tuple<string, string>(variable, value));
        }

        [Given(@"I have the Header ""(.*)""")]
        public void GivenIHaveTheHeader(string header)
        {
            ScenarioContext.Current.Add("header", header);
        }


        [Then(@"the result should contain the string ""(.*)""")]
        public void ThenTheResultShouldContainTheString(string expectedResult)
        {
            string error;
            string actualValue;
            var result = ScenarioContext.Current.Get<IDSFDataObject>("result");
            GetScalarValueFromDataList(result.DataListID, DataListUtil.RemoveLanguageBrackets(ResultVariable),
                                       out actualValue, out error);
            Assert.IsTrue(actualValue.Contains(expectedResult));
        }
    }
}
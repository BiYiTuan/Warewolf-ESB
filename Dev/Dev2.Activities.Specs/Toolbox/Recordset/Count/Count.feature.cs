﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.18063
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Dev2.Activities.Specs.Toolbox.Recordset.Count
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class CountFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Count.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Count", "In order to count records\r\nAs a Warewolf user\r\nI want a tool that takes a record " +
                    "set counts it", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassCleanupAttribute()]
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitializeAttribute()]
        public virtual void TestInitialize()
        {
            if (((TechTalk.SpecFlow.FeatureContext.Current != null) 
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "Count")))
            {
                Dev2.Activities.Specs.Toolbox.Recordset.Count.CountFeature.FeatureSetup(null);
            }
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCleanupAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Count a number of records in a recordset with 3 rows")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Count")]
        public virtual void CountANumberOfRecordsInARecordsetWith3Rows()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Count a number of records in a recordset with 3 rows", ((string[])(null)));
#line 6
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "[[rs]]",
                        ""});
            table1.AddRow(new string[] {
                        "rs().row",
                        "1"});
            table1.AddRow(new string[] {
                        "rs().row",
                        "2"});
            table1.AddRow(new string[] {
                        "rs().row",
                        "3"});
#line 7
 testRunner.Given("I have a recordset with this shape", ((string)(null)), table1, "Given ");
#line 12
 testRunner.And("count on record \"[[rs()]]\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 13
 testRunner.When("the count tool is executed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 14
 testRunner.Then("the result count should be 3", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 15
 testRunner.And("the execution has \"NO\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Recordset"});
            table2.AddRow(new string[] {
                        "[[rs(1).row]] = 1"});
            table2.AddRow(new string[] {
                        "[[rs(2).row]] = 2"});
            table2.AddRow(new string[] {
                        "[[rs(3).row]] = 3"});
#line 16
 testRunner.And("the debug inputs as", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        ""});
            table3.AddRow(new string[] {
                        "[[result]] = 3"});
#line 21
 testRunner.And("the debug output as", ((string)(null)), table3, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Count a number of records in a recordset with 8 rows")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Count")]
        public virtual void CountANumberOfRecordsInARecordsetWith8Rows()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Count a number of records in a recordset with 8 rows", ((string[])(null)));
#line 25
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "rs",
                        ""});
            table4.AddRow(new string[] {
                        "rs().row",
                        "1"});
            table4.AddRow(new string[] {
                        "rs().row",
                        "2"});
            table4.AddRow(new string[] {
                        "rs().row",
                        "3"});
            table4.AddRow(new string[] {
                        "rs().row",
                        "4"});
            table4.AddRow(new string[] {
                        "rs().row",
                        "5"});
            table4.AddRow(new string[] {
                        "rs().row",
                        "6"});
            table4.AddRow(new string[] {
                        "rs().row",
                        "7"});
            table4.AddRow(new string[] {
                        "rs().row",
                        "8"});
#line 26
 testRunner.Given("I have a recordset with this shape", ((string)(null)), table4, "Given ");
#line 36
 testRunner.And("count on record \"[[rs()]]\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
 testRunner.When("the count tool is executed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 38
 testRunner.Then("the result count should be 8", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 39
 testRunner.And("the execution has \"NO\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "Recordset"});
            table5.AddRow(new string[] {
                        "[[rs(1).row]] =  1"});
            table5.AddRow(new string[] {
                        "[[rs(2).row]] =  2"});
            table5.AddRow(new string[] {
                        "[[rs(3).row]] =  3"});
            table5.AddRow(new string[] {
                        "[[rs(4).row]] =  4"});
            table5.AddRow(new string[] {
                        "[[rs(5).row]] =  5"});
            table5.AddRow(new string[] {
                        "[[rs(6).row]] =  6"});
            table5.AddRow(new string[] {
                        "[[rs(7).row]] =  7"});
            table5.AddRow(new string[] {
                        "[[rs(8).row]] =  8"});
#line 40
 testRunner.And("the debug inputs as", ((string)(null)), table5, "And ");
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        ""});
            table6.AddRow(new string[] {
                        "[[result]] = 8"});
#line 50
 testRunner.And("the debug output as", ((string)(null)), table6, "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Count a number of records in a recordset with 0 rows")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "Count")]
        public virtual void CountANumberOfRecordsInARecordsetWith0Rows()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Count a number of records in a recordset with 0 rows", ((string[])(null)));
#line 54
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "rs"});
#line 55
 testRunner.Given("I have a recordset with this shape", ((string)(null)), table7, "Given ");
#line 57
 testRunner.And("count on record \"[[rs()]]\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
 testRunner.When("the count tool is executed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 59
 testRunner.Then("the result count should be 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 60
 testRunner.And("the execution has \"NO\" error", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "Recordset"});
            table8.AddRow(new string[] {
                        "[[rs()]] ="});
#line 61
 testRunner.And("the debug inputs as", ((string)(null)), table8, "And ");
#line hidden
            TechTalk.SpecFlow.Table table9 = new TechTalk.SpecFlow.Table(new string[] {
                        ""});
            table9.AddRow(new string[] {
                        "[[result]] = 0"});
#line 64
 testRunner.And("the debug output as", ((string)(null)), table9, "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion

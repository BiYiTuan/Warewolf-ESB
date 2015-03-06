﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Warewolf.AcceptanceTesting.Explorer.DBServiceSpecs
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute()]
    public partial class NewDatabaseSourceFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "New Database Source.feature"
#line hidden
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.ClassInitializeAttribute()]
        public static void FeatureSetup(Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContext)
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "New Database Source", "In order to avoid silly mistakes\r\nAs a math idiot\r\nI want to be told the sum of t" +
                    "wo numbers", ProgrammingLanguage.CSharp, new string[] {
                        "CreatingNewDBSource"});
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
                        && (TechTalk.SpecFlow.FeatureContext.Current.FeatureInfo.Title != "New Database Source")))
            {
                Warewolf.AcceptanceTesting.Explorer.DBServiceSpecs.NewDatabaseSourceFeature.FeatureSetup(null);
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
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Creating New DB Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New Database Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CreatingNewDBSource")]
        public virtual void CreatingNewDBSource()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating New DB Source", ((string[])(null)));
#line 28
this.ScenarioSetup(scenarioInfo);
#line 29
   testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 30
   testRunner.And("I type Server as \"RSAKLFSVRGENDEV\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
   testRunner.And("Database dropdown is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 32
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 33
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 34
   testRunner.And("I Select Authentication Type as \"Windows\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 35
   testRunner.Then("Username field is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 36
   testRunner.And("Password field is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 37
   testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 38
   testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 39
   testRunner.When("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 40
   testRunner.Then("Database dropdown is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 41
   testRunner.Then("I select \"Dev2TestingDB\" as Database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 42
   testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 43
   testRunner.When("I save the source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 44
   testRunner.Then("the save dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Creating New DB Source under auth type as user")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New Database Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CreatingNewDBSource")]
        public virtual void CreatingNewDBSourceUnderAuthTypeAsUser()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Creating New DB Source under auth type as user", ((string[])(null)));
#line 47
this.ScenarioSetup(scenarioInfo);
#line 48
    testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 49
    testRunner.And("I type Server as \"RSAKLFSVRGENDEV\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 50
    testRunner.And("Database dropdown is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 51
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 52
    testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 53
    testRunner.And("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 54
    testRunner.Then("Username field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 55
    testRunner.And("Password field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 56
    testRunner.And("\"Test Connection\" is \"Disbled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 57
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 58
    testRunner.When("I type Username as \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 59
    testRunner.And("I type Password as \"test123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 60
    testRunner.Then("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 61
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 62
    testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 63
    testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 64
    testRunner.Then("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 65
    testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 66
    testRunner.And("Database dropdown is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
    testRunner.When("I select \"Dev2TestingDB\" as Database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 68
    testRunner.Then("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 69
    testRunner.When("I save the source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 70
    testRunner.Then("the save dialog is opened", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Incorrect Server address wind auth type allowing to save")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New Database Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CreatingNewDBSource")]
        public virtual void IncorrectServerAddressWindAuthTypeAllowingToSave()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Incorrect Server address wind auth type allowing to save", ((string[])(null)));
#line 72
this.ScenarioSetup(scenarioInfo);
#line 73
      testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 74
   testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 75
      testRunner.And("I type Server as \"Incorrect\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 76
      testRunner.And("Database dropdown is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 77
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 78
      testRunner.And("I Select Authentication Type as \"Windows\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 79
      testRunner.Then("Username field is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 80
      testRunner.And("Password field is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 81
      testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 82
      testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 83
      testRunner.When("Test Connecton is \"Unsuccessful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 84
      testRunner.And("the validation message as \"Server not found\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 85
      testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 86
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Incorrect Server source with user auth type is not allowing to save")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New Database Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CreatingNewDBSource")]
        public virtual void IncorrectServerSourceWithUserAuthTypeIsNotAllowingToSave()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Incorrect Server source with user auth type is not allowing to save", ((string[])(null)));
#line 89
this.ScenarioSetup(scenarioInfo);
#line 90
      testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 91
      testRunner.And("I type Server as \"Incorrect\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 92
      testRunner.And("Database dropdown is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 93
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 94
      testRunner.And("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 95
      testRunner.Then("Username field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 96
      testRunner.And("Password field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 97
   testRunner.When("I type Username as \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 98
   testRunner.And("I type Password as \"test123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 99
      testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 100
      testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 101
      testRunner.Then("Test Connecton is \"Unsuccessful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 102
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 103
      testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute()]
        [Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute("Testing as Windows and swaping it resets the test connection")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestPropertyAttribute("FeatureTitle", "New Database Source")]
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestCategoryAttribute("CreatingNewDBSource")]
        public virtual void TestingAsWindowsAndSwapingItResetsTheTestConnection()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Testing as Windows and swaping it resets the test connection", ((string[])(null)));
#line 107
this.ScenarioSetup(scenarioInfo);
#line 108
      testRunner.Given("I open New Database Source", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 109
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
      testRunner.And("I type Server as \"RSAKLFSVRGENDEV\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
      testRunner.And("Database dropdown is \"Invisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 112
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 113
      testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 114
      testRunner.And("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 115
      testRunner.Then("Username field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 116
      testRunner.And("Password field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 117
      testRunner.And("\"Test Connection\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 118
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 119
      testRunner.When("I type Username as \"testuser\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 120
      testRunner.And("I type Password as \"test123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 121
      testRunner.Then("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 122
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 123
      testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 124
      testRunner.Then("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 125
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 126
      testRunner.And("Database dropdown is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 127
      testRunner.When("I select \"Dev2TestingDB\" as Database", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 128
      testRunner.And("I Select Authentication Type as \"Windows\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 129
      testRunner.Then("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 130
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 131
      testRunner.Then("Test Connecton is \"Successful\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 132
      testRunner.Then("Database dropdown is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 133
      testRunner.And("\"Save\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 134
      testRunner.When("I Select Authentication Type as \"User\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 135
      testRunner.Then("Username field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 136
      testRunner.And("Password field is \"Visible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 137
      testRunner.And("\"Test Connection\" is \"Enabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 138
      testRunner.And("\"Save\" is \"Disabled\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 139
   testRunner.Then("Database dropdown is \"InVisible\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
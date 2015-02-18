﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Dev2.Common.Interfaces.Explorer;
using Dev2.Common.Interfaces.Runtime.ServiceModel;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.Studio.ViewModels;
using Dev2.Common.Interfaces.Studio.ViewModels.Dialogues;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TechTalk.SpecFlow;
using Warewolf.Studio.ServerProxyLayer;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.Views;

namespace Warewolf.AcceptanceTesting.Explorer.DBService_Specs
{
    [Binding]
    public class NewDatabaseSourceSteps
    {
        [BeforeFeature("CreatingNewDBSource")]
        public static void SetupForSystem()
        {
            var bootStrapper = new UnityBootstrapperForDatabaseSourceConnectorTesting();
            bootStrapper.Run();
            var databaseSourceControlView = new ManageDatabaseSourceControl();
            var mockStudioUpdateManager = new Mock<IStudioUpdateManager>();
            var mockRequestServiceNameViewModel = new Mock<IRequestServiceNameViewModel>();
            var manageDatabaseSourceControlViewModel = new ManageDatabaseSourceViewModel(mockStudioUpdateManager.Object,mockRequestServiceNameViewModel.Object);
            databaseSourceControlView.DataContext = manageDatabaseSourceControlViewModel;
            var window = new Window { Content = databaseSourceControlView };
            var app = Application.Current;
            app.MainWindow = window;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() =>
            {
                var manageDatabaseSourceControl = (ManageDatabaseSourceControl)Application.Current.MainWindow.Content;
                Assert.IsNotNull(manageDatabaseSourceControl);
                Assert.IsNotNull(manageDatabaseSourceControl.DataContext);
                Assert.IsInstanceOfType(manageDatabaseSourceControl.DataContext, typeof(IManageDatabaseSourceViewModel));
                Application.Current.Shutdown();
            }));

            Application.Current.Run(Application.Current.MainWindow);
            FeatureContext.Current.Add("databaseView", databaseSourceControlView);
            FeatureContext.Current.Add("updateManager", mockStudioUpdateManager);
            FeatureContext.Current.Add("requestServiceNameViewModel", mockRequestServiceNameViewModel);
        }
        
        [BeforeScenario("CreatingNewDBSource")]
        public void SetupForDatabaseSource()
        {
            ScenarioContext.Current.Add("databaseView", FeatureContext.Current.Get<ManageDatabaseSourceControl>("databaseView"));
            ScenarioContext.Current.Add("updateManager", FeatureContext.Current.Get<Mock<IStudioUpdateManager>>("updateManager"));
            ScenarioContext.Current.Add("requestServiceNameViewModel", FeatureContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel"));
        }

        [Given(@"I open New Database Source")]
        public void GivenIOpenNewDatabaseSource()
        {
            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>("databaseView");
            Assert.IsNotNull(manageDatabaseSourceControl);
            Assert.IsNotNull(manageDatabaseSourceControl.DataContext); 
        }

        [Given(@"I type Server as ""(.*)""")]
        public void GivenITypeServerAs(string serverName)
        {
            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>("databaseView");
            manageDatabaseSourceControl.EnterServerName(serverName);
        }

        [Given(@"Database dropdown is ""(.*)""")]
        [When(@"Database dropdown is ""(.*)""")]
        [Then(@"Database dropdown is ""(.*)""")]
        public void GivenDropdownIs(string visibility)
        {
            var expectedVisibility = String.Equals(visibility, "Invisible", StringComparison.InvariantCultureIgnoreCase) ? Visibility.Collapsed : Visibility.Visible;

            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>("databaseView");
            var databaseDropDownVisibility = manageDatabaseSourceControl.GetDatabaseDropDownVisibility();
            Assert.AreEqual(expectedVisibility,databaseDropDownVisibility);
        }

        [Given(@"""(.*)"" is ""(.*)""")]
        [When(@"""(.*)"" is ""(.*)""")]
        [Then(@"""(.*)"" is ""(.*)""")]
        public void GivenIs(string controlName, string enabledString)
        {
            var isEnabled = String.Equals(enabledString, "Enabled", StringComparison.InvariantCultureIgnoreCase);
            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>("databaseView");
            var controlEnabled = manageDatabaseSourceControl.GetControlEnable(controlName);
            Assert.AreEqual(isEnabled,controlEnabled);
        }

        [Given(@"I Select Authentication Type as ""(.*)""")]
        public void GivenISelectAuthenticationTypeAs(string authenticationTypeString)
        {
            var authenticationType = String.Equals(authenticationTypeString, "Windows",
                StringComparison.InvariantCultureIgnoreCase)
                ? AuthenticationType.Windows
                : AuthenticationType.User;

            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>("databaseView");
            manageDatabaseSourceControl.SetAuthenticationType(authenticationType);
        }


        [Given(@"I select ""(.*)"" as Database")]
        [When(@"I select ""(.*)"" as Database")]
        [Then(@"I select ""(.*)"" as Database")]
        public void WhenISelectAsDatabase(string databaseName)
        {
            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>("databaseView");
            manageDatabaseSourceControl.SelectDatabase(databaseName);
        }

        [When(@"I save the source")]
        public void WhenISaveTheSource()
        {
            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            mockRequestServiceNameViewModel.Setup(model => model.ShowSaveDialog()).Verifiable();
            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>("databaseView");
            manageDatabaseSourceControl.PerformSave();

        }

        [Then(@"Username field is ""(.*)""")]
        public void ThenUsernameFieldIs(string visibility)
        {
            var expectedVisibility = String.Equals(visibility, "Invisible", StringComparison.InvariantCultureIgnoreCase) ? Visibility.Collapsed : Visibility.Visible;

            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>("databaseView");
            var databaseDropDownVisibility = manageDatabaseSourceControl.GetUsernameVisibility();
            Assert.AreEqual(expectedVisibility, databaseDropDownVisibility);
        }

        [Then(@"Password field is ""(.*)""")]
        public void ThenPasswordFieldIs(string visibility)
        {
            var expectedVisibility = String.Equals(visibility, "Invisible", StringComparison.InvariantCultureIgnoreCase) ? Visibility.Collapsed : Visibility.Visible;

            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>("databaseView");
            var databaseDropDownVisibility = manageDatabaseSourceControl.GetPasswordVisibility();
            Assert.AreEqual(expectedVisibility, databaseDropDownVisibility);
        }

        [Then(@"Test Connecton is ""(.*)""")]
        [When(@"Test Connecton is ""(.*)""")]
        public void ThenTestConnectonIs(string successString)
        {
            var mockUpdateManager = ScenarioContext.Current.Get<Mock<IStudioUpdateManager>>("updateManager");
            var isSuccess = String.Equals(successString, "Successful", StringComparison.InvariantCultureIgnoreCase);
            if (isSuccess)
            {
                mockUpdateManager.Setup(manager => manager.TestDbConnection(It.IsAny<IDbSource>()))
                    .Returns(new List<string> {"Dev2TestingDB"});
            }
            else
            {
                mockUpdateManager.Setup(manager => manager.TestDbConnection(It.IsAny<IDbSource>()))
                    .Throws(new WarewolfTestException("Test Failed", null));

            }
            var manageDatabaseSourceControl = ScenarioContext.Current.Get<ManageDatabaseSourceControl>("databaseView");
            manageDatabaseSourceControl.PerformTestConnection();
        }

        [Then(@"the save dialog is opened")]
        public void ThenTheSaveDialogIsOpened()
        {
            var mockRequestServiceNameViewModel = ScenarioContext.Current.Get<Mock<IRequestServiceNameViewModel>>("requestServiceNameViewModel");
            mockRequestServiceNameViewModel.Verify();
        }
    }
}

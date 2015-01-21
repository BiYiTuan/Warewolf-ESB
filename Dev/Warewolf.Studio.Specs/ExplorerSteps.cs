﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Toolbox;
using Dev2.Runtime.ServiceModel.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;
using Warewolf.Studio.ViewModels;

namespace Warewolf.Studio.Specs
{
    [Binding]
    public class ExplorerSteps
    {
        [Given(@"I have connected to localhost")]
        public void GivenIHaveConnectedToLocalhost()
        {
            var localHostEnvironment = new EnvironmentViewModel(new Server(null),null)
            {
                DisplayName = "localhost"                
            };
            localHostEnvironment.Connect();
            Assert.IsTrue(localHostEnvironment.IsConnected);
            ScenarioContext.Current.Add("localhost",localHostEnvironment);
        }

        [Given(@"localhost has loaded")]
        [When(@"localhost has loaded")]
        [Then(@"localhost has loaded")]
        public void WhenLocalhostHasLoaded()
        {
            EnvironmentViewModel localHostEnvironment;
            if (ScenarioContext.Current.TryGetValue("localhost", out localHostEnvironment))
            {
                localHostEnvironment.Load();
                Assert.IsTrue(localHostEnvironment.IsLoaded);
            }
        }

        [Then(@"I should see localhost resources")]
        public void ThenIShouldSeeLocalhostResources()
        {
            Assert.Fail("Check for localhost resources");
        }
    }

    public class Server : Resource,IServer
    {
        readonly IStudioResourceRepository _resourceRepository;

        #region Implementation of IServer

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Server(IStudioResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public Task<bool> Connect()
        {
            return new Task<bool>(() => true);
        }

        public IList<IResource> Load()
        {
            return new List<IResource>();
        }

        public IList<IServer> GetServerConnections()
        {
            return new List<IServer>();
        }

        public IList<IToolDescriptor> LoadTools()
        {
            return new List<IToolDescriptor>();
        }

        public bool IsConnected()
        {
            return false;
        }

        public void ReloadTools()
        {
        }

        public void Disconnect()
        {
        }

        public void Edit()
        {
        }

        #endregion


    }
}

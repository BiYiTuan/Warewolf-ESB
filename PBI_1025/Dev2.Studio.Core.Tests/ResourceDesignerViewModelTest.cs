﻿using Dev2.Composition;
using Dev2.Studio.Core.AppResources.Enums;
using Dev2.Studio.Core.Interfaces;
using Dev2.Studio.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Dev2.Core.Tests
{
    
    
    /// <summary>
    ///This is a result class for ResourceDesignerViewModelTest and is intended
    ///to contain all ResourceDesignerViewModelTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ResourceDesignerViewModelTest
    { 

        ResourceDesignerViewModel target;

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the result context which provides
        ///information about and functionality for the current result run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional result attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first result in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //
        //Use TestCleanup to run code after each result has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestInitialize]
        public void MyTestInitialize() {

            ImportService.CurrentContext = CompositionInitializer.InitializeForMeflessBaseViewModel();

            var m = new Mock<IContextualResourceModel>();
            m.Setup(c => c.ServiceDefinition).Returns("result");
            m.Setup(c => c.ResourceType).Returns(ResourceType.Service);

            IContextualResourceModel model = m.Object;
            IEnvironmentModel environment = null; // TODO: Initialize to an appropriate value
            target = new ResourceDesignerViewModel(model, environment);
        }


        #region CTOR Tests
        /// <summary>
        ///A result for ResourceDesignerViewModel Constructor
        ///</summary>
        [TestMethod()]
        public void ResourceDesignerViewModelConstructorTest()
        {
            // init 
        }

        /// <summary>
        /// Tests that the constructor can handle null resourceModel being passed to it
        /// </summary>
        [TestMethod]
        public void Constructor_NullResourceModel_Expected_ConstructorCreatesNewResourceModel() {
        }

        /// <summary>
        /// Tests that the constructor complains when a null environmentmodel is passed in
        /// </summary>
        [TestMethod]
        public void Constructor_NullEnvironmentModel_Expected_ExceptionThrown() {
        }

        #endregion CTOR

        #region DefaultDefinition Tests

        /// <summary>
        /// Tests that the Default Service Definition for Services
        ///</summary>
        [TestMethod]
        public void DefaultDefinition_ServiceType_Expected_ServiceDefinitionBuiltForService()
        {
            Mock<IContextualResourceModel> m = new Mock<IContextualResourceModel>();
            m.Setup(c => c.ServiceDefinition).Returns(string.Empty).Verifiable();
            m.Setup(c => c.ResourceType).Returns(ResourceType.Service);
            IEnvironmentModel environment = null; // TODO: Initialize to an appropriate value
            target = new ResourceDesignerViewModel(m.Object, environment);
            string actual = target.ServiceDefinition;
            m.Verify(c => c.ServiceDefinition, Times.Exactly(2));
        }

        /// <summary>
        /// Tests that the Default Service Definition for Sources
        ///</summary>
        [TestMethod]
        public void DefaultDefinition_SourceType_Expected_ServiceDefinitionBuiltForService() {
            Mock<IContextualResourceModel> m = new Mock<IContextualResourceModel>();
            m.Setup(c => c.ServiceDefinition).Returns(string.Empty).Verifiable();
            m.Setup(c => c.ResourceType).Returns(ResourceType.Source);
            IEnvironmentModel environment = null; // TODO: Initialize to an appropriate value
            target = new ResourceDesignerViewModel(m.Object, environment);
            string actual = target.ServiceDefinition;
            m.Verify(c => c.ServiceDefinition, Times.Exactly(2));
        }

        #endregion DefaultDefinition Tests

        #region UpdateServiceDefinition Tests

        /// <summary>
        ///A result for UpdateServiceDefinition
        ///</summary>
        [TestMethod()]
        public void UpdateServiceDefinition()
        {
            target.ServiceDefinition = "result";

            Assert.IsTrue(target.ServiceDefinition == "result");
        }

        #endregion UpdateServiceDefinition Tests
    }
}

﻿using System.Collections.Generic;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Studio.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Warewolf.Studio.ViewModels.Tests
{
    [TestClass]
    public class ExplorerViewModelTests
    {

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ExplorerViewModel_Filter")]
        public void ExplorerViewModel_Filter_NullEnvironments_ShouldNotCallFilterOnEachEnvironment()
        {
            //------------Setup for test--------------------------
            IExplorerViewModel explorerViewModel = new ExplorerViewModel(new Mock<IShellViewModel>().Object);
            var mockEnv1 = new Mock<IEnvironmentViewModel>();
            mockEnv1.Setup(model => model.Filter(It.IsAny<string>())).Verifiable();
            var mockEnv2 = new Mock<IEnvironmentViewModel>();
            mockEnv2.Setup(model => model.Filter(It.IsAny<string>())).Verifiable();
            explorerViewModel.Environments = null;
            //------------Execute Test---------------------------
            explorerViewModel.Filter("TestValue");
            //------------Assert Results-------------------------
            mockEnv1.Verify(model => model.Filter(It.IsAny<string>()),Times.Never());
            mockEnv2.Verify(model => model.Filter(It.IsAny<string>()),Times.Never());
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ExplorerViewModel_Filter")]
        public void ExplorerViewModel_Filter_ShouldCallFilterOnEachEnvironment()
        {
            //------------Setup for test--------------------------
            IExplorerViewModel explorerViewModel = new ExplorerViewModel(new Mock<IShellViewModel>().Object);
            var mockEnv1 = new Mock<IEnvironmentViewModel>();
            mockEnv1.Setup(model => model.Filter(It.IsAny<string>())).Verifiable();
            var environment1 = mockEnv1.Object;
            var mockEnv2 = new Mock<IEnvironmentViewModel>();
            mockEnv2.Setup(model => model.Filter(It.IsAny<string>())).Verifiable();
            var environment2 = mockEnv2.Object;
            explorerViewModel.Environments = new List<IEnvironmentViewModel>{environment1,environment2};
            //------------Execute Test---------------------------
            explorerViewModel.Filter("TestValue");
            //------------Assert Results-------------------------
            mockEnv1.Verify();
            mockEnv2.Verify();
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ExplorerViewModel_SearchText")]
        public void ExplorerViewModel_SearchText_WhenUpdate_ShouldCallFilter()
        {
            //------------Setup for test--------------------------
            ExplorerViewModel explorerViewModel = new ExplorerViewModel(new Mock<IShellViewModel>().Object);
            var mockEnv1 = new Mock<IEnvironmentViewModel>();
            mockEnv1.Setup(model => model.Filter(It.IsAny<string>())).Verifiable();
            var environment1 = mockEnv1.Object;
            var mockEnv2 = new Mock<IEnvironmentViewModel>();
            mockEnv2.Setup(model => model.Filter(It.IsAny<string>())).Verifiable();
            var environment2 = mockEnv2.Object;
            explorerViewModel.Environments = new List<IEnvironmentViewModel> { environment1, environment2 };
            var propertyChangedFired = false;
            explorerViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "SearchText")
                {
                    propertyChangedFired = true;
                }
            };
            //------------Execute Test---------------------------
            explorerViewModel.SearchText = "TestValue";
            //------------Assert Results-------------------------
            mockEnv1.Verify();
            mockEnv2.Verify();
            Assert.IsTrue(propertyChangedFired);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ExplorerViewModel_SearchText")]
        public void ExplorerViewModel_SearchText_WhenNotUpdated_ShouldCallFilter()
        {
            //------------Setup for test--------------------------
            ExplorerViewModel explorerViewModel = new ExplorerViewModel(new Mock<IShellViewModel>().Object);
            var mockEnv1 = new Mock<IEnvironmentViewModel>();
            mockEnv1.Setup(model => model.Filter(It.IsAny<string>())).Verifiable();
            var environment1 = mockEnv1.Object;
            var mockEnv2 = new Mock<IEnvironmentViewModel>();
            mockEnv2.Setup(model => model.Filter(It.IsAny<string>())).Verifiable();
            var environment2 = mockEnv2.Object;
            explorerViewModel.Environments = new List<IEnvironmentViewModel> { environment1, environment2 };
            explorerViewModel.SearchText = "TestValue";
            var propertyChangedFired = false;
            explorerViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "SearchText")
                {
                    propertyChangedFired = true;
                }
            };
            
            //------------Execute Test---------------------------
            explorerViewModel.SearchText = "TestValue";
            //------------Assert Results-------------------------
            mockEnv1.Verify(model => model.Filter(It.IsAny<string>()),Times.Once());
            mockEnv2.Verify(model => model.Filter(It.IsAny<string>()), Times.Once());
            Assert.IsFalse(propertyChangedFired);
        }
    }
}

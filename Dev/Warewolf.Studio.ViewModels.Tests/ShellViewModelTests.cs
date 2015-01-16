﻿using System;
using System.Linq;
using System.Windows.Input;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Studio;
using Dev2.Common.Interfaces.Studio.ViewModels;
using Dev2.Common.Interfaces.Toolbox;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Warewolf.Studio.Core.View_Interfaces;

// ReSharper disable RedundantTypeArgumentsOfMethod

namespace Warewolf.Studio.ViewModels.Tests
{
    [TestClass]
    public class ShellViewModelTests
    {
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ShellViewModel_Constructor")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShellViewModel_Constructor_NullContainer_NullExceptionThrown()
        {
            //------------Setup for test--------------------------
            
            //------------Execute Test---------------------------
            new ShellViewModel(null, new Mock<IRegionManager>().Object);
            //------------Assert Results-------------------------
        } 
        
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ShellViewModel_Constructor")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShellViewModel_Constructor_NullRegionManager_NullExceptionThrown()
        {
            //------------Setup for test--------------------------
            
            //------------Execute Test---------------------------
            new ShellViewModel(new Mock<IUnityContainer>().Object, null);
            //------------Assert Results-------------------------
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ShellViewModel_Initialize")]
        public void ShellViewModel_Initialize_Should_AddViewsToRegions()
        {
            //------------Setup for test--------------------------
            var testContainer = new UnityContainer();
            var testRegionManager = new RegionManager();
            testRegionManager.Regions.Add("Explorer",new Region());
            testRegionManager.Regions.Add("Toolbox",new Region());
            testRegionManager.Regions.Add("Menu",new Region());
            SetupContainer(testContainer);
            var shellViewModel = new ShellViewModel(testContainer, testRegionManager);
            //------------Execute Test---------------------------
            shellViewModel.Initialize();
            //------------Assert Results-------------------------
            Assert.IsNotNull(shellViewModel);
            Assert.IsTrue(shellViewModel.RegionHasView("Explorer"));
            Assert.IsTrue(shellViewModel.RegionHasView("Toolbox"));
            Assert.IsTrue(shellViewModel.RegionHasView("Menu"));
        }

        static void SetupContainer(UnityContainer testContainer)
        {
            testContainer.RegisterInstance<IExplorerView>(new Mock<IExplorerView>().Object);
            testContainer.RegisterInstance<IToolboxView>(new Mock<IToolboxView>().Object);
            testContainer.RegisterInstance<IMenuView>(new Mock<IMenuView>().Object);

            testContainer.RegisterInstance<IExplorerViewModel>(new Mock<IExplorerViewModel>().Object);
            testContainer.RegisterInstance<IToolboxViewModel>(new Mock<IToolboxViewModel>().Object);
            testContainer.RegisterInstance<IMenuViewModel>(new Mock<IMenuViewModel>().Object);
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ShellViewModel_Initialize")]
        public void ShellViewModel_Initialize_Should_SetViewModelAsDataContextForRegionViews()
        {
            //------------Setup for test--------------------------
            var testContainer = new UnityContainer();
            var testRegionManager = new RegionManager();
            testRegionManager.Regions.Add("Explorer",new Region());
            testRegionManager.Regions.Add("Toolbox",new Region());
            testRegionManager.Regions.Add("Menu",new Region());
            testContainer.RegisterInstance<IExplorerViewModel>(new Mock<IExplorerViewModel>().Object);
            testContainer.RegisterInstance<IToolboxViewModel>(new Mock<IToolboxViewModel>().Object);
            testContainer.RegisterInstance<IMenuViewModel>(new Mock<IMenuViewModel>().Object);

            var mockExplorerView = new Mock<IExplorerView>();
            mockExplorerView.SetupProperty(view => view.DataContext);
            testContainer.RegisterInstance<IExplorerView>(mockExplorerView.Object);
            var mockToolBoxView = new Mock<IToolboxView>();
            mockToolBoxView.SetupProperty(view => view.DataContext);
            testContainer.RegisterInstance<IToolboxView>(mockToolBoxView.Object);
            var mockMenuView = new Mock<IMenuView>();
            mockMenuView.SetupProperty(view => view.DataContext);
            testContainer.RegisterInstance<IMenuView>(mockMenuView.Object);
            var shellViewModel = new ShellViewModel(testContainer, testRegionManager);
            //------------Execute Test---------------------------
            shellViewModel.Initialize();
            //------------Assert Results-------------------------
            Assert.IsNotNull(shellViewModel);
            Assert.IsTrue(shellViewModel.RegionViewHasDataContext("Explorer"));
            Assert.IsTrue(shellViewModel.RegionViewHasDataContext("Toolbox"));
            Assert.IsTrue(shellViewModel.RegionViewHasDataContext("Menu"));
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ShellViewModel_AddService")]
        public void ShellViewModel_AddService_WhenNotInTab_ShouldAddServiceViewModelToTab()
        {
            //------------Setup for test--------------------------
            var testContainer = new UnityContainer();
            testContainer.RegisterType<IServiceDesignerViewModel, MockServiceDesignerViewModel>();
            var testRegionManager = new RegionManager();
            testRegionManager.Regions.Add("Workspace", new SingleActiveRegion());
            var shellViewModel = new ShellViewModel(testContainer, testRegionManager);
            //------------Execute Test---------------------------
            shellViewModel.AddService(new Mock<IResource>().Object);
            //------------Assert Results-------------------------
            Assert.IsTrue(shellViewModel.RegionHasView("Workspace"));
        }

        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ShellViewModel_AddService")]
        public void ShellViewModel_AddService_SameResource_ShouldNotAddAnotherTab()
        {
            //------------Setup for test--------------------------
            var testContainer = new UnityContainer();
            testContainer.RegisterType<IServiceDesignerViewModel, MockServiceDesignerViewModel>();
            var testRegionManager = new RegionManager();
            testRegionManager.Regions.Add("Workspace", new SingleActiveRegion());
            var shellViewModel = new ShellViewModel(testContainer, testRegionManager);
            var resource = new Mock<IResource>().Object;
            shellViewModel.AddService(resource);
            //------------Execute Test---------------------------
            shellViewModel.AddService(resource);
            //------------Assert Results-------------------------
            var viewsCollection = shellViewModel.GetRegionViews("Workspace");
            Assert.AreEqual(1,viewsCollection.Count());
        }
        
        [TestMethod]
        [Owner("Hagashen Naidu")]
        [TestCategory("ShellViewModel_AddService")]
        public void ShellViewModel_AddService_DifferentResource_ShouldAddAnotherTab()
        {
            //------------Setup for test--------------------------
            var testContainer = new UnityContainer();
            testContainer.RegisterType<IServiceDesignerViewModel, MockServiceDesignerViewModel>();
            var testRegionManager = new RegionManager();
            testRegionManager.Regions.Add("Workspace", new SingleActiveRegion());
            var shellViewModel = new ShellViewModel(testContainer, testRegionManager);
            shellViewModel.AddService(new Mock<IResource>().Object);
            //------------Execute Test---------------------------
            shellViewModel.AddService(new Mock<IResource>().Object);
            //------------Assert Results-------------------------
            var viewsCollection = shellViewModel.GetRegionViews("Workspace");
            Assert.AreEqual(2,viewsCollection.Count());
        }
    }

    internal class MockServiceDesignerViewModel : IServiceDesignerViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public MockServiceDesignerViewModel(IResource resource)
        {
            Resource = resource;
        }

        #region Implementation of IServiceDesignerViewModel

        public IResource Resource
        {
            get;
            set;
        }
        /// <summary>
        /// Should the hyperlink to execute the service in browser
        /// </summary>
        public bool IsServiceLinkVisible
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// Command to execute when the hyperlink is clicked
        /// </summary>
        public ICommand OpenWorkflowLinkCommand
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// The hyperlink text shown
        /// </summary>
        public string DisplayWorkflowLink
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// The designer for the resource
        /// </summary>
        public IDesignerView DesignerView
        {
            get
            {
                return null;
            }
        }

        #endregion
    }
}

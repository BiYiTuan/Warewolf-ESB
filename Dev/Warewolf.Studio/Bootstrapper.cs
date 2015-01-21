﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Studio;
using Dev2.Common.Interfaces.Studio.ViewModels;
using Dev2.Common.Interfaces.Toolbox;
using Infragistics.Themes;
using Dev2.Common.Interfaces.DataList.DatalistView;
using Infragistics.Windows.DockManager;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Moq;
using Warewolf.Studio.Core.Infragistics_Prism_Region_Adapter;
using Warewolf.Studio.Core.View_Interfaces;
using Warewolf.Studio.Themes.Luna;
using Warewolf.Studio.ViewModels;
using Warewolf.Studio.ViewModels.DummyModels;
using Warewolf.Studio.ViewModels.VariableList;
using Warewolf.Studio.Views;

namespace Warewolf.Studio
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        #region Overrides of UnityBootstrapper

        /// <summary>
        /// Configures the <see cref="T:Microsoft.Practices.Unity.IUnityContainer"/>. May be overwritten in a derived class to add specific
        ///             type mappings required by the application.
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            ThemeManager.ApplicationTheme = new LunaTheme();
            Container.RegisterInstance<IShellViewModel>(new ShellViewModel(Container,Container.Resolve<IRegionManager>()));
            Container.RegisterInstance<IExplorerViewModel>(new DummyExplorerViewModel(Container.Resolve<IShellViewModel>()));
            Container.RegisterInstance<IToolboxViewModel>(new DummyToolboxViewModel());
            Container.RegisterInstance<IMenuViewModel>(new DummyMenuViewModel());

            Container.RegisterInstance<IExplorerView>(new ExplorerView());
            Container.RegisterInstance<IToolboxView>(new ToolboxView());
            Container.RegisterInstance<IMenuView>(new MenuView());
         

     
            ICollection<IVariableListViewColumnViewModel> colls = new ObservableCollection<IVariableListViewColumnViewModel>();
            colls.Add(new VariableListColumnViewModel("col", "bob", new Mock<Dev2.Common.Interfaces.DataList.DatalistView.IVariableListViewModel>().Object, colls) { Input = true });

            var convertedRecset = new VariableListViewRecordSetViewModel("bob", colls, new Mock<Dev2.Common.Interfaces.DataList.DatalistView.IVariableListViewModel>().Object, new List<IVariablelistViewRecordSetViewModel>());
            var convertedRecset2 = new VariableListViewRecordSetViewModel("dave", new VariableListColumnViewModel[0], new Mock<Dev2.Common.Interfaces.DataList.DatalistView.IVariableListViewModel>().Object, new List<IVariablelistViewRecordSetViewModel>());
            var convertedScalar = new VariableListItemViewScalarViewModel("martha", new Mock<Dev2.Common.Interfaces.DataList.DatalistView.IVariableListViewModel>().Object, new List<IVariableListViewScalarViewModel>());
            var convertedScalar2 = new VariableListItemViewScalarViewModel("stewart", new Mock<Dev2.Common.Interfaces.DataList.DatalistView.IVariableListViewModel>().Object, new List<IVariableListViewScalarViewModel>()) { Used = false };
            var expressions = new List<IDataExpression> { new Mock<IDataExpression>().Object, new Mock<IDataExpression>().Object, new Mock<IDataExpression>().Object, new Mock<IDataExpression>().Object };
            var convertor = new Mock<IDatalistViewExpressionConvertor>();
            convertor.Setup(a => a.Create(expressions[0])).Returns(convertedRecset);
            convertor.Setup(a => a.Create(expressions[1])).Returns(convertedRecset2);
            convertor.Setup(a => a.Create(expressions[2])).Returns(convertedScalar);
            convertor.Setup(a => a.Create(expressions[3])).Returns(convertedScalar2);
            Dev2.Common.Interfaces.DataList.DatalistView.IVariableListViewModel vm = new VariableListViewModel(expressions, convertor.Object);
            var variableList = new VariableListView { DataContext = vm };
            Container.RegisterInstance<IVariableListView>(new VariableListView());
            Container.RegisterInstance<Dev2.Common.Interfaces.DataList.DatalistView.IVariableListViewModel>(vm);
        }

        #endregion

        protected override void InitializeShell()
        {
            base.InitializeShell();

            var window = (Window)Shell;
            window.Show();

        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings mappings = base.ConfigureRegionAdapterMappings();
            mappings.RegisterMapping(typeof(TabGroupPane), Container.Resolve<TabGroupPaneRegionAdapter>());
            return mappings;
        }

    }
}

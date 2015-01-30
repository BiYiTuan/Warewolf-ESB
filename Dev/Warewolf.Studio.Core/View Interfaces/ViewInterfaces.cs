﻿using System;
using System.Collections.Generic;
using Dev2.Common.Interfaces.Studio.ViewModels;
using Microsoft.Practices.Prism.Mvvm;

namespace Warewolf.Studio.Core.View_Interfaces
{

    public interface IWarewolfView : IView
    {
        void Blur();
        void UnBlur();
    }
    public interface IExplorerView : IWarewolfView
    {
        IEnvironmentViewModel OpenEnvironmentNode(string nodeName);
        List<IExplorerItemViewModel> GetFoldersVisible();
        IExplorerItemViewModel OpenFolderNode(string folderName);
        int GetVisibleChildrenCount(string folderName);
        void PerformFolderRename(string originalFolderName, string newFolderName);
        void PerformSearch(string searchTerm);

    }
    public interface IToolboxView : IWarewolfView { }
    public interface IMenuView : IWarewolfView { }
    public interface IVariableListView : IWarewolfView { }
}

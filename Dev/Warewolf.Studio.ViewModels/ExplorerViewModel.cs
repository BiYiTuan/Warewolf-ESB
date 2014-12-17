﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Studio.ViewModels;
using Microsoft.Practices.Prism.Mvvm;

namespace Warewolf.Studio.ViewModels
{
    public class ExplorerViewModel:BindableBase,IExplorerViewModel
    {
        ICollection<IEnvironmentViewModel> _environments;
        public ICollection<IEnvironmentViewModel> Environments
        {
            get
            {
                return _environments;
            }
            set
            {
                _environments = value;
                OnPropertyChanged(() => Environments);
            }
        }
    }

    public class ExplorerItemViewModel : BindableBase,IExplorerItemViewModel
    {
        string _resourceName;

        public ExplorerItemViewModel()
        {
            Children = new ObservableCollection<IExplorerItemViewModel>();
        }

        #region Implementation of IExplorerItemViewModel

        public string ResourceName
        {
            get
            {
                return _resourceName;
            }
            set
            {
                _resourceName = value;
                OnPropertyChanged(() => ResourceName);
            }
        }
        public ICollection<IExplorerItemViewModel> Children
        {
            get;
            set;
        }
        public IResource Resource { get; set; }

        #endregion
    }
}

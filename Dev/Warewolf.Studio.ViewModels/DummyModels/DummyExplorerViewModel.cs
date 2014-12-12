﻿using System.Collections.Generic;
using Dev2.Common.Interfaces.Studio.ViewModels;

namespace Warewolf.Studio.ViewModels.DummyModels
{
    public class DummyExplorerViewModel:ExplorerViewModel
    {
        public DummyExplorerViewModel()
        {
            Environments = CreateEnvironments();
        }

        static List<IEnvironmentViewModel> CreateEnvironments()
        {
            var oneLevelDeep = new ExplorerItemViewModel
            {
                ResourceName = "One Level Deep",
            };
            oneLevelDeep.Children.Add(new ExplorerItemViewModel
            {
                ResourceName = "Resource One Level Deep"
            });
            var multiLevelDeep = new ExplorerItemViewModel
            {
                ResourceName = "Multi Level Deep"
            };
            multiLevelDeep.Children.Add(new ExplorerItemViewModel
            {
                ResourceName = "No children"
            });
            var childHasChildren = new ExplorerItemViewModel
            {
                ResourceName = "Has One Chid"
            };
            multiLevelDeep.Children.Add(childHasChildren);
            childHasChildren.Children.Add(new ExplorerItemViewModel
            {
                ResourceName = "Is child of child"
            });
            childHasChildren.Children.Add(new ExplorerItemViewModel
            {
                ResourceName = "Is Another child of a child"
            });
            return new List<IEnvironmentViewModel>
            {
                new EnviromentViewModel
                {
                    DisplayName = "Test1",
                    ExplorerItemViewModels = new List<IExplorerItemViewModel>
                    {
                        new ExplorerItemViewModel
                        {
                            ResourceName = "SingleLevel"
                        },
                        oneLevelDeep,
                        multiLevelDeep
                    }
                }, 
                new EnviromentViewModel { DisplayName = "Test4" }
            };
        }
    }
}

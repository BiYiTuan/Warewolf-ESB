using System;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Help;
using Dev2.Common.Interfaces.PopupController;
using Dev2.Common.Interfaces.Studio.ViewModels;

namespace Dev2.Common.Interfaces
{
    public interface IShellViewModel
    {
        void AddService(IResource resource);
        void DeployService(IExplorerItemViewModel resourceToDeploy);
        void UpdateHelpDescriptor(IHelpDescriptor helpDescriptor);
        void NewResource(ResourceType? type);
        IServer LocalhostServer { get; set; }

        void Handle(Exception err);

        bool ShowPopup(IPopupMessage getDeleteConfirmation);

        void RemoveServiceFromExplorer(IExplorerItemViewModel explorerItemViewModel);
    }
}
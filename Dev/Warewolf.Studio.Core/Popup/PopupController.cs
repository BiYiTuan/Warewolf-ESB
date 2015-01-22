﻿using System.Windows;
using Dev2;
using Dev2.Common.Interfaces.PopupController;

namespace Warewolf.Studio.Core.Popup
{
    public class PopupController:IPopupController
    {
        #region Implementation of IPopupController
        private readonly IPopupMessageBoxFactory _popupMessageBoxFactory;
        readonly IPopupWindow _popupWindow;

        public PopupController(IPopupMessageBoxFactory popupMessageBoxFactory,IPopupWindow popupWindow)
        {
            VerifyArgument.IsNotNull("popupMessageBoxFactory",popupMessageBoxFactory);
            _popupMessageBoxFactory = popupMessageBoxFactory;
            _popupWindow = popupWindow;
        }

        public MessageBoxResult Show(IPopupMessage message)
        {
            var window = _popupMessageBoxFactory.Create(message, _popupWindow);
            var result = window.Show();
            return result;
        }

        #endregion
    }
}

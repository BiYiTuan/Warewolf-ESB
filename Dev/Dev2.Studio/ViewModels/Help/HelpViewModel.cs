﻿using System.ComponentModel.Composition;
using Caliburn.Micro;
using CefSharp.Wpf;
using Dev2.Studio.Core.Messages;
using Dev2.Studio.ViewModels.WorkSurface;
using Dev2.Studio.Views.Help;

namespace Dev2.Studio.ViewModels.Help
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class HelpViewModel : BaseWorkSurfaceViewModel, 
        IHandle<TabClosedMessage>
    {
        private WebView _browser;
        private string _uri;

        public string Uri
        {
            get
            {
                return _uri;
            }
            set
            {
                if (_uri == value) return;

                _uri = value;
                NotifyOfPropertyChange(() => Uri);
            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            if (!(view is HelpView)) return;

            var helpView = (HelpView)view;
            _browser = helpView.BDSBrowser;
            _browser.Load(Uri);
        } 

        public void Handle(TabClosedMessage message)
        {
            if (!message.Context.Equals(this)) return;

            EventAggregator.Unsubscribe(this);
            _browser.Dispose();
        }
    }
}

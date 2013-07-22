﻿using System;
using System.ComponentModel;
using System.Net;
using Dev2.Studio.Core.Helpers;

namespace Dev2.Helpers
{
    class Dev2WebClient : IDev2WebClient
    {
        readonly WebClient _webClient;

        public Dev2WebClient(WebClient webClient)
        {
            _webClient = webClient;
        }

        public string DownloadString(string address)
        {
            using (var client = _webClient)
            {
                return client.DownloadString(address);
            }
        }

        public event DownloadProgressChangedEventHandler DownloadProgressChanged
        {
            add
            {
                _webClient.DownloadProgressChanged += value;
            }
            remove
            {
                _webClient.DownloadProgressChanged -= value;
            }
        }
        public event AsyncCompletedEventHandler DownloadFileCompleted
        {
            add
            {
                _webClient.DownloadFileCompleted += value;
            }
            remove
            {
                _webClient.DownloadFileCompleted -= value;
            }
        }
        public bool IsBusy { get; private set; }
        public void DownloadFileAsync(Uri address, string fileName, string userToken)
        {
            _webClient.DownloadFileAsync(address, fileName, userToken);
        }

        public void CancelAsync()
        {
            _webClient.CancelAsync();
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion
    }
}

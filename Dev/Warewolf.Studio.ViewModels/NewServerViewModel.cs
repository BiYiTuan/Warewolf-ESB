﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Dev2;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.Explorer;
using Dev2.Common.Interfaces.Runtime.ServiceModel;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Common.Interfaces.ServerDialogue;
using Dev2.Common.Interfaces.Studio.ViewModels.Dialogues;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace Warewolf.Studio.ViewModels
{
    public class NewServerViewModel : BindableBase, INewServerDialogue
    {
        string _userName;
        string _password;
        string _testMessage;
        string _address;
        bool _testPassed;
        AuthenticationType _authenticationType;

        #region Implementation of IInnerDialogueTemplate

        readonly IStudioUpdateManager _updateManager;
        readonly ISaveDialog _saveDialog;
        readonly IShellViewModel _shellViewModel;
        readonly string _connectedServer;

        string _headerText;
        IServerSource _serverSource;
        string[] _protocols;
        string _protocol;
        ObservableCollection<string> _ports;
        ObservableCollection<string> _servers; 
        string _selectedPort; 
        // ReSharper disable TooManyDependencies
        public NewServerViewModel(IServerSource newServerSource,
            IStudioUpdateManager updateManager, ISaveDialog saveDialog,
            IShellViewModel shellViewModel,
            string connectedServer)
        // ReSharper restore TooManyDependencies
        {
            VerifyArgument.AreNotNull(new Dictionary<string, object> { { "newServerSource", newServerSource }, { "updateManager", updateManager }, { "saveDialog", saveDialog } ,{"shellViewModel",shellViewModel},{"connectedServer",connectedServer}});
            Protocols = new[] { "http", "https" };
            Protocol = Protocols[0];
         
            Ports = new ObservableCollection<string> { "3142", "3143" };
            SelectedPort = Ports[0];
            _updateManager = updateManager;
            _saveDialog = saveDialog;
            _shellViewModel = shellViewModel;
            _connectedServer = connectedServer;
      
            ServerSource = newServerSource;
            _headerText = String.IsNullOrEmpty(newServerSource.Name) ? "New Server Source" : SetToEdit(newServerSource);

            IsValid = false;
            Address = newServerSource.Address;
            AuthenticationType = newServerSource.AuthenticationType;
            UserName = newServerSource.UserName;
            Password = newServerSource.Password;
            TestPassed = false;


            TestCommand = new DelegateCommand(Test);
            OkCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(() => CloseAction.Invoke());
        }

        void Save()
        {
            var res = MessageBoxResult.OK;
            if (String.IsNullOrEmpty(ServerSource.Name))
                res = SaveDialog.ShowSaveDialog();
            if (res == MessageBoxResult.OK)
            {
                try
                {
                    var source = new ServerSource
                    {
                        Address = Protocol+Address+":"+SelectedPort,
                        AuthenticationType = AuthenticationType,
                        ID = ServerSource.ID == Guid.Empty ? Guid.NewGuid() : ServerSource.ID,
                        Name = String.IsNullOrEmpty(ServerSource.Name) ? SaveDialog.ResourceName.Name : ServerSource.Name,
                        Password = Password,
                        ResourcePath = "" //todo: needs to come from explorer
                    };

                    ServerSource = source;
                    _updateManager.Save(source);
                    HeaderText = SetToEdit(source);
                    _shellViewModel.ServerSourceAdded(source);
                    if (CloseAction != null)
                        CloseAction.Invoke();
                }
                catch (Exception err)
                {

                    TestMessage = err.Message;
                }
            }

        }

        string SetToEdit(IServerSource source)
        {
            return "Edit " + _connectedServer.Trim() + "/" + source.ResourcePath + source.Name;
        }

        public Action CloseAction { get; set; }
        void Test()
        {
            TestFailed = false;
            TestPassed = false;
            Testing = true;
            TestMessage = _updateManager.TestConnection(new ServerSource
            {
                Address = Protocol +"://" + Address + ":" + SelectedPort,
                AuthenticationType = AuthenticationType,
                ID = ServerSource.ID == Guid.Empty ? Guid.NewGuid() : ServerSource.ID,
                Name = String.IsNullOrEmpty(ServerSource.Name) ? "" : ServerSource.Name,
                Password = Password,
                ResourcePath = "" //todo: needs to come from explorer
            });
            Testrun = true;
            Testing = false;
            if (TestMessage == "")
            {
                TestPassed = true;
                TestFailed = false;
            }
            else
            {
                TestPassed = false;
                TestFailed = true;
            }

            OnPropertyChanged(() => Validate);
            OnPropertyChanged(() => CanClickOk);

        }

        /// <summary>
        /// called by outer when validating
        /// </summary>
        /// <returns></returns>
        public string Validate
        {

            get
            {
                IsValid = false;



                if (!Testrun)
                {
                    return Resources.Languages.Core.ServerDialogNoTestMessage;
                }

                if (TestFailed)
                {
                    return Resources.Languages.Core.ServerDialogTestConnectionLabel; 
                }

                IsValid = true;
                return String.Empty;
            }



        }
        public bool Testrun { get; set; }

        /// <summary>
        /// called by outer when validating
        /// </summary>
        /// <returns></returns>
        string IInnerDialogueTemplate.Validate()
        {
            return Validate;
        }

        /// <summary>
        /// Is valid 
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// Command for save/ok
        /// </summary>
        public ICommand OkCommand { get; set; }
        /// <summary>
        /// Command for cancel
        /// </summary>
        public ICommand CancelCommand { get; set; }
        public bool CanClickOk
        {
            get
            {
                return Validate == "";
            }
        }

        public string HeaderText
        {
            get
            {
                return _headerText;
            }
            set
            {
                _headerText = value;
                OnPropertyChanged(() => HeaderText);
            }
        }

        #endregion

        #region Implementation of INewServerDialogue

        /// <summary>
        /// The server address that we are trying to connect to
        /// </summary>
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                
                _address = value;

                OnPropertyChanged(() => Address);
                OnPropertyChanged(() => Validate);
                OnPropertyChanged(() => CanClickOk);
            }

        }
        /// <summary>
        ///  Windows or user or publlic
        /// </summary>
        public AuthenticationType AuthenticationType
        {
            get
            {
                return _authenticationType;
            }
            set
            {
                if (value != _authenticationType)
                {
                    _authenticationType = value;
                    OnPropertyChanged(() => AuthenticationType);
                    OnPropertyChanged(() => IsUserNameVisible);
                    Testrun = false;
                    TestPassed = false;
                }
            }
        }
        /// <summary>
        /// User Name
        /// </summary>
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                OnPropertyChanged(() => UserName);
            }
        }
        /// <summary>
        /// Password
        /// </summary>
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(() => Password);
            }
        }

        /// <summary>
        /// The message that will be set if the test is either successful or not
        /// </summary>
        public string TestMessage
        {
            get
            {
                return _testMessage;
            }

            set
            {
                _testMessage = value;
                OnPropertyChanged(() => TestMessage);
            }

        }

        #endregion


        public bool TestPassed
        {
            get
            {
                return _testPassed;
            }
            set
            {
                _testPassed = value;
                OnPropertyChanged(() => TestPassed);
                OnPropertyChanged(() => CanClickOk);
            }
        }
        public bool TestFailed
        {
            get
            {
                return _testPassed;
            }
            set
            {
                _testPassed = value;
                OnPropertyChanged(() => TestFailed);
                OnPropertyChanged(() => CanClickOk);
            }
        }
        public bool Testing
        {
            get
            {
                return _testPassed;
            }
            set
            {
                _testPassed = value;
                OnPropertyChanged(() => Testing);
                OnPropertyChanged(() => CanClickOk);
            }
        }

        public bool IsOkEnabled
        {
            get
            {
                return IsValid;
            }

        }

        public bool IsTestEnabled
        {
            get
            {
                return (Address.Length > 0);
            }

        }

        public bool IsUserNameVisible
        {
            get
            {
                return AuthenticationType == AuthenticationType.User;
            }

        }

        public bool IsPasswordVisible
        {
            get
            {
                return AuthenticationType == AuthenticationType.User;
            }

        }

        public string AddressLabel
        {
            get
            {
                return Resources.Languages.Core.ServerDialogAddressLabel;
            }
        }

        public string UserNameLabel
        {
            get
            {
                return Resources.Languages.Core.ServerDialogUserNameLabel;
            }
        }

        public string AuthenticationLabel
        {
            get
            {
                return Resources.Languages.Core.ServerDialogAuthenticationTypeLabel;
            }
        }

        public string PasswordLabel
        {
            get
            {
                return Resources.Languages.Core.ServerDialogPasswordLabel;

            }
        }

        public string TestLabel
        {
            get
            {
                return Resources.Languages.Core.ServerDialogTestConnectionLabel;
            }
        }


        /// <summary>
        /// Test if connection is successful
        /// </summary>
        public ICommand TestCommand
        { get; set; }
        public IServerSource ServerSource
        {
            get
            {
                return _serverSource;
            }
            set
            {
                _serverSource = value;
                if(!String.IsNullOrEmpty(value.Name))
                HeaderText = SetToEdit(value);
            }
        }
        public ISaveDialog SaveDialog
        {
            get
            {
                return _saveDialog;
            }
        }
        public string Protocol
        {
            get
            {
                return _protocol;
            }
            set
            {
                _protocol = value;
                OnPropertyChanged(Protocol);
                if (Protocol == "https" && SelectedPort == "3142")
                {
                    SelectedPort = "3143";
                }
                else if(Protocol == "http" && SelectedPort == "3143")
                {
                    SelectedPort = "3142";
                }
            }
        }
        public string[] Protocols
        {
            get
            {
                return _protocols;
            }
            set
            {
                _protocols = value;
            }
        }
        public ObservableCollection<string> Ports
        {
            get
            {
                return _ports;
            }
            set
            {
                _ports = value;
            }
        }
        public string SelectedPort
        {
            get
            {
                return _selectedPort;
            }
            set
            {
                if (_selectedPort != value)
                {
                    _selectedPort = value;
                    OnPropertyChanged(() => SelectedPort);
                    TestPassed = false;
                    OnPropertyChanged(()=>TestPassed);
                }
               
            }
        }
    }
}
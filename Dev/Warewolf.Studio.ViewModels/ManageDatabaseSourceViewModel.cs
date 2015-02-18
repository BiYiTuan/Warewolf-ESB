using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Dev2.Common.Interfaces.Core;
using Dev2.Common.Interfaces.Core.DynamicServices;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.Explorer;
using Dev2.Common.Interfaces.Runtime.ServiceModel;
using Dev2.Common.Interfaces.SaveDialog;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.Studio.ViewModels.Dialogues;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace Warewolf.Studio.ViewModels
{
    public class ManageDatabaseSourceViewModel : BindableBase, IManageDatabaseSourceViewModel, IDockViewModel
    {
        private enSourceType _serverType;
        private AuthenticationType _authenticationType;
        private string _serverName;
        private string _databaseName;
        private string _userName;
        private string _password;
        private string _testMessage;
        private IList<string> _databaseNames;
        private string _header;
        readonly IStudioUpdateManager _updateManager ;
         IDbSource _dbSource;
        bool _testPassed;
        bool _testFailed;
        bool _testing;
        string _resourceName;

        public ManageDatabaseSourceViewModel(IStudioUpdateManager updateManager)
        {
            _updateManager = updateManager;
       
            HeaderText = "New Database Connector Source Server";
            TestCommand = new DelegateCommand(TestConnection);
            OkCommand = new DelegateCommand(SaveConnection,CanSave);
            Testing = false;
            Types = new List<enSourceType> { enSourceType.SqlDatabase };
            ServerType = enSourceType.SqlDatabase;
            _testPassed = false;
            _testFailed = false;
            
        }

        public ManageDatabaseSourceViewModel(IStudioUpdateManager updateManager, IRequestServiceNameViewModel requestServiceNameViewModel)
            : this(updateManager)
        {

            RequestServiceNameViewModel = requestServiceNameViewModel;

        }
        public ManageDatabaseSourceViewModel(IStudioUpdateManager updateManager, IDbSource dbSource)
            : this(updateManager)
        {
            _dbSource = dbSource;
            FromDbSource(dbSource);
        }

        public bool CanSave()
        {
            return TestPassed && !String.IsNullOrEmpty(DatabaseName);
        }

        void FromDbSource(IDbSource dbSource)
        {
            ResourceName = dbSource.Name;
            AuthenticationType = dbSource.AuthenticationType;
            UserName = dbSource.UserName;
            DatabaseName = dbSource.DbName;
            ServerName = dbSource.ServerName;
            Password = dbSource.Password;
            
        }

        public string ResourceName
        {
            get
            {
                return _resourceName;
            }
            set
            {
                _resourceName = value;
                if(!String.IsNullOrEmpty(value))
                {
                    HeaderText = "Edit Database Service-" + _resourceName;
                }
                OnPropertyChanged(_resourceName);
            }
        }

        public bool UserAuthenticationSelected
        {
            get { return AuthenticationType==AuthenticationType.User; }            
        }

        void SaveConnection()
        {
            if(_dbSource == null)
            {
                var res = RequestServiceNameViewModel.ShowSaveDialog();
               
                if(res==MessageBoxResult.OK)
                {
                    ResourceName = RequestServiceNameViewModel.ResourceName.Name;
                    var src = ToDbSource();
                    src.Path = RequestServiceNameViewModel.ResourceName.Path;
                    Save(src);
                    _dbSource = src;
                    HeaderText = "Edit Database Service-" + _resourceName;
                }
            }
            else
            {
                Save(ToDbSource());
            }
        }

        void Save(IDbSource toDbSource)
        {
            _updateManager.Save(toDbSource);
        }

 

        void TestConnection()
        {
            Testing = true;
            try
            {
                TestFailed = false;
                TestPassed = false;
             
                
                DatabaseNames = _updateManager.TestDbConnection(ToDbSource());
                TestMessage = "Passed";
                TestFailed = false;
                TestPassed = true;

            }
            catch(Exception err)
            {
                TestFailed = true;
                TestPassed = false;
                TestMessage = err.Message;
                DatabaseNames.Clear();
            }
            finally
            {
                Testing = false;
            }
            OnPropertyChanged(() => DatabaseNames);
        }

        IDbSource ToDbSource()
        {
            if(_dbSource == null)
            return new DbSourceDefinition
            {
                AuthenticationType = AuthenticationType,
                ServerName = ServerName ,
                Password = Password,
                UserName =  UserName ,
                Type = ServerType,
                Name = ResourceName,
                DbName = DatabaseName,
                Id =  _dbSource==null?Guid.NewGuid():_dbSource.Id
            };
                // ReSharper disable once RedundantIfElseBlock
            else
            {
                _dbSource.AuthenticationType = AuthenticationType;
                _dbSource.DbName = DatabaseName;
                _dbSource.Password = Password;
                _dbSource.ServerName = ServerName;
                return _dbSource;

            }
        }
        IRequestServiceNameViewModel RequestServiceNameViewModel { get; set; }
        public bool Haschanged
        {
            get { return !ToDbSource().Equals(_dbSource); }
        }
        private void RaiseCanExecuteChanged()
        {
            var command = OkCommand as DelegateCommand;
            if (command != null)
            {
                command.RaiseCanExecuteChanged();
            }
        }
        public IList<enSourceType> Types { get; set; }

        public enSourceType ServerType
        {
            get { return _serverType; }
            set
            {
                _serverType = value;
                OnPropertyChanged(() => ServerType);
                OnPropertyChanged(()=>Haschanged);
            }
        }

        public AuthenticationType AuthenticationType
        {
            get { return _authenticationType; }
            set
            {
                _authenticationType = value;
                OnPropertyChanged(() => AuthenticationType);
                OnPropertyChanged(() => Haschanged);
                OnPropertyChanged(() => UserAuthenticationSelected);
            }
        }

        public string ServerName
        {
            get { return _serverName; }
            set
            {
                _serverName = value;
                OnPropertyChanged(() => ServerName);
                OnPropertyChanged(() => Haschanged);
            }
        }

        public string DatabaseName
        {
            get { return _databaseName; }
            set
            {
                _databaseName = value;
                OnPropertyChanged(() => DatabaseName);
                OnPropertyChanged(() => Haschanged);
                RaiseCanExecuteChanged();
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(() => UserName);
                OnPropertyChanged(() => Haschanged);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(() => Password);
                OnPropertyChanged(() => Haschanged);
            }
        }

        public ICommand TestCommand { get; set; }

        public ICommand CancelTestCommand { get; set; }

        public string TestMessage
        {
            get { return _testMessage; }
            // ReSharper disable UnusedMember.Local
            private set
                // ReSharper restore UnusedMember.Local
            {
                _testMessage = value;
                OnPropertyChanged(() => TestMessage);
                OnPropertyChanged(()=>TestPassed);
            }
        }


        public bool TestPassed
        {
            get { return _testPassed; }
            set
            {
                _testPassed = value;
                OnPropertyChanged(()=>TestPassed);
                RaiseCanExecuteChanged();

            }
        
 
        }
        public string ServerTypeLabel
        {
            get
            {
                return Resources.Languages.Core.DatabaseSourceTypeLabel;
            }
        }
       

        public string UserNameLabel
        {
            get
            {
                return Resources.Languages.Core.UserNameLabel;
            }
        }

        public string AuthenticationLabel
        {
            get
            {
                return Resources.Languages.Core.AuthenticationTypeLabel;
            }
        }

        public string PasswordLabel
        {
            get
            {
                return Resources.Languages.Core.PasswordLabel;

            }
        }

        public string TestLabel
        {
            get
            {
                return Resources.Languages.Core.TestConnectionLabel;
            }
        }
        
        public string CancelTestLabel
        {
            get
            {
                return Resources.Languages.Core.CancelTest;
            }
        }

        public bool TestFailed
        {
            get
            {
                return _testFailed;
            }
            set
            {
                _testFailed = value;
                OnPropertyChanged(()=>TestFailed);
            }
        }

        public bool Testing
        {
            get
            {
                return _testing;
            }
            private set
            {
                _testing = value;
                OnPropertyChanged(()=>Testing);
            }
        }

        public string ServerLabel
        {
            get
            {
                return Resources.Languages.Core.DatabaseSourceServerLabel;
            }
        }

        public string DatabaseLabel
        {
            get
            {
                return Resources.Languages.Core.DatabaseSourceDatabaseLabel;
            }
        }

        public ICommand OkCommand { get; set; }

        public string HeaderText
        {
            get { return _header; }
            set
            {
                _header = value;
                OnPropertyChanged(() => HeaderText);
                OnPropertyChanged(() => Header);
            }
        }

        public string WindowsAuthenticationToolTip
        {
            get
            {
                return Resources.Languages.Core.WindowsAuthenticationToolTip;
            }
        }
        public string UserAuthenticationToolTip
        {
            get
            {
                return Resources.Languages.Core.UserAuthenticationToolTip;
            }
        }

        public string ServerTypeTool
        {
            get
            {
                return Resources.Languages.Core.DatabaseSourceTypeToolTip;
            }
        }

        public IList<string> DatabaseNames
        {
            get { return _databaseNames; }
            set
            {
                _databaseNames = value;
                OnPropertyChanged(() => DatabaseNames);
            }
        }

        public bool IsActive { get; set; }

        public event EventHandler IsActiveChanged;

        public string Header
        {
            get { return _header; }
            set
            {
                _header = value;
                OnPropertyChanged(() => Header);
            }
        }

        public ResourceType? Image
        {
            get { return ResourceType.DbSource; }
        }
    }
}
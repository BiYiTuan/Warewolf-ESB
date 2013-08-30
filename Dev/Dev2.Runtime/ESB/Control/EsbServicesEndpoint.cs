﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;
using Dev2.Common;
using Dev2.Data.Binary_Objects;
using Dev2.Data.ServiceModel.Helper;
using Dev2.DataList.Contract;
using Dev2.Runtime.ESB;
using Dev2.Runtime.ESB.Execution;
using Dev2.Runtime.Helpers;
using Dev2.Runtime.Hosting;
using Dev2.Workspaces;
using Newtonsoft.Json;

namespace Dev2.DynamicServices
{

    /// <summary>
    /// Amended as per PBI 7913
    /// </summary>
    /// IEsbActivityChannel
    public class EsbServicesEndpoint : IFrameworkDuplexDataChannel, IEsbWorkspaceChannel
    {

        #region IFrameworkDuplexDataChannel Members
        Dictionary<string, IFrameworkDuplexCallbackChannel> _users = new Dictionary<string, IFrameworkDuplexCallbackChannel>();
        private RuntimeHelpers _runtimeHelpers = new RuntimeHelpers();
        public void Register(string userName)
        {
            if(_users.ContainsKey(userName))
            {
                _users.Remove(userName);
            }

            _users.Add(userName, OperationContext.Current.GetCallbackChannel<IFrameworkDuplexCallbackChannel>());
            NotifyAllClients(string.Format("User '{0}' logged in", userName));

        }

        public void Unregister(string userName)
        {
            if(UserExists(userName))
            {
                _users.Remove(userName);
                NotifyAllClients(string.Format("User '{0}' logged out", userName));
            }
        }

        public void ShowUsers(string userName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("=========Current Users==========");
            sb.Append("\r\n");
            _users.ToList().ForEach(c => sb.Append(c.Key + "\r\n"));
            SendPrivateMessage("System", userName, sb.ToString());

        }

        public void SendMessage(string userName, string message)
        {
            string suffix = " Said:";
            if(userName == "System")
            {
                suffix = string.Empty;
            }
            NotifyAllClients(string.Format("{0} {1} {2}", userName, suffix, message));
        }

        public void SendPrivateMessage(string userName, string targetUserName, string message)
        {
            string suffix = " Said:";
            if(userName == "System")
            {
                suffix = string.Empty;
            }
            if(UserExists(userName))
            {
                if(!UserExists(targetUserName))
                {
                    NotifyClient(userName, string.Format("System: Message failed - User '{0}' has logged out ", targetUserName));
                }
                else
                {
                    NotifyClient(targetUserName, string.Format("{0} {1} {2}", userName, suffix, message));
                }
            }
        }

        public void SetDebug(string userName, string serviceName, bool debugOn)
        {
            
        }

        public void Rollback(string userName, string serviceName, int versionNo)
        {

        }

        public void Rename(string userName, string resourceType, string resourceName, string newResourceName)
        {


        }

        public void ReloadSpecific(string userName, string serviceName)
        {


        }

        public void Reload()
        {
           
        }

        private bool UserExists(string userName)
        {
            return _users.ContainsKey(userName) || userName.Equals("System", StringComparison.InvariantCultureIgnoreCase);
        }

        private void NotifyAllClients(string message)
        {
            _users.ToList().ForEach(c => NotifyClient(c.Key, message));
        }

        private void NotifyClient(string userName, string message)
        {

            try
            {
                if(UserExists(userName))
                {
                    _users[userName].CallbackNotification(message);
                }
            }
            catch(Exception ex)
            {
                ServerLogger.LogError(ex);
                _users.Remove(userName);
            }
        }

        #endregion


        /// <summary>
        ///Loads service definitions.
        ///This is a singleton service so this object
        ///will be visible in every call 
        /// </summary>
        public EsbServicesEndpoint()
        {
            try
            {
                
            }
            catch(Exception ex)
            {
                ServerLogger.LogError(ex);
                throw ex;
            }
        }

        public bool LoggingEnabled
        {
            get
            {
                return true;
            }
        }

        #region Travis' New Entry Point 
        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="workspaceID">The workspace ID.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public Guid ExecuteRequest(IDSFDataObject dataObject, Guid workspaceID, out ErrorResultTO errors)
        {
            Guid resultID = GlobalConstants.NullDataListID;
            errors = new ErrorResultTO();
            IWorkspace theWorkspace = WorkspaceRepository.Instance.Get(workspaceID);
            IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();

            // If no DLID, we need to make it based upon the request ;)
            if(dataObject.DataListID == GlobalConstants.NullDataListID)
            {
                string theShape = null;
                try
                {
                    theShape = _runtimeHelpers.FindServiceShape(workspaceID, dataObject.ServiceName, true);

                }
                catch(Exception ex)
                {
                    ServerLogger.LogError(ex);
                    errors.AddError(string.Format("Service [ {0} ] not found.", dataObject.ServiceName));
                   return resultID;
                }

                ErrorResultTO invokeErrors;
                dataObject.DataListID = compiler.ConvertTo(DataListFormat.CreateFormat(GlobalConstants._XML), dataObject.RawPayload, theShape, out invokeErrors);
                errors.MergeErrors(invokeErrors);
                dataObject.RawPayload = string.Empty;
            }

            try
            {
                ErrorResultTO invokeErrors;
                // Setup the invoker endpoint ;)
                var invoker = new DynamicServicesInvoker(this, this, theWorkspace);

                // Should return the top level DLID
                resultID = invoker.Invoke(dataObject, out invokeErrors);
                errors.MergeErrors(invokeErrors);

            }
            catch(Exception ex)
            {
                errors.AddError(ex.Message);
            }

            return resultID;
        }

        public T FetchServerModel<T>(IDSFDataObject dataObject, Guid workspaceID, out ErrorResultTO errors)
        {
            var serviceName = dataObject.ServiceName;
            IWorkspace theWorkspace = WorkspaceRepository.Instance.Get(workspaceID);
            var invoker = new DynamicServicesInvoker(this, this, theWorkspace);
            var generateInvokeContainer = invoker.GenerateInvokeContainer(dataObject, serviceName, true);
            var curDlid = generateInvokeContainer.Execute(out errors);
            IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();
            var convertFrom = compiler.ConvertFrom(curDlid, DataListFormat.CreateFormat(GlobalConstants._XML), enTranslationDepth.Data, out errors);
            var jsonSerializerSettings = new JsonSerializerSettings();
            var deserializeObject = JsonConvert.DeserializeObject<T>(convertFrom, jsonSerializerSettings);
            return deserializeObject;
        }


        /// <summary>
        /// Finds the service shape.
        /// </summary>
        /// <param name="workspaceID">The workspace ID.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns></returns>
        public string FindServiceShape(Guid workspaceID, string serviceName, bool serviceInputs)
        {
            var services = ResourceCatalog.Instance.GetDynamicObjects<DynamicService>(workspaceID, serviceName);

            var tmp = services.FirstOrDefault();
            const string baseResult = "<ADL></ADL>";
            var result = "<DataList></DataList>";

            if (tmp != null)
            {
                result = tmp.DataListSpecification;

                // Handle services ;)
                if (result == baseResult && tmp.OutputSpecification == null)
                {
                    var serviceDef = tmp.ResourceDefinition;

                    ErrorResultTO errors;
                    if (!serviceInputs)
                    {
                        var outputMappings = ServiceUtils.ExtractOutputMapping(serviceDef);
                        result = DataListUtil.ShapeDefinitionsToDataList(outputMappings, enDev2ArgumentType.Output,
                                                                         out errors);
                    }
                    else
                    {
                        var inputMappings = ServiceUtils.ExtractInputMapping(serviceDef);
                        result = DataListUtil.ShapeDefinitionsToDataList(inputMappings, enDev2ArgumentType.Input,
                                                                         out errors);
                    }
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                result = "<DataList></DataList>";
            }

            return result;
        }


        /// <summary>
        /// Executes the transactionally scoped request, caller must delete datalist
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="workspaceID">The workspace ID.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public Guid ExecuteTransactionallyScopedRequest(IDSFDataObject dataObject, Guid workspaceID, out ErrorResultTO errors)
        {

            // ----------------- OLD FOREACH WORKING ;)
            IWorkspace theWorkspace = WorkspaceRepository.Instance.Get(workspaceID);
            var invoker = new DynamicServicesInvoker(this, this, theWorkspace);
            errors = new ErrorResultTO();
            string theShape;
            Guid oldID = new Guid();
            Guid innerDatalistID = new Guid();
            ErrorResultTO invokeErrors;

            // Account for silly webpages...
            IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();

            // If no DLID, we need to make it based upon the request ;)
            if (dataObject.DataListID == GlobalConstants.NullDataListID)
            {
                theShape = FindServiceShape(workspaceID, dataObject.ServiceName, true);
                dataObject.DataListID = compiler.ConvertTo(DataListFormat.CreateFormat(GlobalConstants._XML),
                    dataObject.RawPayload, theShape, out invokeErrors);
                errors.MergeErrors(invokeErrors);
                dataObject.RawPayload = string.Empty;
            }

            // local non-scoped execution ;)
            bool isLocal = string.IsNullOrEmpty(dataObject.RemoteInvokerID);

            theShape = FindServiceShape(workspaceID, dataObject.ServiceName, true);
            innerDatalistID = compiler.ConvertTo(DataListFormat.CreateFormat(GlobalConstants._XML),
                                                 string.Empty, theShape, out invokeErrors);
            errors.MergeErrors(invokeErrors);



            // Add left to right
            var left = compiler.FetchBinaryDataList(dataObject.DataListID, out invokeErrors);
            errors.MergeErrors(invokeErrors);
            var right = compiler.FetchBinaryDataList(innerDatalistID, out invokeErrors);
            errors.MergeErrors(invokeErrors);

            DataListUtil.AddMissingFromRight(left, right, out invokeErrors);
            errors.MergeErrors(invokeErrors);
            compiler.PushBinaryDataList(left.UID, left, out invokeErrors);
            errors.MergeErrors(invokeErrors);

            EsbExecutionContainer executionContainer = invoker.GenerateInvokeContainer(dataObject, dataObject.ServiceName, isLocal);
            Guid result = dataObject.DataListID;

            if (executionContainer != null)
            {
                result = executionContainer.Execute(out errors);
            }
            else
            {
                errors.AddError("Null container returned");
            }

            if (!dataObject.IsDataListScoped)
            {
                compiler.ForceDeleteDataListByID(oldID);
                compiler.ForceDeleteDataListByID(innerDatalistID);
            }

            return result;

        }

        /// <summary>
        /// Fetches the execution payload.
        /// </summary>
        /// <param name="dataObj">The data obj.</param>
        /// <param name="targetFormat">The target format.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public string FetchExecutionPayload(IDSFDataObject dataObj, DataListFormat targetFormat, out ErrorResultTO errors)
        {
            errors = new ErrorResultTO();
            string targetShape = _runtimeHelpers.FindServiceShape(dataObj.WorkspaceID, dataObj.ServiceName, false);
            string result = string.Empty;

            if(!string.IsNullOrEmpty(targetShape))
            {
                string translatorShape = ManipulateDataListShapeForOutput(targetShape);
                IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();
                ErrorResultTO invokeErrors;
                result = compiler.ConvertAndFilter(dataObj.DataListID, targetFormat, translatorShape, out invokeErrors);
                errors.MergeErrors(invokeErrors);
            }
            else
            {
                errors.AddError("Could not locate service shape for " + dataObj.ServiceName);
            }

            return result;
        }

        #endregion



        /// <summary>
        /// Manipulates the data list shape for output.
        /// </summary>
        /// <param name="preShape">The pre shape.</param>
        /// <returns></returns>
        public string ManipulateDataListShapeForOutput(string preShape)
        {
            XDocument xDoc = XDocument.Load(new StringReader(preShape));

            XElement rootEl = xDoc.Element("DataList");
            if(rootEl == null) return xDoc.ToString();

            rootEl.Elements().Where(el =>
            {
                var firstOrDefault = el.Attributes("ColumnIODirection").FirstOrDefault();
                    var removeCondition = firstOrDefault != null &&
                                          (firstOrDefault.Value == enDev2ColumnArgumentDirection.Input.ToString() ||
                                           firstOrDefault.Value == enDev2ColumnArgumentDirection.None.ToString());
                return (removeCondition && !el.HasElements);
            }).Remove();

            var xElements = rootEl.Elements().Where(el => el.HasElements);
            var enumerable = xElements as IList<XElement> ?? xElements.ToList();
            enumerable.Elements().Where(element =>
            {
                var xAttribute = element.Attributes("ColumnIODirection").FirstOrDefault();
                    var removeCondition = xAttribute != null &&
                                          (xAttribute.Value == enDev2ColumnArgumentDirection.Input.ToString() ||
                                           xAttribute.Value == enDev2ColumnArgumentDirection.None.ToString());
                return removeCondition;
            }).Remove();
            enumerable.Where(element => !element.HasElements).Remove();
            return xDoc.ToString();
        }


    }
}

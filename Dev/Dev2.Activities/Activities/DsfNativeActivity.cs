
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2014 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using Dev2;
using Dev2.Activities;
using Dev2.Activities.Debug;
using Dev2.Common;
using Dev2.Common.Interfaces.Diagnostics.Debug;
using Dev2.Common.Interfaces.Infrastructure.Providers.Errors;
using Dev2.Data.Binary_Objects;
using Dev2.Data.Enums;
using Dev2.DataList.Contract;
using Dev2.DataList.Contract.Binary_Objects;
using Dev2.Diagnostics;
using Dev2.Diagnostics.Debug;
using Dev2.Instrumentation;
using Dev2.Runtime.Execution;
using Dev2.Simulation;
using Dev2.Util;
using Unlimited.Applications.BusinessDesignStudio.Activities.Hosting;
using Unlimited.Applications.BusinessDesignStudio.Activities.Utilities;
using Warewolf.Storage;

// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Unlimited.Applications.BusinessDesignStudio.Activities
{
    public abstract class DsfNativeActivity<T> : NativeActivity<T>, IDev2ActivityIOMapping, IDev2Activity
    {
        protected ErrorResultTO errorsTo;
        // TODO: Remove legacy properties - when we've figured out how to load files when these are not present
        [GeneralSettings("IsSimulationEnabled")]
        public bool IsSimulationEnabled { get; set; }
        // ReSharper disable RedundantAssignment
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] 
        public IDSFDataObject DataObject { get { return null; } set { value = null; } }
        // ReSharper restore RedundantAssignment
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] 
        public IDataListCompiler Compiler { get; set; }
        // END TODO: Remove legacy properties 

        public InOutArgument<List<string>> AmbientDataList { get; set; }

        // Moved into interface ;)
        public string InputMapping { get; set; }

        public string OutputMapping { get; set; }

        public bool IsWorkflow { get; set; }
        public string ParentServiceName { get; set; }
        public string ParentServiceID { get; set; }
        public string ParentWorkflowInstanceId { get; set; }
        public SimulationMode SimulationMode { get; set; }
        public string ScenarioID { get; set; }
        protected Guid WorkSurfaceMappingId { get; set; }
        /// <summary>
        /// UniqueID is the InstanceID and MUST be a guid.
        /// </summary>
        public string UniqueID { get; set; }

        // PBI 6602 - On Error properties
        [FindMissing]
        public string OnErrorVariable { get; set; }
        [FindMissing]
        public string OnErrorWorkflow { get; set; }
        public bool IsEndedOnError { get; set; }

        protected Variable<Guid> DataListExecutionID = new Variable<Guid>();
        protected List<DebugItem> _debugInputs = new List<DebugItem>();
        protected List<DebugItem> _debugOutputs = new List<DebugItem>();


        internal readonly IDebugDispatcher _debugDispatcher;
        readonly bool _isExecuteAsync;
        string _previousParentInstanceID;
        IDebugState _debugState;
        bool _isOnDemandSimulation;

        //Added for decisions checking errors bug 9704
        ErrorResultTO _tmpErrors = new ErrorResultTO();

        protected IDebugState DebugState { get { return _debugState; } } // protected for testing!

        #region ShouldExecuteSimulation

        bool ShouldExecuteSimulation
        {
            get
            {
                return _isOnDemandSimulation
                    ? SimulationMode != SimulationMode.Never
                    : SimulationMode == SimulationMode.Always;
            }
        }

        #endregion

        #region Ctor

        protected DsfNativeActivity(bool isExecuteAsync, string displayName)
            : this(isExecuteAsync, displayName, DebugDispatcher.Instance)
        {
        }

        protected DsfNativeActivity(bool isExecuteAsync, string displayName, IDebugDispatcher debugDispatcher)
        {
            if(debugDispatcher == null)
            {
                throw new ArgumentNullException("debugDispatcher");
            }

            if(!string.IsNullOrEmpty(displayName))
            {
                DisplayName = displayName;
            }

            _debugDispatcher = debugDispatcher;
            _isExecuteAsync = isExecuteAsync;

            // This will get overwritten when rehydrating
            UniqueID = Guid.NewGuid().ToString();
        }

        #endregion

        #region CacheMetadata

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            metadata.AddImplementationVariable(DataListExecutionID);
            metadata.AddDefaultExtensionProvider(() => new WorkflowInstanceInfo());
        }

        #endregion

        #region Execute

        // 4423 : TWR - sealed so that this cannot be overridden
        protected override sealed void Execute(NativeActivityContext context)
        {
            Dev2Logger.Log.Debug(String.Format("Start {0}", GetType().Name));
            _tmpErrors = new ErrorResultTO();
            _isOnDemandSimulation = false;
            var dataObject = context.GetExtension<IDSFDataObject>();

            IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();


            // we need to register this child thread with the DataListRegistar so we can scope correctly ;)
            DataListRegistar.RegisterActivityThreadToParentId(dataObject.ParentThreadID, Thread.CurrentThread.ManagedThreadId);

            if(compiler != null)
            {
                string errorString = dataObject.Environment.FetchErrors();
                _tmpErrors = ErrorResultTO.MakeErrorResultFromDataListString(errorString);
                if(!(this is DsfFlowDecisionActivity))
                {
                }

                DataListExecutionID.Set(context, dataObject.DataListID);
            }

            _previousParentInstanceID = dataObject.ParentInstanceID;
            _isOnDemandSimulation = dataObject.IsOnDemandSimulation;

            OnBeforeExecute(context);

            try
            {
                var className = GetType().Name;
                Tracker.TrackEvent(TrackerEventGroup.ActivityExecution, className);

                if(ShouldExecuteSimulation)
                {
                    OnExecuteSimulation(context);
                }
                else
                {
                    OnExecute(context);
                }
            }
            catch(Exception ex)
            {

                Dev2Logger.Log.Error("OnExecute", ex);
                var errorString = ex.Message;
                var errorResultTO = new ErrorResultTO();
                errorResultTO.AddError(errorString);
                if(compiler != null)
                {
                    dataObject.Environment.AddError(errorResultTO.MakeDataListReady());
                }
            }
            finally
            {
                if(!_isExecuteAsync || _isOnDemandSimulation)
                {
                    var resumable = dataObject.WorkflowResumeable;
                    OnExecutedCompleted(context, false, resumable);
                    if(compiler != null)
                    {
                        DoErrorHandling(dataObject);
                    }
                }

            }
        }

        protected void DoErrorHandling(IDSFDataObject dataObject)
        {
            string errorString = dataObject.Environment.FetchErrors();
            _tmpErrors.AddError(errorString);
            if(_tmpErrors.HasErrors())
            {
                if(!(this is DsfFlowDecisionActivity))
                {
                    if (!String.IsNullOrEmpty(errorString))
                    {
                        PerformCustomErrorHandling(dataObject, errorString);
                    }
                }
            }
        }

        void PerformCustomErrorHandling(IDSFDataObject dataObject, string currentError)
        {
            try
            {
                if(!String.IsNullOrEmpty(OnErrorVariable))
                {
                    dataObject.Environment.Assign(OnErrorVariable,currentError);                    
                }
                if(!String.IsNullOrEmpty(OnErrorWorkflow))
                {
                    var esbChannel = dataObject.EsbChannel;
                    ErrorResultTO tmpErrors;
                    esbChannel.ExecuteLogErrorRequest(dataObject, dataObject.WorkspaceID, OnErrorWorkflow, out tmpErrors);
                    dataObject.Environment.AddError(tmpErrors.MakeDisplayReady());
                }
            }
            catch(Exception e)
            {
                dataObject.Environment.AddError(e.Message);
            }
            finally
            {
                
                if(IsEndedOnError)
                {
                    PerformStopWorkflow(dataObject);
                }
            }
        }

        void PerformStopWorkflow(IDSFDataObject dataObject)
        {
            var service = ExecutableServiceRepository.Instance.Get(dataObject.WorkspaceID, dataObject.ResourceID);
            if(service != null)
            {
                Guid parentInstanceID;
                Guid.TryParse(dataObject.ParentInstanceID, out parentInstanceID);
                var debugState = new DebugState
                {
                    ID = dataObject.DataListID,
                    ParentID = parentInstanceID,
                    WorkspaceID = dataObject.WorkspaceID,
                    StateType = StateType.End,
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    ActivityType = ActivityType.Workflow,
                    DisplayName = dataObject.ServiceName,
                    IsSimulation = dataObject.IsOnDemandSimulation,
                    ServerID = dataObject.ServerID,
                    ClientID = dataObject.ClientID,
                    OriginatingResourceID = dataObject.ResourceID,
                    OriginalInstanceID = dataObject.OriginalInstanceID,
                    Server = string.Empty,
                    Version = string.Empty,
                    SessionID = dataObject.DebugSessionID,
                    EnvironmentID = dataObject.EnvironmentID,
                    Name = GetType().Name,
                    ErrorMessage = "Termination due to error in activity",
                    HasError = true
                };
                DebugDispatcher.Instance.Write(debugState);
            }
        }

        #endregion

        #region OnBeforeExecute

        protected virtual void OnBeforeExecute(NativeActivityContext context)
        {
        }

        #endregion

        #region OnExecute

        /// <summary>
        /// When overridden runs the activity's execution logic 
        /// </summary>
        /// <param name="context">The context to be used.</param>
        protected abstract void OnExecute(NativeActivityContext context);

        #endregion

        #region OnExecuteSimulation

        /// <summary>
        /// When overridden runs the activity's simulation logic
        /// </summary>
        /// <param name="context">The context to be used.</param>
        protected virtual void OnExecuteSimulation(NativeActivityContext context)
        {
            var rootInfo = context.GetExtension<WorkflowInstanceInfo>();

            var key = new SimulationKey
            {
                WorkflowID = rootInfo.ProxyName,
                ActivityID = UniqueID,
                ScenarioID = ScenarioID
            };
            var result = SimulationRepository.Instance.Get(key);
            if(result != null && result.Value != null)
            {
                var dataListExecutionID = context.GetValue(DataListExecutionID);
                IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();

                var dataObject = context.GetExtension<IDSFDataObject>();

                if(compiler != null && dataObject != null)
                {
                    var allErrors = new ErrorResultTO();
                    var dataList = compiler.FetchBinaryDataList(dataObject.DataListID, out errorsTo);
                    allErrors.MergeErrors(errorsTo);

                    compiler.Merge(dataList, result.Value, enDataListMergeTypes.Union, enTranslationDepth.Data, false, out errorsTo);
                    allErrors.MergeErrors(errorsTo);

                    compiler.Shape(dataListExecutionID, enDev2ArgumentType.Output, OutputMapping, out errorsTo);
                    allErrors.MergeErrors(errorsTo);

                    if(allErrors.HasErrors())
                    {
                        DisplayAndWriteError(rootInfo.ProxyName, allErrors);
                        dataObject.Environment.AddError(allErrors.MakeDataListReady());
                    }
                }
            }
        }

        #endregion

        #region OnExecutedCompleted

        protected virtual void OnExecutedCompleted(NativeActivityContext context, bool hasError, bool isResumable)
        {
            var dataListExecutionID = DataListExecutionID.Get(context);
            IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();
            var dataObject = context.GetExtension<IDSFDataObject>();

            if(dataObject.ForceDeleteAtNextNativeActivityCleanup)
            {
                // Used for web-pages to signal a force delete after checks of what would become a zombie datalist ;)
                dataObject.ForceDeleteAtNextNativeActivityCleanup = false; // set back
                compiler.ForceDeleteDataListByID(dataListExecutionID);
            }

            if(!dataObject.IsDebugNested)
            {
                dataObject.ParentInstanceID = _previousParentInstanceID;
            }

            dataObject.NumberOfSteps = dataObject.NumberOfSteps + 1;
            //Disposes of all used data lists 

            int threadID = Thread.CurrentThread.ManagedThreadId;

            if(dataObject.IsDebugMode())
            {
                List<Guid> datlistIds;
                if(!dataObject.ThreadsToDispose.TryGetValue(threadID, out datlistIds))
                {
                    dataObject.ThreadsToDispose.Add(threadID, new List<Guid> { dataObject.DataListID });
                }
                else
                {
                    if(!datlistIds.Contains(dataObject.DataListID))
                    {
                        datlistIds.Add(dataObject.DataListID);
                        dataObject.ThreadsToDispose[threadID] = datlistIds;
                    }
                }
            }

        }

        #endregion

        #region ForEach Mapping

        public abstract void UpdateForEachInputs(IList<Tuple<string, string>> updates);

        public abstract void UpdateForEachOutputs(IList<Tuple<string, string>> updates);

        #endregion

        #region GetDataList

        protected static IBinaryDataList GetDataList(NativeActivityContext context)
        {
            var dataObject = context.GetExtension<IDSFDataObject>();
            IDataListCompiler compiler = DataListFactory.CreateDataListCompiler();

            ErrorResultTO errors;
            return compiler.FetchBinaryDataList(dataObject.DataListID, out errors);
        }

        #endregion

        #region GetDebugInputs/Outputs

        public virtual List<DebugItem> GetDebugInputs(IExecutionEnvironment env)
        {
            return DebugItem.EmptyList;
        }

        public virtual List<DebugItem> GetDebugOutputs(IExecutionEnvironment env)
        {
            return DebugItem.EmptyList;
        }
        #endregion

        #region DispatchDebugState

        public void DispatchDebugState(IDSFDataObject dataObject, StateType stateType)
        {
            
            
            Guid remoteID;
            Guid.TryParse(dataObject.RemoteInvokerID, out remoteID);

            if(stateType == StateType.Before)
            {
                if (_debugState == null)
                {
                    InitializeDebugState(stateType, dataObject, remoteID, false, "");
                }

                if (_debugState != null)
                {
                    // Bug 8595 - Juries
                    var type = GetType();
                    var instance = Activator.CreateInstance(type);
                    var activity = instance as Activity;
                    if (activity != null)
                    {
                        _debugState.Name = activity.DisplayName;

                    }
                    var act = instance as DsfActivity;
                    //End Bug 8595
                    try
                    {
                        Copy(GetDebugInputs(dataObject.Environment), _debugState.Inputs);
                    }
                    catch (Exception err)
                    {
                        Dev2Logger.Log.Error("DispatchDebugState", err);
                        AddErrorToDataList(err, dataObject);
                        var errorMessage = dataObject.Environment.FetchErrors();
                        _debugState.ErrorMessage = errorMessage;
                        _debugState.HasError = true;
                        var debugError = err as DebugCopyException;
                        if (debugError != null)
                        {
                            _debugState.Inputs.Add(debugError.Item);
                        }
                    }

                    if (dataObject.RemoteServiceType == "Workflow" && act != null && !_debugState.HasError)
                    {
                        var debugItem = new DebugItem();
                        var debugItemResult = new DebugItemResult { Type = DebugItemResultType.Value, Label = "Execute workflow asynchronously: ", Value = dataObject.RunWorkflowAsync ? "True" : "False" };
                        debugItem.Add(debugItemResult);
                        _debugState.Inputs.Add(debugItem);
                    }
                }
            }
            else
            {
                bool hasError = dataObject.Environment.HasErrors();

                var errorMessage = String.Empty;
                if(hasError)
                {
                    errorMessage = string.Join(Environment.NewLine,dataObject.Environment.Errors);
                }

                if(_debugState == null)
                {
                    InitializeDebugState(stateType, dataObject, remoteID, hasError, errorMessage);
                }

                if(_debugState != null)
                {
                    _debugState.NumberOfSteps = IsWorkflow ? dataObject.NumberOfSteps : 0;
                    _debugState.StateType = stateType;
                    _debugState.EndTime = DateTime.Now;
                    _debugState.HasError = hasError;
                    _debugState.ErrorMessage = errorMessage;
                    try
                    {
                        if(dataObject.RunWorkflowAsync && !_debugState.HasError)
                        {
                            var debugItem = new DebugItem();
                            var debugItemResult = new DebugItemResult { Type = DebugItemResultType.Value, Value = "Asynchronous execution started" };
                            debugItem.Add(debugItemResult);
                            _debugState.Outputs.Add(debugItem);
                            _debugState.NumberOfSteps = 0;
                        }
                        else
                        {
                            Copy(GetDebugOutputs(dataObject.Environment), _debugState.Outputs);
                        }
                    }
                    catch(Exception e)
                    {
                        Dev2Logger.Log.Error("Debug Dispatch Error", e);
                        AddErrorToDataList(e,dataObject);
                        errorMessage = dataObject.Environment.FetchErrors();
                        _debugState.ErrorMessage = errorMessage;
                        _debugState.HasError = true;
                    }
                }
            }

            if(_debugState != null && (!(_debugState.ActivityType == ActivityType.Workflow || _debugState.Name == "DsfForEachActivity") && remoteID == Guid.Empty))
            {
                _debugState.StateType = StateType.All;

                // Only dispatch 'before state' if it is a workflow or for each activity or a remote activity ;)
                if(stateType == StateType.Before)
                {
                    return;
                }
            }

            // We know that if a if it is not a workflow it must be a service ;)
            if(dataObject.RemoteServiceType != "Workflow" && !String.IsNullOrWhiteSpace(dataObject.RemoteServiceType))
            {
                if(_debugState != null)
                {
                    _debugState.ActivityType = ActivityType.Service;
                }
            }

            if(_debugState != null)
            {
                if(remoteID == Guid.Empty)
                {
                    switch(_debugState.StateType)
                    {
                        case StateType.Before:
                            _debugState.Outputs.Clear();
                            break;
                        case StateType.After:
                            _debugState.Inputs.Clear();
                            break;
                    }
                }

                // BUG 9706 - 2013.06.22 - TWR : refactored from here to DebugDispatcher
                _debugState.ClientID = dataObject.ClientID;
                _debugState.OriginatingResourceID = dataObject.ResourceID;
                _debugDispatcher.Write(_debugState, dataObject.RemoteInvoke, dataObject.RemoteInvokerID, dataObject.ParentInstanceID, dataObject.RemoteDebugItems);

                if(stateType == StateType.After)
                {
                    // Free up debug state
                    _debugState = null;
                }
            }
        }

        void AddErrorToDataList(Exception err, IDSFDataObject dataObject)
        {
            var errorString = err.Message;
            dataObject.Environment.Errors.Add(errorString);
        }

        protected void InitializeDebug(IDSFDataObject dataObject)
        {
            if(dataObject.IsDebugMode())
            {
                string errorMessage = string.Empty;
                Guid remoteID;
                Guid.TryParse(dataObject.RemoteInvokerID, out remoteID);
                InitializeDebugState(StateType.Before, dataObject, remoteID, false, errorMessage);
            }
        }

        protected void InitializeDebugState(StateType stateType, IDSFDataObject dataObject, Guid remoteID, bool hasError, string errorMessage)
        {
            Guid parentInstanceID;
            Guid.TryParse(dataObject.ParentInstanceID, out parentInstanceID);
            UpdateDebugParentID(dataObject);
            if(remoteID != Guid.Empty)
            {
                UniqueID = Guid.NewGuid().ToString();
            }
            _debugState = new DebugState
            {
                ID = Guid.Parse(UniqueID),
                ParentID = parentInstanceID,
                WorkSurfaceMappingId = WorkSurfaceMappingId,
                WorkspaceID = dataObject.WorkspaceID,
                StateType = stateType,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                ActivityType = IsWorkflow ? ActivityType.Workflow : ActivityType.Step,
                DisplayName = DisplayName,
                IsSimulation = ShouldExecuteSimulation,
                ServerID = dataObject.ServerID,
                OriginatingResourceID = dataObject.ResourceID,
                OriginalInstanceID = dataObject.OriginalInstanceID,
                Server = remoteID.ToString(),
                Version = string.Empty,
                Name = GetType().Name,
                HasError = hasError,
                ErrorMessage = errorMessage,
                EnvironmentID = dataObject.EnvironmentID,
                SessionID = dataObject.DebugSessionID
            };
        }


        public virtual void UpdateDebugParentID(IDSFDataObject dataObject)
        {
            WorkSurfaceMappingId = Guid.Parse(UniqueID);
        }
        static void Copy<TItem>(IEnumerable<TItem> src, List<TItem> dest)
        {
            if(src == null || dest == null)
            {
                return;
            }

            // ReSharper disable ForCanBeConvertedToForeach
            dest.AddRange(src);
        }

        #endregion

        #region DisplayAndWriteError

        protected static void DisplayAndWriteError(string serviceName, ErrorResultTO errors)
        {
            var errorBuilder = new StringBuilder();
            foreach(var e in errors.FetchErrors())
            {
                errorBuilder.AppendLine(string.Format("--[ Execution Exception ]--\r\nService Name = {0}\r\nError Message = {1} \r\n--[ End Execution Exception ]--", serviceName, e));
            }
            Dev2Logger.Log.Error("DsfNativeActivity", new Exception(errorBuilder.ToString()));
        }

        #endregion

        #region GetForEachInputs/Outputs

        public abstract IList<DsfForEachItem> GetForEachInputs();

        public abstract IList<DsfForEachItem> GetForEachOutputs();

        #endregion

        #region GetForEachItems

        protected IList<DsfForEachItem> GetForEachItems(params string[] strings)
        {
            if(strings == null || strings.Length == 0)
            {
                return DsfForEachItem.EmptyList;
            }

            return (from s in strings
                    where !string.IsNullOrEmpty(s)
                    select new DsfForEachItem
                    {
                        Name = s,
                        Value = s
                    }).ToList();
        }

        #endregion

        #region GetDataListItemsForEach

        #endregion

        #region GetFindMissingEnum

        public virtual enFindMissingType GetFindMissingType()
        {
            return enFindMissingType.StaticActivity;
        }

        public virtual IDev2Activity Execute(IDSFDataObject data)
        {
            try
            {
                var className = GetType().Name;
                Tracker.TrackEvent(TrackerEventGroup.ActivityExecution, className);

                ExecuteTool(data);
            }
            catch (Exception ex)
            {

                Dev2Logger.Log.Error("OnExecute", ex);
                var errorString = ex.Message;
                var errorResultTO = new ErrorResultTO();
                errorResultTO.AddError(errorString);                
            }
            finally
            {
                if (!_isExecuteAsync || _isOnDemandSimulation)
                {
                    DoErrorHandling(data);
                }

            }

            
            if(NextNodes != null && NextNodes.Count()>0)
            {
                    NextNodes.First().Execute(data);
                    return NextNodes.First();
             }
            return null;
        }

        #endregion

        protected abstract void ExecuteTool(IDSFDataObject dataObject);

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable<IDev2Activity> NextNodes { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)] 
        public Guid ActivityId { get; set; }



        #region Create Debug Item

        protected void AddDebugInputItem(DebugOutputBase parameters)
        {
            IDebugItem itemToAdd = new DebugItem();
            itemToAdd.AddRange(parameters.GetDebugItemResult());
            _debugInputs.Add((DebugItem)itemToAdd);
        }

        protected void AddDebugOutputItem(DebugOutputBase parameters)
        {
            DebugItem itemToAdd = new DebugItem();
            itemToAdd.AddRange(parameters.GetDebugItemResult());
            _debugOutputs.Add(itemToAdd);
        }

        protected void AddDebugItem(DebugOutputBase parameters, DebugItem debugItem)
        {
            try
            {
                var debugItemResults = parameters.GetDebugItemResult();
                debugItem.AddRange(debugItemResults);
            }
            catch(Exception e)
            {
                Dev2Logger.Log.Error(e);
            }
        }

        #endregion

        #region Get Debug State

        public IDebugState GetDebugState()
        {
            return _debugState;
        }

        #endregion

        #region workSurfaceMappingId
        public Guid GetWorkSurfaceMappingId()
        {
            return WorkSurfaceMappingId;
        }
        #endregion

        public virtual IList<IActionableErrorInfo> PerformValidation()
        {
            return new List<IActionableErrorInfo>();
        }

        #region Overrides of Object

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            var act = obj as IDev2Activity;
            if (act == null)
            {
                return false;
            }
            return act.UniqueID == UniqueID;
        }

        #region Overrides of Object

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #endregion
    }
}

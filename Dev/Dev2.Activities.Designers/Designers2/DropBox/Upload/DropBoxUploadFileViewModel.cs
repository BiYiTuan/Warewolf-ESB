
/*
*  Warewolf - The Easy Service Bus
*  Copyright 2014 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.ComponentModel;
using Dev2.Activities.Designers2.Core;
using Dev2.Common.Interfaces.Core.DynamicServices;
using Dev2.Data.ServiceModel;
using Dev2.Studio.Core;
using Dev2.Studio.Core.Interfaces;

namespace Dev2.Activities.Designers2.DropBox.Upload
{
    public class DropBoxUploadFileViewModel : ActivityDesignerViewModel,INotifyPropertyChanged
    {
        IEnvironmentModel _environmentModel;

        public DropBoxUploadFileViewModel(ModelItem modelItem, IEnvironmentModel environmentModel)
            : base(modelItem)
        {
            _environmentModel = environmentModel;
            AddTitleBarHelpToggle();
        }


        public DropBoxUploadFileViewModel(ModelItem modelItem)
            : this(modelItem, EnvironmentRepository.Instance.ActiveEnvironment)
        {
          
            AddTitleBarHelpToggle();
        }

        public override void Validate()
        {
        }

        public IEnumerable<DropBoxSource> Sources
        {
            get { return _environmentModel.ResourceRepository.FindSourcesByType<DropBoxSource>(_environmentModel, enSourceType.DropboxSource); }
        }


        public DropBoxSource SelectedSource
        {
            get { return GetProperty<DropBoxSource>(); }
            // ReSharper restore ExplicitCallerInfoArgument
            set
            {
                SetProperty(value);
                OnPropertyChanged("SelectedSource");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

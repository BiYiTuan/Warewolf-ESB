﻿using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.TaskScheduler;

namespace Dev2.Common.Interfaces.WindowsTaskScheduler.Wrappers
{
    public interface ITaskCollection : IDisposable, IWrappedObject<TaskCollection>, IEnumerable<IDev2Task>, IEnumerable
    {
        new IEnumerator<IDev2Task> GetEnumerator();
    }
}
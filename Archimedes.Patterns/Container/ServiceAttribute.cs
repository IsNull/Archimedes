﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Container
{
    /// <summary>
    /// Marks the class as a service component
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : ComponentAttribute
    {
        public ServiceAttribute() { }

        public ServiceAttribute(string name) : base(name)
        {
        }

    }
}
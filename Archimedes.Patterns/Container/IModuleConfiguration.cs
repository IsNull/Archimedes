﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Container
{
    public interface IModuleConfiguration
    {
        void Configure();

        Type GetImplementaionTypeFor(Type type);
    }
}
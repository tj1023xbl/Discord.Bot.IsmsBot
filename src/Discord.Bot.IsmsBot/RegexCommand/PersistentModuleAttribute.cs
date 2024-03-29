﻿using System;

namespace IsmsBot.RegexCommand
{
    /// <summary>Tells regex command to not recreate class on every invokation, and use cached instance instead.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PersistentModuleAttribute : Attribute
    {
        public bool SharedInstance { get; set; } = true;
        public bool PreInitialize { get; set; } = false;

        public PersistentModuleAttribute() { }
    }
}
﻿using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IsmsBot.RegexCommand
{
    public class RegexCommandModuleProvider : IRegexCommandModuleProvider
    {
        private readonly IServiceProvider _services;
        private readonly IDictionary<Type, object> _sharedInstances;
        private readonly IDictionary<RegexCommandInstance, object> _persistentInstances;
        private readonly IDictionary<RegexCommandInstance, RegexCommandModuleInfo> _knownModules;

        public RegexCommandModuleProvider(IServiceProvider services)
        {
            this._services = services;
            this._sharedInstances = new Dictionary<Type, object>();
            this._persistentInstances = new Dictionary<RegexCommandInstance, object>();
            this._knownModules = new Dictionary<RegexCommandInstance, RegexCommandModuleInfo>();
        }

        public object GetModuleInstance(RegexCommandInstance commandInstance)
        {
            // init module info
            RegexCommandModuleInfo moduleInfo;
            if (!_knownModules.TryGetValue(commandInstance, out moduleInfo))
            {
                IEnumerable<ConstructorInfo> constructors = commandInstance.ModuleType
                    .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .OrderByDescending(ctor => ctor.GetParameters().Length);
                foreach (ConstructorInfo ctor in constructors)
                {
                    moduleInfo = InitializeModuleInfo(ctor);
                    if (moduleInfo != null)
                    {
                        Log.Verbose("Adding module {0} to known modules for {1} in RegexCommandModuleProvider", moduleInfo, commandInstance.MethodName);
                        _knownModules.Add(commandInstance, moduleInfo);
                        break;
                    }
                }
                if (moduleInfo == null)
                    throw new InvalidOperationException($"Cannot create {commandInstance.ModuleType.FullName} - none of the constructors can have its dependencies resolved");
            }

            // check if there is already a persistent or shared instance
            if (_persistentInstances.TryGetValue(commandInstance, out object instance))
                return instance;
            if (_sharedInstances.TryGetValue(moduleInfo.Type, out instance))
            {
                _persistentInstances.Add(commandInstance, instance);
                return instance;
            }

            // create a new instance, cache it if it's persistent
            instance = moduleInfo.CreateInstance();
            if (moduleInfo.IsPersistent)
            {
                _persistentInstances.Add(commandInstance, instance);
                if (moduleInfo.IsShared)
                    _sharedInstances.Add(moduleInfo.Type, instance);
            }

            return instance;
        }

        private RegexCommandModuleInfo InitializeModuleInfo(ConstructorInfo constructor)
        {
            ParameterInfo[] ctorParams = constructor.GetParameters();
            object[] paramsValues = new object[ctorParams.Length];
            foreach (ParameterInfo param in ctorParams)
            {
                object value = _services.GetService(param.ParameterType);
                if (value == null)
                {
                    if (param.IsOptional)
                        value = param.HasDefaultValue ? param.DefaultValue : null;
                    else
                        return null;
                }
                paramsValues[param.Position] = value;
            }
            return new RegexCommandModuleInfo(constructor, paramsValues);
        }

        private class RegexCommandModuleInfo
        {
            public Type Type { get; }
            public bool IsShared { get; }
            public bool IsPersistent { get; }
            private readonly ConstructorInfo _ctor;
            private readonly object[] _params;

            public RegexCommandModuleInfo(ConstructorInfo ctor, object[] parameters)
            {
                this._ctor = ctor;
                this._params = parameters;

                this.Type = ctor.DeclaringType;
                PersistentModuleAttribute persistent = this.Type.GetCustomAttribute<PersistentModuleAttribute>();
                this.IsPersistent = persistent != null;
                this.IsShared = this.IsPersistent && persistent.SharedInstance;
            }

            public object CreateInstance()
                => _ctor.Invoke(_params);
        }
    }
}

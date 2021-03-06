﻿using BR.AN.PviServices;
using ControlWorks.Logging;
using ControlWorks.Services.Configuration;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ControlWorks.Services.Pvi
{
    public interface IVariableManager
    {
        void ConnectVariables();
        List<Tuple<string, string>> ReadVariables(string cpuName, IEnumerable<string> variables);

    }
    public class VariableManager
    {
        private ILogger _logger;
        private Service _service;
        private IVariableApi _variableApi;
        private bool _triggerValue = false;
        private IEventNotifier _notifier;


        public VariableManager(Service service, IVariableApi variableApi) : this(service, variableApi, null)
        {

        }
        public VariableManager(Service service, IVariableApi variableApi, IEventNotifier notifier)
        {
            _logger = new Log4netAdapter(LogManager.GetLogger("ServiceLogger"));
            _notifier = notifier;
            _service = service;
            _variableApi = variableApi;

            bool t;
            if (Boolean.TryParse(ConfigurationProvider.ShutdownTrigger, out t))
            {
                _triggerValue = t;
            }
        }

        public void ConnectVariables()
        {
            foreach (Cpu cpu in _service.Cpus.Values)
            {
                var cpuVariables = _variableApi.FindByCpuName(cpu.Name);
                CreateVariables(cpu, cpuVariables.VariableNames);
            }
        }

        public List<Tuple<string, string>> ReadVariables(string cpuName)
        {

            var variableInfo = _variableApi.FindByCpuName(cpuName);

            var list = new List<Tuple<string, string>>();

            if (_service.Cpus.ContainsKey(cpuName))
            {
                var cpu = _service.Cpus[cpuName];

                foreach (var variable in variableInfo.VariableNames)
                {
                    if (cpu.Variables.ContainsKey(variable))
                    {
                        list.Add(new Tuple<string, string>(variable, cpu.Variables[variable].Value));
                    }
                }
            }

            return list;
        }

        private void CreateVariables(Cpu cpu, string[] variables)
        {
            foreach (var variableName in variables)
            {
                CreateVariable(cpu, variableName);
            }

            var trigger = ConfigurationProvider.ShutdownTriggerVariable;

            CreateEventVariable(cpu, trigger);
        }

        private Variable CreateEventVariable(Cpu cpu, string name)
        {
            if (!cpu.Variables.ContainsKey(name))
            {
                Variable variable = new Variable(cpu, name);
                variable.UserTag = name;
                variable.UserData = cpu.UserData;
                variable.Connected += Variable_Connected;
                variable.Error += Variable_Error;
                variable.ValueChanged += Variable_ValueChanged;
                variable.Active = true;
                variable.Connect();
                return variable;
            }
            _logger.Log(new LogEntry(LoggingEventType.Information, $"A variable with the name {name} already exists for Cpu {cpu.Name}"));

            return null;
        }

        private void Variable_ValueChanged(object sender, VariableEventArgs e)
        {
            var variable = sender as Variable;

            if (variable == null)
            {
                return;
            }

            var cpu = variable.Parent as Cpu;

            if (cpu == null)
            {
                return;
            }

            var value = variable.Value.ToBoolean(CultureInfo.CurrentCulture);

            if (value == _triggerValue  && _notifier != null)
            {
                _notifier.RaiseShutdown(cpu);
            }

            _logger.Log(new LogEntry(LoggingEventType.Information, $"Variable {e.Name} value changed. Value={value}; Cpu={cpu.Name}/{cpu.Connection.TcpIp.DestinationIpAddress}"));
        }

        private Variable CreateVariable(Cpu cpu, string name)
        {
            if (!cpu.Variables.ContainsKey(name))
            {
                Variable variable = new Variable(cpu, name);
                variable.UserTag = name;
                variable.UserData = cpu.UserData;
                variable.Connected += Variable_Connected;
                variable.Error += Variable_Error;
                variable.Active = true;
                variable.Connect();
                return variable;
            }
            _logger.Log(new LogEntry(LoggingEventType.Information, $"A variable with the name {name} already exists for Cpu {cpu.Name}"));

            return null;
        }

        private void Variable_Error(object sender, PviEventArgs e)
        {
            var variable = sender as Variable;
            var cpu = variable.Parent as Cpu;

            if (variable != null && cpu != null)
            {
                _logger.Log(new LogEntry(LoggingEventType.Information, PviMessage.FormatMessage($"Variable Error Cpu={cpu.Name}; Variable={variable.Name}", e)));
            }
        }

        private void Variable_Connected(object sender, PviEventArgs e)
        {
            var variable = sender as Variable;
            var cpu = variable.Parent as Cpu;

            if (variable != null && cpu != null)
            {
                _logger.Log(new LogEntry(LoggingEventType.Information, PviMessage.FormatMessage($"Variable Connected. Cpu={cpu.Name}; Variable={variable.Name}", e)));
            }
        }
    }
}

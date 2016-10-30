using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ControlWorks.Services.Pvi
{
    public class VariableInfo
    {
        public string CpuName { get; set; }
        public string[] Variables { get; set; }
    }
    public class VariableInfoCollection
    {
        public static readonly object _syncLock = new object();

        const string VARIABLE_MASTER = "VARIABLE_MASTER";

        Dictionary<string, VariableInfo> _variableLookup;

        public VariableInfoCollection()
        {
            _variableLookup = new Dictionary<string, VariableInfo>();

        }

        public List<VariableInfo> GetAll()
        {
            VariableInfo master = null;
            if (_variableLookup.ContainsKey(VARIABLE_MASTER))
            {
                var responseList = new List<VariableInfo>();
                master = _variableLookup[VARIABLE_MASTER];
                responseList.Add(master);

                foreach (var vi in _variableLookup.Values)
                {
                    if (vi.CpuName != VARIABLE_MASTER)
                    {
                        var list = new List<string>(vi.Variables);
                        list.AddRange(master.Variables);
                        responseList.Add(new VariableInfo { CpuName = vi.CpuName, Variables = list.ToArray() });
                    }
                }

                return responseList;

            }

            return _variableLookup.Values.ToList();
        }

        public void AddCpuRange(string[] cpuList)
        {
            foreach (var cpu in cpuList)
            {
                if (!_variableLookup.ContainsKey(cpu))
                {
                    _variableLookup.Add(cpu, new VariableInfo { CpuName = cpu, Variables= new List<string>().ToArray()});
                }
            }

        }

        public void RemoveCpuRange(string[] cpuList)
        {
            foreach (var cpu in cpuList)
            {
                if (_variableLookup.ContainsKey(cpu))
                {
                    _variableLookup.Remove(cpu);
                }
            }
        }

        public VariableInfo FindByCpu(string name)
        {
            return GetAll().FirstOrDefault(v => v.CpuName == name);
        }

        public void AddRange(string cpuName, IEnumerable<string> variableNames)
        {
            foreach(var name in variableNames)
            {
                Add(cpuName, name);
            }
        }

        public void RemoveRange(string cpuName, IEnumerable<string> variableNames)
        {
            foreach (var name in variableNames)
            {
                Remove(cpuName, name);
            }
        }

        public void Add(string cpuName, string variableName)
        {
            if (_variableLookup.ContainsKey(VARIABLE_MASTER))
            {
                var master = _variableLookup[VARIABLE_MASTER];
                if (master.Variables.Contains(variableName))
                {
                    return;
                }
            }

            if (!_variableLookup.ContainsKey(cpuName))
            {
                var info = new VariableInfo();
                info.CpuName = cpuName;
                info.Variables = new List<string>().ToArray();
                _variableLookup.Add(cpuName, info);
            }

            var v_info = _variableLookup[cpuName];
            
            if (!v_info.Variables.Contains(variableName))
            {
                var list = new List<string>(v_info.Variables);
                list.Add(variableName);
                v_info.Variables = list.ToArray();
            }
        }

        public void Remove(string cpuName, string variableName)
        {
            if (!_variableLookup.ContainsKey(VARIABLE_MASTER))
            {
                RemoveVariable(cpuName, variableName);
            }
            else
            {
                var master = _variableLookup[VARIABLE_MASTER];
                if (master.Variables.Contains(variableName))
                {
                    RemoveVariable(VARIABLE_MASTER, variableName);
                }
            }
        }

        private void RemoveVariable(string cpuName, string variableName)
        {
            if (_variableLookup.ContainsKey(cpuName))
            {
                var v_info = _variableLookup[cpuName];
                var name = v_info.Variables.FirstOrDefault();
                if (!String.IsNullOrEmpty(name))
                {
                    var list = new List<string>(v_info.Variables);
                    list.Remove(variableName);
                    v_info.Variables = list.ToArray();
                }

                if (v_info.Variables.Count() == 0)
                {
                    _variableLookup.Remove(cpuName);
                }
            }
        }

        public void Open(string filepath)
        {
            lock(_syncLock)
            {
                if (File.Exists(filepath))
                {
                    var json = FileAccess.Read(filepath);
                    var list = JsonConvert.DeserializeObject<List<VariableInfo>>(json);
                    _variableLookup.Clear();

                    foreach (var v in list)
                    {
                        _variableLookup.Add(v.CpuName, v);
                    }
                }
            }
        }

        public void Save(string filepath)
        {
            string path = filepath;
            if (String.IsNullOrEmpty(Path.GetExtension(filepath)))
            {
                path = $"{filepath}.config";
            }
            var fi = new FileInfo(path);

            if (!Directory.Exists(fi.DirectoryName))
            {
                Directory.CreateDirectory(fi.DirectoryName);
            }

            string json = JsonConvert.SerializeObject(new List<VariableInfo>(_variableLookup.Values));

            FileAccess.Write(fi.FullName, json);
        }
    }
}

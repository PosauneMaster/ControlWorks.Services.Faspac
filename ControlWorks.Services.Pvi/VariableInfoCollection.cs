using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Pvi
{
    public class VariableInfo
    {
        public string CpuName { get; set; }
        public string[] Variables { get; set; }
    }
    public class VariableInfoCollection
    {
        Dictionary<string, VariableInfo> _variableLookup;

        public VariableInfoCollection()
        {
            _variableLookup = new Dictionary<string, VariableInfo>();

        }

        public List<VariableInfo> GetAll()
        {
            return _variableLookup.Values.ToList();
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
            if (File.Exists(filepath))
            {
                var json = File.ReadAllText(filepath);
                var list = JsonConvert.DeserializeObject<List<VariableInfo>>(json);
                _variableLookup.Clear();

                foreach (var v in list)
                {
                    _variableLookup.Add(v.CpuName, v);
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

            string json = JsonConvert.SerializeObject(GetAll());
            File.WriteAllText(fi.FullName, json);
        }
    }
}

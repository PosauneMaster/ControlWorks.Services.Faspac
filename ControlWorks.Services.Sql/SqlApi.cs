using ControlWorks.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Sql
{
    public interface ISqlApi
    {
        Task AddSnapshot(VariableShutdownDetail detail);
    }

    public class SqlApi : ISqlApi
    {
        public async Task AddSnapshot(VariableShutdownDetail detail)
        {
            var repository = new MachineSnapshotRepository();

            await Task.Run(() => repository.Add(detail));
        }
    }
}

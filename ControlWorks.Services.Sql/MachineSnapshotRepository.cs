using ControlWorks.Logging;
using ControlWorks.Services.Configuration;
using ControlWorks.Services.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Sql
{
    public class MachineSnapshotRepository
    {

        private ILogger _logger;

        public MachineSnapshotRepository()
        {
            _logger = new Log4netAdapter(LogManager.GetLogger("ServiceLogger"));
        }


        public void Add(VariableShutdownDetail entity)
        {
            _logger.Log(new LogEntry(LoggingEventType.Information, $"MachineSnapshotRepository.Add {entity.ToJson()}"));

            var connectionStringName = ConfigurationProvider.ConnectionStringName;
            var connectionString = ConfigurationProvider.GetConnectionString(connectionStringName);

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (var command = new SqlCommand(InsertQuery(), conn))
                    {
                        command.Parameters.AddWithValue("MachineIp", entity.MachineIp);
                        command.Parameters.AddWithValue("MachineName", entity.MachineName);
                        command.Parameters.AddWithValue("PouchesPerMinute", entity.PouchesPerMinute);
                        command.Parameters.AddWithValue("CyclingTime", entity.CyclingTime);
                        command.Parameters.AddWithValue("CycleCount", entity.CycleCount);
                        command.Parameters.AddWithValue("HourMeter", entity.HourMeter);
                        command.Parameters.AddWithValue("Comment", entity.Comment);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.Log(new LogEntry(LoggingEventType.Error, $"MachineSnapshotRepository.Add {ex.Message}", ex));
                _logger.Log(new LogEntry(LoggingEventType.Error, $"connectionStringName={connectionStringName}; connectionString={connectionString}"));
                _logger.Log(new LogEntry(LoggingEventType.Error, $"commandQuery={InsertQuery()}"));

            }
        }

        private string InsertQuery()
        {
            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO [dbo].[MachineSnapshot]");
            sb.AppendLine("([StopDateTime]");
            sb.AppendLine(",[MachineIp]");
            sb.AppendLine(",[MachineName]");
            sb.AppendLine(",[PouchesPerMinute]");
            sb.AppendLine(",[CyclingTime]");
            sb.AppendLine(",[CycleCount]");
            sb.AppendLine(",[HourMeter]");
            sb.AppendLine(",[Comment])");
            sb.AppendLine("VALUES");
            sb.AppendLine("(GETDATE()");
            sb.AppendLine(",@MachineIp");
            sb.AppendLine(",@MachineName");
            sb.AppendLine(",@PouchesPerMinute");
            sb.AppendLine(",@CyclingTime");
            sb.AppendLine(",@CycleCount");
            sb.AppendLine(",@HourMeter");
            sb.AppendLine(",@Comment )");

            return sb.ToString();

        }

    }
}

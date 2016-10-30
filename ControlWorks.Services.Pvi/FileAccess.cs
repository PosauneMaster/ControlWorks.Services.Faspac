using System.IO;
using System.Threading;

namespace ControlWorks.Services.Pvi
{
    public static class FileAccess
    {
        private static object _syncLock = new object();

        private static AutoResetEvent _waitHandle = new AutoResetEvent(false);

        public static string Read(string filepath)
        {
            _waitHandle.WaitOne(1000);
            string json = null;
            lock (_syncLock)
            {
                using (var reader = new StreamReader(filepath))
                {
                    json = reader.ReadToEnd();
                }
            }

            _waitHandle.Set();

            return json;
        }

        public static void Write(string filepath, string contents)
        {
            _waitHandle.WaitOne(1000);

            lock (_syncLock)
            {
                using (var writer = new StreamWriter(filepath, false))
                {
                    writer.WriteLine(contents);
                }
            }

            _waitHandle.Set();

        }
    }
}

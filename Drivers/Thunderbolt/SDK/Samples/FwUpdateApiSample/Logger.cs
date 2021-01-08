using FwUpdateDriverApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Diagnostics;

namespace FwUpdateApiSample
{
    internal class Logger: ILogger
    {
        private static readonly Lazy<Logger> _instance = new Lazy<Logger>(() => new Logger());

        public static Logger Instance => _instance.Value;

        private LoggingChannel _channel;

        private Logger()
        {
            _channel = new LoggingChannel("TbtFwUpdateLog", null, new Guid("89C22C1B-7C52-49D1-B450-9AAAC4B72102"));
        }

        public void LogInfo(string strMessage, [CallerMemberName] string member = "", [CallerFilePath] string filepath = "", [CallerLineNumber] int line = 0)
        {
            _LogMsg(strMessage, LoggingLevel.Information, member, filepath, line);
        }

        public void LogWarn(string strMessage, [CallerMemberName] string member = "", [CallerFilePath] string filepath = "", [CallerLineNumber] int line = 0)
        {
            _LogMsg(strMessage, LoggingLevel.Warning, member, filepath, line);
        }

        public void LogErr(string strMessage, [CallerMemberName] string member = "", [CallerFilePath] string filepath = "", [CallerLineNumber] int line = 0)
        {
            _LogMsg(strMessage, LoggingLevel.Error, member, filepath, line);
        }

        private void _LogMsg(string strMessage, LoggingLevel level, [CallerMemberName] string member = "", 
            [CallerFilePath] string filepath = "", [CallerLineNumber] int line = 0)
        {
            var fileName = _ToShortFileName(filepath);
            var str = String.Format("{0} {1} [{2}@{3}:{4}] {5}", DateTime.Now.ToString("hh:mm:ss.fff"),
                level, member, fileName, line, strMessage);
            _channel.LogMessage(str, LoggingLevel.Verbose);
        }

        private static string _ToShortFileName(string filepath, int nameTokensCnt = 2)
        {
            var tokens = filepath.Split('\\');
            if (tokens.Count() <= nameTokensCnt)
            {
                return String.Join("\\", tokens);
            }
            return String.Join("\\", tokens.Skip(tokens.Count() - nameTokensCnt));
        }
    }
}

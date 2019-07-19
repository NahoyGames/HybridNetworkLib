using System;

namespace HybridNetworkLib.Runtime
{
    public static class Logger
    {
        private static Action<string> _logMethod = Console.WriteLine;
        public static void SetLogMethod(Action<string> method) => _logMethod = method;
        
        private static Action<string> _warnMethod = Console.WriteLine;
        public static void SetWarnMethod(Action<string> method) => _warnMethod = method;
        
        private static Action<string> _errorMethod = Console.WriteLine;
        public static void SetErrorMethod(Action<string> method) => _errorMethod = method;

        public static void Log(string msg)
        {
            if (_logMethod == Console.WriteLine)
            {
                msg = "[Log] " + msg;
                Console.ResetColor();
            }
            
            _logMethod("[HybridNetLib]" + msg);
        }
        
        public static void Warn(string msg)
        {
            if (_logMethod == Console.WriteLine)
            {
                msg = "[Warn] " + msg;
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            _warnMethod("[HybridNetLib]" + msg);
        }

        public static void Error(string msg)
        {
            if (_logMethod == Console.WriteLine)
            {
                msg = "[Error] " + msg;
                Console.ForegroundColor = ConsoleColor.Red;
            }

            _errorMethod("[HybridNetLib]" + msg);
        }
    }
}
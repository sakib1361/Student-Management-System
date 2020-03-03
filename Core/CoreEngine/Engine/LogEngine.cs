using CoreEngine.Helpers;
using System;
using System.Runtime.CompilerServices;

namespace CoreEngine.Engine
{
    public class LogEngine
    {

        public static bool IsDetailed = false;
        private static IToastService _toastService;

        public static void Initialize(IToastService toastService)
        {
            _toastService = toastService;
        }

        public static void Error(Exception ex, [CallerMemberName]string name = "")
        {
            string msg;
            if (IsDetailed)
            {
                msg = string.Format("{0} [{1}] => {2}", DateTime.Now.ToShortTimeString(), name, ex.ToString());
            }
            else
            {
                msg = ex.Message;
            }
            _toastService?.ShowMessage("Error", ex.Message);
            Console.WriteLine(msg);
        }
    }
}

using System.Diagnostics;
using System.Reflection;
using Serilog;

namespace ATM
{
    public class LogLogic
    {
        public void LogMethodName()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame frame = stackTrace.GetFrame(1); // 1 - індекс в стек-відстеженні для методу, який викликав LogMethodName
            MethodBase method = frame?.GetMethod();

            if (method != null)
            {
                Console.WriteLine($"Method called: {method.Name}");
            }
            else
            {
                Console.WriteLine("Unable to determine calling method");
            }
        }
    }
}

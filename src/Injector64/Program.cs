using System.Diagnostics;

namespace Injector64
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int pid = int.Parse(args[0]);
                string fullPath = Environment.GetCommandLineArgs()[0];
                string directory = Path.GetDirectoryName(fullPath);
                DllInject.Inject(pid, directory + "\\LightHookCore.dll");
                Environment.Exit(0);
            }
            catch
            {
                Environment.Exit(1);
            }
        }
    }
}


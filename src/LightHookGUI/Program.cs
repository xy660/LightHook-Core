using System.Runtime.InteropServices;

namespace LightHook
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += (object s, UnhandledExceptionEventArgs ev) =>
            {
                MessageBox.Show(ev.ExceptionObject.ToString());
                while (true)
                {
                    Thread.Sleep(999999);
                }
            };
            Application.ThreadException += (object s, ThreadExceptionEventArgs e) =>
            {
                MessageBox.Show(e.ToString());
            };
#if !DEBUG
            ComWrappers.RegisterForMarshalling(WinFormsComInterop.WinFormsComWrappers.Instance);
#endif
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
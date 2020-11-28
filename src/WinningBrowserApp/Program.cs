using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetDimension.NanUI;

namespace WinningBrowserApp
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

#if NETCOREAPP3_1 || NET5_0
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
#endif
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
         
            ShortCutHelper.CreateShortcutOnDesktop("卫宁基层卫生信息系统V5.6", Application.ExecutablePath);
            WinFormium.CreateRuntimeBuilder(env =>
            {
                // You should do some initializing staffs of Cef Environment in this function.

                env.CustomCefCommandLineArguments(cmdLine =>
                {
                    // Configure command line arguments of Cef here.

                    //cmdLine.AppendSwitch("disable-gpu");
                    //cmdLine.AppendSwitch("disable-gpu-compositing");
                   cmdLine.AppendSwitch("disable-web-security");
                   cmdLine.AppendSwitch("unsafely-treat-insecure-origin-as-secure", Wnconfig.GetUrl());
                   cmdLine.AppendSwitch("enable-media-stream", "1");


                });

                env.CustomCefSettings(settings =>
                {
                    // Configure default Cef settings here.
                    //settings.WindowlessRenderingEnabled = true;
                    //settings.
                    
                });

                env.CustomDefaultBrowserSettings(cefSettings =>
                {
                  
                  //  cefSettings.WebSecurity = Xilium.CefGlue.CefState.Disabled;

                    // Configure default browser settings here.

                });
            },
            app =>
            {
                // You can configure your application settings of NanUI here.
#if DEBUG
                // Use this setting if your application running in DEBUG mode, it will allow user to open or clode DevTools by right-clicking mouse button and selecting menu items on context menu.
                //app.UseDebuggingMode();
#endif



                // Clear all cached files such as cookies, histories, localstorages, etc.
                if (args!=null && args.Length>0 && args[0] == "DeleteCache")
                {
                    app.ClearCacheFile();
                     
                 }

               

                // Set a main window class inherit Formium here to start appliation message loop.
                app.UseMainWindow(context =>
                {
                    
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    TaskCompletionSource<object> source = new TaskCompletionSource<object>();
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            RunServiceProxy.InvokProxy();
                            source.SetResult("启动成功");

                        }
                        catch (Exception ex)
                        {
                            source.SetException(ex);
                        }
                    });
                    //thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();

                    // You should return a Formium instatnce here or you can use context.MainForm property to set a Form which does not inherit Formium.

                    // context.MainForm = new Form();
                    Xilium.CefGlue.CefRuntime.AddCrossOriginWhitelistEntry("*", "http", "*", true);
                    return new MainForm();
                });

            })
            // Build the NanUI runtime
            .Build()
            // Run the main process
            .Run();
        }
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            // Log the exception, display it, etc
            // MessageBox.Show(e.Exception.Message);
            MainForm.Restart();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MainForm.Restart();
            // Log the exception, display it, etc
            //MessageBox.Show((e.ExceptionObject as Exception).Message);
        }
    }
}

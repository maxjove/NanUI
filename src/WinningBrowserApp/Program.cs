using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetDimension.NanUI;
using NetDimension.NanUI.LocalFileResource;

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
            //bool createNew;
            //using (System.Threading.Mutex m = new System.Threading.Mutex(true, Application.ProductName, out createNew))
            //{
            //    if (!createNew)
            //    {
            //        MessageBox.Show("只能运行一个实例!");
            //        return;
            //    }
            //}
            //MessageBox.Show("只能运行一个实例!");
            Application.ThreadException -= new ThreadExceptionEventHandler(Application_ThreadException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException -= new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            //MainForm mfrm = null;
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
                    //--disk-cache-size=xxx
                   // cmdLine.AppendSwitch("disk-cache-size", "1");
                    if (!Environment.Is64BitProcess)
                    {
                        //MessageBox.Show("设置内存");
                        cmdLine.AppendArgument("max-old-space-size=1000");
                     }


                });

                env.CustomCefSettings(settings =>
                {
                    //MessageBox.Show(settings.CachePath);
                    // Configure default Cef settings here.
                    //settings.WindowlessRenderingEnabled = true;
                    //settings.
                 
                 

                });

                env.CustomDefaultBrowserSettings(cefSettings =>
                {

                    cefSettings.ApplicationCache= Xilium.CefGlue.CefState.Disabled;
                  //  cefSettings.WebSecurity = Xilium.CefGlue.CefState.Disabled;

                    // Configure default browser settings here.

                });
            },
            app =>
            {

#if DEBUG
                // Use this setting if your application running in DEBUG mode, it will allow user to open or clode DevTools by right-clicking mouse button and selecting menu items on context menu.
                //app.UseDebuggingMode();
#endif


                app.UseLocalFileResource("http", "demo.app.local", "asserts");
                app.RegisterJavaScriptExtension(() => new WinningBrowerWindowExtension());
                // Clear all cached files such as cookies, histories, localstorages, etc.
                if (args != null && args.Length > 0 && args[0] == "DeleteCache")
                {
                    //RunServiceProxy.InvokProxy();

                    app.ClearCacheFile();


                }
                Thread.Sleep(600);
                //var thisProcess = Process.GetCurrentProcess();
                //if (Process.GetProcessesByName(thisProcess.ProcessName).Length == 1)
                //{
                //    if (Process.GetProcessesByName(thisProcess.ProcessName)[0].Id != thisProcess.Id)
                //    {
                //        Process.GetProcessesByName(thisProcess.ProcessName)[0].Kill();
                //    }
                //}

                app.UseSingleInstance(() =>
                {
                    //MessageBox.Show("只能运行一个程序");

                    RunServiceProxy.SetWebAppFrmFront();
                    return;
                });

                // Set a main window class inherit Formium here to start appliation message loop.
                app.UseMainWindow(context =>
                {
                    
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);



                    //Process[] processesByName = Process.GetProcessesByName("ProxySvr4HIS");

                    //if (processesByName == null || processesByName.Length <= 0)
                    //{
                    //RunServiceProxy.InvokProxy();
                   
                    //}
                    ShortCutHelper.CreateShortcutOnDesktop("卫宁基层卫生信息系统V5.6", Application.ExecutablePath);
                    // You should return a Formium instatnce here or you can use context.MainForm property to set a Form which does not inherit Formium.

                    // context.MainForm = new Form();
                    //Xilium.CefGlue.CefRuntime.AddCrossOriginWhitelistEntry("*", "http", "*", true);
                    //mfrm= new MainForm();
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
            //MainForm.Restart();
            
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
           // MainForm.Restart();
            // Log the exception, display it, etc
            //MessageBox.Show((e.ExceptionObject as Exception).Message);
        }
    }
}

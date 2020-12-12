using NetDimension.NanUI;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinningBrowserApp
{
  public class RunServiceProxy
    {
        public static void InvokProxy()
        {
            
            string text = Application.StartupPath + "\\NetHisProxy\\ProxySvr4HIS.exe";
            
            if (!File.Exists(text))
            {
                MessageBox.Show(string.Format("在目录{0}未找到代理服务文件!", text));
                return;
            }
            //MessageBox.Show("restartProxy");
            restartProxy();


           
        }
        public static void DelayRestartProxy()
        {
            Thread.Sleep(2000);
            //restartProxy();
            Process[] processesByName = Process.GetProcessesByName("ProxySvr4HIS");
            string text = Application.StartupPath + "\\NetHisProxy\\ProxySvr4HIS.exe";
            if (processesByName == null || processesByName.Length <= 0)
            {
                Process.Start(new ProcessStartInfo(text));
            }
        }
        public static void MonitorProxyRun()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(5000);
                    Process[] processesByName = Process.GetProcessesByName("ProxySvr4HIS");
                    string text = Application.StartupPath + "\\NetHisProxy\\ProxySvr4HIS.exe";
                    if (processesByName == null || processesByName.Length <= 0)
                    {
                        Process p = Process.Start(new ProcessStartInfo(text));
                        break;

                    }
                    else
                    {
                        Thread.Sleep(5000);
                    }
                }
            }
            catch (Exception)
            {

                
            }
            
        }
        public static void restartProxy()
        {

          
           
            try
            {
                Process[] processesByName = Process.GetProcessesByName("ProxySvr4HIS");
                string text = Application.StartupPath + "\\NetHisProxy\\ProxySvr4HIS.exe";
                if (processesByName != null && processesByName.Length > 0)
                {
                    
                    var client = new RestClient("http://127.0.0.1:9045/CisRestService/ConfigRestService/proxycmd");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "text/plain");
                    request.AddParameter("text/plain", string.Format( "RESTART_PROXY|{0}", text), ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    Thread.Sleep(300);
                }
                else
                {
                    Process p = Process.Start(new ProcessStartInfo(text)
                    {


                        //WindowStyle = ProcessWindowStyle.Hidden,

                        //CreateNoWindow = true
                    });
                }
            }
            catch (Exception)
            {

                
            }
            
            
        }

        public static void DeleteCache()
        {


            MainForm.CacheString = WinFormium.DefaultAppDataDirectory;
            try
            {
                Process[] processesByName = Process.GetProcessesByName("ProxySvr4HIS");
               
                if (processesByName != null && processesByName.Length > 0)
                {
                    var client = new RestClient("http://127.0.0.1:9045/CisRestService/ConfigRestService/proxycmd");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "text/plain");
                    request.AddParameter("text/plain", string.Format("DELETE_CACHE|{0}|{1}", MainForm.CacheString,Process.GetCurrentProcess().MainModule.FileName), ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                }
              
            }
            catch (Exception)
            {


            }

        }
        public const uint SW_SHOW = 5;
       
        internal static void SetWebAppFrmFront()
        {
            RaiseOtherProcess();
           

        }
        private const uint Restore = 9;
        public static void ActivateWindow(IntPtr mainWindowHandle)
        {
            //check if already has focus
            if (mainWindowHandle == WindowsApi.GetForegroundWindow()) return;

            //check if window is minimized
            if (WindowsApi.IsIconic(mainWindowHandle))
            {
                WindowsApi.ShowWindow(mainWindowHandle, Restore);
            }

            
            WindowsApi.SwitchToThisWindow(mainWindowHandle, true);
            WindowsApi.SetForegroundWindow(mainWindowHandle);
        }

        /// <summary>
        /// 打开该程序主窗口
        /// </summary>
        public static void RaiseOtherProcess()
        {
            System.Diagnostics.Process proc = System.Diagnostics.Process.GetCurrentProcess();
            Process[] Proes = System.Diagnostics.Process.GetProcessesByName(proc.ProcessName);
            foreach (System.Diagnostics.Process otherProc in Proes)
            {
                if (proc.Id != otherProc.Id)
                {
                    IntPtr hWnd = otherProc.MainWindowHandle;
                    if (hWnd.ToInt32() == 0)
                    {

                        hWnd = WindowsApi.FindWindow(null, $"{MainForm.WbSubtitle} - {MainForm.WbTitle}");

                        int id = -1;
                        WindowsApi.GetWindowThreadProcessId(hWnd, out id);
                        if (id == otherProc.Id)
                            break;

                    }
                    ActivateWindow(hWnd);
                    //此处获取的hWnd即为之前运行程序的主窗口句柄，再使用其他函数打开窗体
                    break;
                }
            }
        }
    }
}

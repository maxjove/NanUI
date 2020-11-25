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
            restartProxy();


            //Process[] processesByName = Process.GetProcessesByName("ProxySvr4HIS");

            //if (processesByName != null && processesByName.Length > 0)
            //{

            //    if (processesByName[0].MainModule.FileName != text)
            //    {
            //        processesByName[0].Kill();


            //        Process[] arrProLog = Process.GetProcessesByName("NetHisWebconsole");
            //        if (arrProLog != null && arrProLog.Length > 0)
            //        {
            //            foreach (Process p in arrProLog)
            //            {
            //                if (p.HasExited)
            //                    p.Kill();
            //            }
            //        }
            //        processesByName = null;
            //    }
            //    else
            //    {
            //        restartProxy();
            //    }
            //}

            //if (processesByName == null || processesByName.Length == 0)
            //{
            //  Process p=  Process.Start(new ProcessStartInfo(text)
            //    {


            //        //WindowStyle = ProcessWindowStyle.Hidden,

            //        //CreateNoWindow = true
            //    });

            //}
            //else
            //{

            //    return;
            //}
        }
        public static void DelayRestartProxy()
        {
            //Thread.Sleep(25000);
            //restartProxy();
        }
        public static void restartProxy()
        {

            //Process[] arrPro1 = Process.GetProcessesByName("ProxySvr4HIS");
            //if (arrPro1 != null && arrPro1.Length > 0)
            //{
            //    foreach (Process p in arrPro1)
            //    {
            //        p.Kill();
            //    }
            //}
            //Process[] arrPro = Process.GetProcessesByName("RestSvrHost");
            //if (arrPro != null && arrPro.Length > 0)
            //{
            //    foreach (Process p in arrPro)
            //    {
            //        p.Kill();
            //    }
            //}

            //Process[] arrProLog = Process.GetProcessesByName("NetHisWebconsole");
            //if (arrProLog != null && arrProLog.Length > 0)
            //{
            //    foreach (Process p in arrProLog)
            //    {
            //        if (p.HasExited)
            //            p.Kill();
            //    }
            //}
            //Thread.Sleep(200);
            // InvokProxy();
           
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
        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern bool SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        //private static void SetProxyFrmFront()
        //{
        //    bool hasFound = false;
        //    Process processInfo = null;
        //    foreach (Process process in Process.GetProcesses())
        //    {
        //        if (process.ProcessName == "ProxySvr4HIS")
        //        {
        //            processInfo = process;
        //            hasFound = true;
        //            break;
        //        }
        //    }
        //    if (!hasFound)
        //    {
        //        //MessageBox.Show("未找到指定进程");
        //        Console.WriteLine("未找到指定进程");

        //    }
        //    //移动到最前
        //    SwitchToThisWindow(processInfo.MainWindowHandle, true);
        //}
    }
}

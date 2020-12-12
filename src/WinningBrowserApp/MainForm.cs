using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetDimension.NanUI;
using NetDimension.NanUI.HostWindow;
using NetDimension.NanUI.JavaScript;
using Xilium.CefGlue;

namespace WinningBrowserApp
{
    public class MainForm : Formium
    {
        // You must indicate a style of HostWindow.
        public override HostWindowType WindowType { get; } = HostWindowType.Borderless;


        // You must specify the startup url here.


        //public override string StartUrl { get; } = "http://main.app.local/";
        //public override string StartUrl { get; } = "http://localhost:3000"; // For development purpose
        //http://172.17.0.130:10000/basic-frame/#/login;
        public override string StartUrl { get; } = Wnconfig.GetUrl();

        internal static string WbTitle = "卫宁浏览器";
        internal static string WbSubtitle = "云HIS5.6";
        internal static string CacheString;
        public MainForm()
        {

            //StartPosition = FormStartPosition.CenterScreen;
            // Set form settings here.
           MinimumSize = new Size(1000, 700);

            this.WindowState = FormWindowState.Maximized;
          
            Icon = Properties.Resources.nethis56;

            Title = WbTitle;
            Subtitle = WbSubtitle;

            
            //this.CanMaximize = true;
            //this.CanMinimize = true;
            

           
            
            // Set up settings for BorderlessWindow style.

            BorderlessWindowProperties.BorderEffect = BorderEffect.BorderLine;
            BorderlessWindowProperties.ShadowEffect = ShadowEffect.Shadow;
            BorderlessWindowProperties.ShadowColor = Color.DimGray;

            // Customize the Splash with Mask property.
          
            CustomizeMaskPanel();
            Thread thread = new Thread(() =>
            {
                try
                {
                    RunServiceProxy.MonitorProxyRun();


                }
                catch (Exception ex)
                {

                }
            });
            //thread.SetApartmentState(ApartmentState.STA);
            thread.Start();



        }

        private void MainForm_RenderProcessTerminated(object sender, NetDimension.NanUI.Browser.RenderProcessTerminatedEventArgs e)
        {
            try
            {
                e.ShouldTryResetProcess = true;
                //Reload(true);
            }
            catch (Exception)
            {

                 
            }
            
            // throw new NotImplementedException();
        }

        private void MainForm_BeforeClose(object sender, NetDimension.NanUI.Browser.FormiumCloseEventArgs e)
        {
            ShowMask();
            if (MessageBox.Show(this.HostWindow, "是否关闭浏览器?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {

                
                e.Canceled = true;
            }
            else
            {
                //Thread.Sleep(1500);
            }
            CloseMask();
            //throw new NotImplementedException();
        }

        // Create the splash panel
        private void CustomizeMaskPanel()
        {
            //var label = new Label
            //{
            //    Text = "WinFormium\nExample Application\nPowered by NanUI",
            //    AutoSize = false,
            //    TextAlign = ContentAlignment.MiddleCenter,
            //    Anchor = AnchorStyles.None,
            //    ForeColor = Color.White,
            //    Font = new Font("Segoe UI Light", 24.0f, GraphicsUnit.Point)
            //};

            //label.Width = Width;
            //label.Height = Height / 2;
            //label.Location = new Point(0, (Height - label.Height) / 2);

            //var loaderGif = new PictureBox
            //{
            //    Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
            //    Size = new Size(40, 40),
            //    Image = Properties.Resources.Indicator,
            //    SizeMode = PictureBoxSizeMode.CenterImage
            //};

            //loaderGif.Location = new Point(Width - (int)(loaderGif.Width * 2.0f), Height - (int)(loaderGif.Height * 1.5f));

            // Add a label

            // Mask.Content.Add(label);

            // Add a picture box

            // Mask.Content.Add(loaderGif);
            Mask.ImageLayout = ImageLayout.Stretch;
            Mask.Image = Properties.Resources.启动图;

        }


        // Raise if browser is ready.
        protected override void OnReady()
        {
            // Set browser settings here.


            BeforeClose += MainForm_BeforeClose;
            RenderProcessTerminated += MainForm_RenderProcessTerminated;
            BeforePopup += MainWindow_BeforePopup;
            KeyEvent += MainForm_KeyEvent;
            LoadError += MainForm_LoadError;
           

            //TaskCompletionSource<object> source = new TaskCompletionSource<object>();
            //Thread thread = new Thread(() =>
            //{
            //    try
            //    {
            //        RunServiceProxy.InvokProxy();
            //        //source.SetResult("启动成功");
            //    }
            //    catch (Exception ex)
            //    {
            //        //source.SetException(ex);
            //    }
            //});
            ////thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();

        }

        private void MainForm_LoadError(object sender, NetDimension.NanUI.Browser.LoadErrorEventArgs e)
        {
            try
            {
                string strmsg = string.Format("{0}\n网址无法打开,原因{1},是否刷新重试?", e.FailedUrl, e.ErrorText);
               
                if ((e.FailedUrl == Wnconfig.GetUrl()) && (MessageBox.Show(strmsg, "信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes))
                {
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            delRestart();




                        }
                        catch (Exception ex)
                        {
                            
                        }
                    });
                    //thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                }
            }
            catch (Exception)
            {

                
            }
           

            //throw new NotImplementedException();
        }

        private void MainForm_KeyEvent(object sender, NetDimension.NanUI.Browser.KeyEventArgs e)
        {
            if (e.KeyEvent.WindowsKeyCode == 123 && e.KeyEvent.NativeKeyCode < 0)
            {
                ShowDevTools();
            }

            if (e.KeyEvent.WindowsKeyCode == 115 && e.KeyEvent.NativeKeyCode < 0)
            {
                if (MessageBox.Show("删除缓存会自动重启浏览器,是否删除缓存?", "信息", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    
                    Restart();
                    


                }

            }
            if (e.KeyEvent.WindowsKeyCode == 116 && e.KeyEvent.NativeKeyCode < 0)
            {
                Reload(true);
            }

        }
        public static void delRestart()
        {
            Thread.Sleep(500);
            Restart();
        }
        public static void Restart()
        {
            ProcessStartInfo startInfo = Process.GetCurrentProcess().StartInfo;
            startInfo.FileName = Application.ExecutablePath;
            var exit = typeof(Application).GetMethod("ExitInternal",
                                System.Reflection.BindingFlags.NonPublic |
                                System.Reflection.BindingFlags.Static);
            exit.Invoke(null, null);
            startInfo.Arguments = "DeleteCache";
            Process.Start(startInfo);
        }


        // Handle Before Popup event. If links have a target property that value equals "_blank", open the link in default browser of current system.
        private void MainWindow_BeforePopup(object sender, NetDimension.NanUI.Browser.BeforePopupEventArgs e)
        {
            if (e.TargetUrl != "about:blank#blocked")
            {
                e.Handled = true;

                InvokeIfRequired(() =>
                {
                    OpenExternalUrl(e.TargetUrl);
                });
            }
        }


        private void OpenExternalUrl(string url)
        {
            var ps = new System.Diagnostics.ProcessStartInfo(url)
            {
                UseShellExecute = true,
                Verb = "open"
            };
            System.Diagnostics.Process.Start(ps);
        }


    

   

    

    }
}

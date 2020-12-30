using NetDimension.NanUI;
using NetDimension.NanUI.JavaScript;

namespace WinningBrowserApp
{
    internal class WinningBrowerWindowExtension : JavaScriptExtensionBase
    {
        public override string Name => "WBrower/MainForm";
        public override string JavaScriptCode => Properties.Resources.WinningBrowerWindowJavaScriptExtension;

        public WinningBrowerWindowExtension()
        {
            RegisterFunctionHandler(nameof(Minimize), Minimize);

            RegisterFunctionHandler(nameof(Maximize), Maximize);

            RegisterFunctionHandler(nameof(Close), Close);

            RegisterFunctionHandler(nameof(Restore), Restore);

           

        }

      


      
        private JavaScriptValue Minimize(Formium owner, JavaScriptValue[] arguments)
        {
            if (owner.WindowType == NetDimension.NanUI.HostWindow.HostWindowType.Kiosk || !owner.CanMinimize)
                return null;

            if (owner.WindowState != System.Windows.Forms.FormWindowState.Minimized)
            {
                owner.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            }
            return null;
        }
        private JavaScriptValue Maximize(Formium owner, JavaScriptValue[] arguments)
        {

            if (owner.WindowType == NetDimension.NanUI.HostWindow.HostWindowType.Kiosk || !owner.CanMaximize)
                return null;


            if (owner.WindowState != System.Windows.Forms.FormWindowState.Maximized)
            {
                owner.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            }
            else if (owner.WindowState == System.Windows.Forms.FormWindowState.Maximized)
            {
                if (owner.IsFullScreen)
                {
                    owner.FullScreen(false);
                }
                else
                {
                    owner.WindowState = System.Windows.Forms.FormWindowState.Normal;
                }
            }

            return null;
        }
        private JavaScriptValue Restore(Formium owner, JavaScriptValue[] arguments)
        {
            if (owner.WindowType == NetDimension.NanUI.HostWindow.HostWindowType.Kiosk)
                return null;

            if (owner.WindowState != System.Windows.Forms.FormWindowState.Normal)
            {
                owner.WindowState = System.Windows.Forms.FormWindowState.Normal;
            }

            return null;
        }

        private JavaScriptValue Close(Formium owner, JavaScriptValue[] arguments)
        {
            if (owner.WindowType == NetDimension.NanUI.HostWindow.HostWindowType.Kiosk)
                return null;

            owner.Close(true);

            return null;
        }

      
      
    }
}

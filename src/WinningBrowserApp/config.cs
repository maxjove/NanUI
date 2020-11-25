using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinningBrowserApp
{
   internal class Wnconfig
    {
        public static string GetUrl()
        {
            string url = ConfigurationManager.AppSettings["DefaultURL"];
            return url;
            //return @"http://res.app.local/asserts/index.html";
        }
        public static  PictureBox  GetScImage()
        {
            PictureBox px = new PictureBox();

            px.SizeMode = PictureBoxSizeMode.StretchImage;
            px.Image = Properties.Resources.启动图;


            return px;
        }
    }
}

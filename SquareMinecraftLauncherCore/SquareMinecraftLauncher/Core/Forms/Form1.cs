using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicrosoftLoginFroms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Url = new Uri(url1);
        }
        public static string url = "";
        internal static string url1;

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (webBrowser1.Url.AbsoluteUri.IndexOf("https://login.live.com/oauth20_desktop.srf") >= 0)
            {
                url = webBrowser1.Url.AbsoluteUri;
                webBrowser1.Dispose();
                this.Close();
            }
            
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }

    

}

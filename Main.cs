using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;


namespace MultiWebView2Sample
{
    public partial class Main : Form
    {
        CoreWebView2Environment env;
        string userDataFolder;
        int browserCount = 0;
        int browsersPerRow = 4;

        public Main()
        {
            InitializeComponent();
            userDataDirectorySetup();
            webView2Setup();
        }

        private void userDataDirectorySetup()
        {
            Debug.Print("Entering userDataDirectorySetup.. ");
            string userprofile = Environment.GetEnvironmentVariable("USERPROFILE");
            userDataFolder = $"{userprofile}\\AppData\\Local\\MultiWebView2Sample\\";

            if (!Directory.Exists(userDataFolder))
            {
                DirectoryInfo di = Directory.CreateDirectory(userDataFolder);
                Debug.Print($"Created directory {userDataFolder}");
            }

            Debug.Print("Leaving userDataDirectorySetup.. ");
        }

        async private void webView2Setup()
        {
            Debug.Print("Entering webView2Setup.. ");
            env = await CoreWebView2Environment.CreateAsync(null, userDataFolder, null);
            /*
             *  CreateAsync( 
             *      browserExecutableFolder should remain null, since you should be running this from Visual Studio
             *      userDataFolder should be "C:\\Users\\username\\AppData\\Local\\MultiWebView2Sample\\" or similar, once WebView2 is created programmatically it will create an EBWebView folder
             *      options should be null, but you can even specify some and test this out. It should use the same instance over a 'null' value as long as we're using the same environment.
             *      
             */

            //CoreWebView2EnvironmentOptions opts;
            //opts.AdditionalBrowserArguments = "";
            //opts.AllowSingleSignOnUsingOSPrimaryAccount = true;
            //opts.Language = "es-US";
            //opts.TargetCompatibleBrowserVersion = true;

            Debug.Print($"Created env {env}");

            Debug.Print("Leaving webView2Setup.. ");
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            Debug.Print("Entering createButton_Click..");
            createWebView2();
            Debug.Print("Leaving createButton_Click..");
        }

        async private void createWebView2()
        {
            Debug.Print("Entering makeBrowser..");
            WebView2 wv2 = new WebView2();
            wv2.Width = 200;
            wv2.Height = 200;
            wv2.Left = 201 * (browserCount % browsersPerRow);
            wv2.Top = 201 * (browserCount / browsersPerRow);

            Debug.Print($"{wv2} added to Main form");

            await wv2.EnsureCoreWebView2Async(env);
            // If you pass null (the default value) then a default environment will be created and used automatically.
            // from  https://docs.microsoft.com/en-us/dotnet/api/microsoft.web.webview2.winforms.webview2.ensurecorewebview2async?view=webview2-dotnet-1.0.864.35#parameters 

            Controls.Add(wv2);
            wv2.CoreWebView2.Navigate(@"https://www.bing.com");

            browserCount++;

            Debug.Print($"Leaving makeBrowser, another WebView2 added: {browserCount} total");
        }
    }
}

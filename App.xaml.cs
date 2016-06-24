using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using WpfMvvmSample.View;

namespace WpfMvvmSample
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MediaPlayerView win = new MediaPlayerView();
            win.Width = 1800;
            win.MinWidth = 1000;
            win.Height = 800;
            win.MinHeight = 800;
            win.WindowState = WindowState.Normal;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }
    }
}

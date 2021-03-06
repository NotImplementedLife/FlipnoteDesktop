﻿using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.External.Plugins;
using FlipnoteDotNet.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlipnoteDotNet.Windows
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    internal partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
            BuildVersion.Text = App.Version.ToString();
            Loaded += SplashWindow_Loaded;
        }

        /// <summary>
        /// Show logo for 3 seconds, then launch the MainWindow
        /// </summary>      
        private void SplashWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {                
                Log("Checking for user data...");
                if (File.Exists(".fsuserdata"))
                {
                    try
                    {
                        using (BinaryReader r = new BinaryReader(File.Open(".fsuserdata", FileMode.Open)))
                        {

                            Dispatcher.Invoke(() => App.AuthorName = r.ReadWChars(11).Trim('\0'));
                            Dispatcher.Invoke(() => App.AuthorId = r.ReadBytes(8));

                        }
                    }
                    catch (Exception)
                    {
                        Dispatcher.Invoke(() => MessageBox.Show("The file \".fsuserdata\" exists but seems to be corrupted. You nees to reload your Flipnote user data", "Could not load user data."));
                    }
                }
                Thread.Sleep(1000);

                Log("Loading plugins...");
                if (!Directory.Exists("Plugins")) 
                {
                    Directory.CreateDirectory("Plugins");
                }
                Directory.GetFiles("Plugins", "*.dll", SearchOption.TopDirectoryOnly).AsParallel().ForAll((fn) =>
                {
                    Log($"Loading plugins... ({System.IO.Path.GetFileName(fn)})");
                    Thread.Sleep(500);
                    PluginManager.ImportPlugins(fn);
                });
                
                Thread.Sleep(3000);
                Log("Ready");
                Dispatcher.Invoke(() =>
                {
                    var wnd = new MainWindow();
                    PluginManager.Populate(wnd);
                    wnd.Show();
                    Close();                    
                });
            });
        }       
        
        /// <summary>
        /// Message to display loading progress
        /// </summary>
        private void Log(string msg)
        {
            Dispatcher.Invoke(() => { LogMsg.Text = msg; });
        }
    }
}

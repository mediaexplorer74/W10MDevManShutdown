using DeviceManager.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DeviceManager.SampleApp.Shutdown
{
    

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfigPage : Page
    {
       
        ResourceLoader loader;
        MobileDevice device;

        // ConfigPage
        public ConfigPage()
        {
            this.InitializeComponent();
            
                       
            loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            
            SetStatusBar(Colors.Black, Colors.White);
        }
        private static async void SetStatusBar(Color bc, Color fc)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundColor = bc;
                statusBar.ForegroundColor = fc;
                statusBar.BackgroundOpacity = 1;
                await statusBar.ShowAsync();
            }
        }
        private async void help_btn_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://yandex.ru/search/?lr=213&text=quick-shutdown-for-windows-10-mobile"));
        }


        // connect_btn_Click
        private async void connect_btn_Click(object sender, RoutedEventArgs e)
        {
            App.IPAddr = addr_tbx.Text;

            if (App.IPAddr == "127.0.0.1")
            {
                // local mode 

                device = DeviceManager.Connect
                  (
                  DeviceType.Mobile,
                  ConnectionType.Local,
                  App.IPAddr //"127.0.0.1"
                  )
                  as MobileDevice;
            }
            else
            {
                // remote  mode

                device = DeviceManager.Connect
                   (DeviceType.Mobile, 
                   ConnectionType.Remote,
                   App.IPAddr
                   )
                   as MobileDevice;
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(addr_tbx.Text))
                {

                    // Remote mode 

                    device = MobileDeviceManager.ConnectRemote(addr_tbx.Text);

                    if (device != null)
                    {

                        Debug.WriteLine("Device connected remotely.");
                        
                    }


                    device.Auth
                    (
                        new ConnectCredential() 
                        { 
                            Pin = "1234",// not needed =)
                            Persistent = 0, //((bool)rememberme_cbx.IsChecked) ? 1 : 0 
                        }
                    );
                    
                    await Task.Delay(500);

                    //if (device.IsAuthed == false)
                    //{
                    //    await new MessageDialog(loader.GetString("ConnectFailedContent"), loader.GetString("ConnectFailedTitle")).ShowAsync();
                    //}
                    //else
                    //{
                        Frame.Navigate(typeof(StartPage));
                    //}
                }
            }
            catch
            {
                await new MessageDialog
                    (loader.GetString("ConnectFailedContent"), 
                    loader.GetString("ConnectFailedTitle")).ShowAsync();
            }
        }

        private async void about_btn_Click(object sender, RoutedEventArgs e)
        {
           await Launcher.LaunchUriAsync(new Uri("http://github.com/mediaexplorer74/"));
        }
    }
}

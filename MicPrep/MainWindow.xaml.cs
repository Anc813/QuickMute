using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MicPrep
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CoreAudioController AudioController = new CoreAudioController();
        public Boolean MicMuted;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Minimized;
            //this.Hide();

            this.DataContext = MicMuted;
        }

        private async void UpdateMuteStatus()
        {
            var devices = await AudioController.GetDevicesAsync(DeviceType.Capture, DeviceState.Active);
            var device = devices.FirstOrDefault(x => x.IsDefaultDevice);

            if (device != null)
            {
                this.MicMuted = device.IsMuted;
            }
        }

        private async void MuteToggle_Click(object sender, RoutedEventArgs e)
        {
            var devices = await this.AudioController.GetDevicesAsync(DeviceType.Capture, DeviceState.Active);
            var device = devices.FirstOrDefault(x => x.IsDefaultDevice);

            if(device != null)
            {
                await device.ToggleMuteAsync();
                UpdateMuteStatus();
            }
        }
    }
}

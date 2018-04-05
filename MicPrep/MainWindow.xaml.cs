using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void MuteBtn_Click(object sender, RoutedEventArgs e)
        {
            CoreAudioController AudioController = new CoreAudioController();
            var devices = await AudioController.GetDevicesAsync(DeviceType.Capture, DeviceState.Active);
            var device = devices.FirstOrDefault(x => x.IsDefaultDevice);

            if(device != null)
            {
                await device.MuteAsync(!device.IsMuted);
            }
        }
    }
}

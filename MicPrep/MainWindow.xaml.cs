using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
            this.Subscribe();

            this.WindowState = System.Windows.WindowState.Minimized;
            this.Hide();

            this.DataContext = MicMuted;
            this.UpdateMuteStatus();
        }

        private async void UpdateMuteStatus()
        {
            var devices = await AudioController.GetDevicesAsync(DeviceType.Capture, DeviceState.Active);
            var device = devices.FirstOrDefault(x => x.IsDefaultDevice);

            if (device != null)
            {
                this.MicMuted = device.IsMuted;
                this.myNotifyIcon.Icon = device.IsMuted ? Properties.Resources.off : Properties.Resources.on;
            }
        }

        public async void MuteToggle_Click(object sender, RoutedEventArgs e)
        {
            var devices = await this.AudioController.GetDevicesAsync(DeviceType.Capture, DeviceState.Active);
            var device = devices.FirstOrDefault(x => x.IsDefaultDevice);

            if (device != null)
            {
                await device.ToggleMuteAsync();
                UpdateMuteStatus();
            }
        }

        private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // shortcut https://tyrrrz.me/Blog/WndProc-in-WPF
        public HwndSource source;

        private void Subscribe()
        {
            //var source = HwndSource.FromHwnd(new WindowInteropHelper(this).EnsureHandle());

            //source.AddHook(WndProc);
        }
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Handle messages...
            Console.WriteLine(msg);
            return IntPtr.Zero;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            //source.RemoveHook(WndProc);
            //source.Dispose();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;
using System.Diagnostics;
using RCVNuget;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Transmitter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainPage()
        {
            this.InitializeComponent();
        }

        public RCReceiverFinder finder = new RCReceiverFinder();
        public RCReceiver CurrentReceiver { get; set; } = null;
        public ObservableCollection<RCReceiver> receivers = new ObservableCollection<RCReceiver>();


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            DataContext = this;
            AvailableReceivers.ItemsSource = receivers;
            finder.lost += Receiver_lost;
            finder.discovered += Receiver_discovered;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            finder.lost -= Receiver_lost;
            finder.discovered -= Receiver_discovered;
        }

        private void Receiver_discovered(object finder, RCReceiver receiver)
        {
            receivers.Add(receiver);
            OnPropertyChanged("receivers");
        }

        private void Receiver_lost(object finder, RCReceiver receiver)
        {
            receivers.Remove(receiver);
            OnPropertyChanged("receivers");
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var ignore = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    handler(this, new PropertyChangedEventArgs(name));
                });
            }
        }

        private void AvailableReceivers_ItemClick(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine("Clicked " + e.ClickedItem.ToString());
            var receiver = e.ClickedItem as RCReceiver;
            if (receiver != null)
            {
                switch (receiver.deviceType)
                {
                    case DeviceType.Tank:
                        this.Frame.Navigate(typeof(TankPage), receiver);
                        break;
                    case DeviceType.Car:
                        this.Frame.Navigate(typeof(CarPage), receiver);
                        break;
                    case DeviceType.MultiRotorCopter:
                        this.Frame.Navigate(typeof(MultiRotorCopterPage), receiver);
                        break;

                }
            }

        }
    }
}

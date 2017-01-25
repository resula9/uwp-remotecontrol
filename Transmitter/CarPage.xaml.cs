using System;
using System.Collections.Generic;
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
using RCVNuget;
using System.Diagnostics;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Transmitter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CarPage : Page
    {
        public RCReceiver receiver;

        public CarPage()
        {
            this.InitializeComponent();
            
            SteeringSlider.DataContext = this;
            ThrottleSlider.DataContext = this;
        }

        double _throttle = 0;
        double _steering = 0;
        public double throttle
        {
            get
            {
                return _throttle;
            }
            set
            {
                _throttle = value;
                Debug.WriteLine("Thottle Set: " + _throttle.ToString());

                Task.Run(async () =>
                {
                    await receiver.SetAnalogChannelStateAsync(1, value);
                });
            }
        }
        public double steering
        {
            get
            {
                return _steering;
            }
            set
            {
                _steering = value;
                Debug.WriteLine("Steering Set: " + _steering.ToString());

                Task.Run(async () =>
                {
                    await receiver.SetAnalogChannelStateAsync(0, value);
                });
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);



            receiver = e.Parameter as RCReceiver;
            receiver.LostEvent += Receiver_LostEvent;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (receiver != null)
            {
                receiver.LostEvent -= Receiver_LostEvent;
            }
        }

        private void Receiver_LostEvent(object sender, ReceiverLostReason reason)
        {
            this.Frame.GoBack();
        }
    }
}


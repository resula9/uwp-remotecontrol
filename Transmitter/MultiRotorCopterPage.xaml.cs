using RCVNuget;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Transmitter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MultiRotorCopterPage : Page
    {
        public MultiRotorCopterPage()
        {
            this.InitializeComponent();

            SteeringSlider.DataContext = this;
            ThrottleSlider.DataContext = this;
        }
        public RCReceiver receiver;

        double _throttle = 0;
        double _roll = 0;
        double _pitch = 0;
        double _yaw = 0;
        bool _arm = false;


        // todo: make these channels dynamically binding.
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
                    await receiver.SetAnalogChannelStateAsync(3, value);
                });
            }
        }
        public double roll
        {
            get
            {
                return _roll;
            }
            set
            {
                _roll = value;
                Debug.WriteLine("roll  Set: " + _roll.ToString());

                Task.Run(async () =>
                {
                    await receiver.SetAnalogChannelStateAsync(0, value);
                });
            }
        }

        public double pitch
        {
            get
            {
                return _pitch;
            }
            set
            {
                _pitch = value;
                Debug.WriteLine("pitch  Set: " + _pitch.ToString());

                Task.Run(async () =>
                {
                    await receiver.SetAnalogChannelStateAsync(1, value);
                });
            }
        }

        public double yaw
        {
            get
            {
                return _yaw;
            }
            set
            {
                _yaw = value;
                Debug.WriteLine("yaw Set: " + _yaw.ToString());

                Task.Run(async () =>
                {
                    await receiver.SetAnalogChannelStateAsync(2, value);
                });
            }
        }

        public bool arm
        {
            get
            {
                return _arm;
            }
            set
            {
                _arm = value;
                Debug.WriteLine("arm Set: " + _arm.ToString());

                Task.Run(async () =>
                {
                    await receiver.SetToggleChannelStateAsync(4, value?1u:0u);
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

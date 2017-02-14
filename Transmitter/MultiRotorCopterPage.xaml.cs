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
using Windows.Gaming.Input;
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
        }
        public RCReceiver receiver;

        double _throttle = 0;
        double _roll = 0;
        double _pitch = 0;
        double _yaw = 0;
        bool _arm = false;

        Gamepad _controller;
        DispatcherTimer _timer = new DispatcherTimer();


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

            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Tick += _timer_Tick;
            _timer.Start();
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (receiver != null)
            {
                receiver.LostEvent -= Receiver_LostEvent;
            }

            Gamepad.GamepadAdded -= Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved -= Gamepad_GamepadRemoved;
        }

        private void _timer_Tick(object sender, object e)
        {
            if (_controller == null)
            {
                return;
            }

            var reading = _controller.GetCurrentReading();

            if (reading.Buttons.HasFlag(GamepadButtons.A))
            {
                Debug.WriteLine("Toggle Arming");
                arm = !arm;
            }

            yaw = reading.LeftThumbstickX;
            if (reading.LeftThumbstickY > 0)
            {
                throttle = reading.LeftThumbstickY;
            }
            else
            {
                throttle = 0;
            }

            roll = reading.RightThumbstickX;
            pitch = reading.RightThumbstickY;
        }

        private void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            if (_controller == e)
            {
                _controller = null;
            }
        }

        private void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            if (_controller == null)
            {
                _controller = Gamepad.Gamepads.First();
            }
        }

        private void ArmButton_Click(object sender, RoutedEventArgs e)
        {
            arm = !arm;
        }

        double XFromJoystick(VirtualJoystick vj)
        {
            double x = Math.Sin(DegToRad(vj.Angle)) * vj.Distance;

            return x;
        }

        double YFromJoystick(VirtualJoystick vj)
        {
            double y = Math.Cos(DegToRad(vj.Angle)) * vj.Distance;

            return y;
        }

        private void Receiver_LostEvent(object sender, ReceiverLostReason reason)
        {
            this.Frame.GoBack();
        }

        private void VirtualJoystick_LeftStickMove(object sender, EventArgs e)
        {
            double x = XFromJoystick(JoystickLeft);
            double y = YFromJoystick(JoystickLeft);

            // x is yaw.
            // y is throttle. Centered is Zero, top is 100%.
            if (y > -double.Epsilon)
            {
                double newThrottle = y / 100.0;
                Debug.WriteLine("Throttle: " + throttle.ToString());

                throttle = newThrottle;
            }

            yaw = x;
        }

        private void VirtualJoystick_RightStickMove(object sender, EventArgs e)
        {
            double x = XFromJoystick(JoystickRight);
            double y = YFromJoystick(JoystickRight);

            roll = x;
            _pitch = y;
        }

        private void Joystick_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private double DegToRad(double angle)
        {
            return Math.PI * angle / 180.0;
        }

    }
}

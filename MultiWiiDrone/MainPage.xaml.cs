﻿using System;
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
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MultiWiiDrone
{
    internal class DroneImplementation : IDroneImplementation
    {
        MSP msp = new MSP();
        public async Task initialize()
        {
            await msp.connect();
        }

        public void setThrottle(double amount)
        {
            msp.Throttle = amount;
        }

        public void setYaw(double amount)
        {
            msp.Yaw = amount;
        }

        public void setPitch(double amount)
        {
            msp.Pitch = amount;
        }

        public void setRoll(double amount)
        {
            msp.Roll = amount;
        }

        public void setArm(bool armed)
        {
            if (armed)
            {
                msp.Arm();
            }
            else
            {
                msp.Disarm();
            }
        }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DroneRC drone = new DroneRC();

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var droneImpl = new DroneImplementation();

            drone.name = "Dorthy";
            drone.manufacturer = "ooeygui";
            drone.implementation = droneImpl;
            await droneImpl.initialize();

            drone.Start();

        }
    }
}

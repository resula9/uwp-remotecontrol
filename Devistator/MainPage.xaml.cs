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
using System.Threading.Tasks;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Devistator
{
    internal class DevistatorTankImplementation : ITankImplementation
    {
        MotorController controller = new MotorController();

        public async Task initialize()
        {
            try
            {
                await controller.initialize();


            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void setSpeed(double left, double right)
        {
            controller.setSpeedAB((float)left, (float)right);
        }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        TankRC tank = new TankRC();

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var devistator = new DevistatorTankImplementation();

            tank.name = "Devistator";
            tank.manufacturer = "DFRobot";
            tank.implementation = devistator;
            await devistator.initialize();

            tank.Start();
        }
    }
}

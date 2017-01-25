using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using RCVNuget;

namespace Transmitter
{
    class ReceiverTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, string language)
        {
            DeviceType deviceType;
            if (Enum.TryParse<DeviceType>(value.ToString(), out deviceType))
            {
                switch (deviceType)
                {
                    case DeviceType.Tank:
                        return "ms-appx:/Assets/TankReceiver.png";
                    case DeviceType.Car:
                        return "ms-appx:/Assets/CarReceiver.png";
                }
            }

            return "ms-appx:/Assets/CustomReceiver.png";
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

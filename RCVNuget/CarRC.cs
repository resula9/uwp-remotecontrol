using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCVNuget
{
    public interface ICarImplementation
    {
        void setSpeed(double throttle);
        void setSteering(double degree);
    }

    public sealed class CarRC : IRCServiceImplementation
    {
        RCService rcService = new RCService();

        double speed = 0;
        double steering = 0;

        ChannelDescriptor steeringChannel = new ChannelDescriptor() { channelId = 0, category = ChannelCategory.Steering, channelName = "Steering", type = ChannelType.Analog };
        ChannelDescriptor throttleChannel = new ChannelDescriptor() { channelId = 1, category = ChannelCategory.Throttle, channelName = "Throttle", type = ChannelType.Analog };

        public CarRC()
        {
            rcService.implementation = this;
            rcService.ChannelDescriptors.Add(steeringChannel);
            rcService.ChannelDescriptors.Add(throttleChannel);

            rcService.deviceType = DeviceType.Car;
        }

        public ICarImplementation implementation { get; set; } = null;

        // Forward name to emcapsulated service implementation
        public string name
        {
            get
            {
                return rcService.receiverName;
            }
            set
            {
                rcService.receiverName = value;
            }
        }

        // Forward name to emcapsulated service implementation
        public string manufacturer
        {
            get
            {
                return rcService.manufacturerName;
            }
            set
            {
                rcService.manufacturerName = value;
            }
        }
        public AnalogChannelInfo GetChannelInfo(uint channelId)
        {
            AnalogChannelInfo info = new AnalogChannelInfo();
            info.maximum = 1.0;
            info.minimum = -1.0;
            info.trimMaximum = 0;
            info.trimMinimum = 0;

            switch (channelId)
            {
                case 0:
                    info.value = steering;
                    break;

                case 1:
                    info.value = speed;
                    break;

                default:
                    throw new ArgumentException("Unknown Channel " + channelId.ToString());
            }

            return info;
        }

        public void SetAnalogChannelValue(uint channelId, double value)
        {
            switch (channelId)
            {
                case 0:
                    steering = value;
                    implementation.setSteering(steering);
                    break;
                case 1:
                    speed = value;
                    implementation.setSpeed(speed);
                    break;
            }

        }

        public void SetToggleChannelValue(uint channelId, uint value)
        {
            // none
        }


        public void SetMultipleChannelValues(IReadOnlyList<uint> channels, IReadOnlyList<double> values)
        {
            if (channels.Count != values.Count)
            {
                throw new ArgumentException("Values and channels have different lengths");
            }

            for(int index = 0; index < channels.Count; index++)
            {
                SetAnalogChannelValue(channels[index], values[index]);
            }
        }


        public void Start()
        {
            rcService.Start();
        }
    }
}

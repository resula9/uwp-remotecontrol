using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCVNuget
{
    public interface ITankImplementation
    {
        void setSpeed(double left, double right);
    }

    public sealed class TankRC : IRCServiceImplementation
    {
        RCService rcService = new RCService();

        double leftSpeed = 0;
        double rightSpeed = 0;

        ChannelDescriptor leftChannel = new ChannelDescriptor() { channelId = 0, category = ChannelCategory.Throttle, channelName = "Left Throttle", type = ChannelType.Analog };
        ChannelDescriptor rightChannel = new ChannelDescriptor() { channelId = 1, category = ChannelCategory.Throttle, channelName = "Right Throttle", type = ChannelType.Analog };

        public TankRC()
        {
            rcService.implementation = this;
            rcService.ChannelDescriptors.Add(leftChannel);
            rcService.ChannelDescriptors.Add(rightChannel);

            rcService.deviceType = DeviceType.Tank;
        }

        public ITankImplementation implementation { get; set; } = null;

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
                    info.value = leftSpeed;
                    break;

                case 1:
                    info.value = rightSpeed;
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
                    leftSpeed = value;
                    break;
                case 1:
                    rightSpeed = value;
                    break;
            }

            implementation.setSpeed(leftSpeed, rightSpeed);

        }

        public void SetMultipleChannelValues(IReadOnlyList<uint> channels, IReadOnlyList<double> values)
        {
            if (channels.Count != values.Count)
            {
                throw new ArgumentException("Values and channels have different lengths");
            }

            for(int index = 0; index < channels.Count; index++)
            {
                uint channelId = channels[index];
                switch (channelId)
                {
                    case 0:
                        leftSpeed = values[index];
                        break;
                    case 1:
                        rightSpeed = values[index];
                        break;
                }
            }

            implementation.setSpeed(leftSpeed, rightSpeed);
        }

        public void SetToggleChannelValue(uint channelId, uint value)
        {
            // none
        }

        public void Start()
        {
            rcService.Start();
        }
    }
}

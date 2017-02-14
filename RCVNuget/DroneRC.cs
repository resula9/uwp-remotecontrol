using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCVNuget
{
    public interface IDroneImplementation
    {
        void setThrottle(double amount);
        void setYaw(double amount);
        void setPitch(double amount);
        void setRoll(double amount);
        void setArm(bool armed);
    }

    public sealed class DroneRC : IRCServiceImplementation
    {
        RCService rcService = new RCService();

        double throttle = 0;
        double roll = 0;
        double pitch = 0;
        double yaw = 0;
        bool arm = false;

        ChannelDescriptor RollChannel = new ChannelDescriptor() { channelId = 0, category = ChannelCategory.Roll, channelName = "Roll", type = ChannelType.Analog };
        ChannelDescriptor PitchChannel = new ChannelDescriptor() { channelId = 1, category = ChannelCategory.Pitch, channelName = "Pitch", type = ChannelType.Analog };
        ChannelDescriptor YawChannel = new ChannelDescriptor() { channelId = 2, category = ChannelCategory.Yaw, channelName = "Yaw", type = ChannelType.Analog };
        ChannelDescriptor ThrottleChannel = new ChannelDescriptor() { channelId = 3, category = ChannelCategory.Throttle, channelName = "Throttle", type = ChannelType.Analog };
        ChannelDescriptor ArmChannel = new ChannelDescriptor() { channelId = 4, category = ChannelCategory.Arm, channelName = "Arm", type = ChannelType.Toggle };

        public DroneRC()
        {
            rcService.implementation = this;
            rcService.ChannelDescriptors.Add(RollChannel);
            rcService.ChannelDescriptors.Add(PitchChannel);
            rcService.ChannelDescriptors.Add(YawChannel);
            rcService.ChannelDescriptors.Add(ThrottleChannel);
            rcService.ChannelDescriptors.Add(ArmChannel);

            rcService.deviceType = DeviceType.MultiRotorCopter;
        }

        public IDroneImplementation implementation { get; set; } = null;

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
                    info.value = roll;
                    break;

                case 1:
                    info.value = pitch;
                    break;

                case 2:
                    info.value = yaw;
                    break;

                case 3:
                    info.value = throttle;
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
                    roll = value;
                    implementation.setRoll(value);
                    break;
                case 1:
                    pitch = value;
                    implementation.setPitch(value);
                    break;
                case 2:
                    yaw = value;
                    implementation.setYaw(value);
                    break;
                case 3:
                    throttle = value;
                    implementation.setThrottle(value);
                    break;
            }

        }

        public void SetToggleChannelValue(uint channelId, uint value)
        {
            switch (channelId)
            {
                case 4:
                    arm = value == 1;
                    implementation.setArm(arm);
                    break;
            }
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

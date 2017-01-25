using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.ooeygui.remotecontrolvehicle;
using Windows.Foundation;
using Windows.Devices.AllJoyn;

namespace RCVNuget
{
    public enum DeviceType : uint
    {
        Car = 0,
        Tank,
        Boat,
        Plane,
        Helicopter,
        MultiRotorCopter,
        Submarine,
        Custom,
        Unknown = uint.MaxValue
    }
    public enum ChannelType : uint
    {
        Toggle,
        Analog,
    }

    public enum ChannelCategory : uint
    {
        Throttle,
        Steering,
        Brake,
        Elevator,
        Aileron,
        Yaw,
        Pitch,
        Roll,
        Arm
    }

    public struct ChannelDescriptor
    {
        public ChannelType type;
        public ChannelCategory category;
        public uint channelId;
        public String channelName;
    }

    public struct AnalogChannelInfo
    {
        public double minimum;
        public double maximum;
        public double value;
        public double trimMinimum;
        public double trimMaximum;
    }

    public interface IRCServiceImplementation
    {
        AnalogChannelInfo GetChannelInfo(uint channelId);
        void SetAnalogChannelValue(uint channelId, double value);
        void SetToggleChannelValue(uint channelId, uint value);
        void SetMultipleChannelValues(IReadOnlyList<uint> channels, IReadOnlyList<double> values);
    }

    public sealed class RCService : IremotecontrolvehicleService
    {
        private remotecontrolvehicleProducer rcProducer = null;
        private AllJoynBusAttachment rcBusAttachment = null;
        private List<ChannelDescriptor> channelDescriptors = new List<ChannelDescriptor>();

        public IList<ChannelDescriptor> ChannelDescriptors
        {
            get
            {
                return channelDescriptors;
            }
        }

        public IRCServiceImplementation implementation { get; set; } = null;

        public string manufacturerName { get; set; } = "Default";
        public string receiverName { get; set; } = "Default";

        public DeviceType deviceType { get; set; } = DeviceType.Custom;

        public RCService()
        {
            rcBusAttachment = new AllJoynBusAttachment();
            rcProducer = new remotecontrolvehicleProducer(rcBusAttachment);
            rcProducer.Service = this;
        }

        public IAsyncOperation<remotecontrolvehicleGetChannelsResult> GetChannelsAsync(AllJoynMessageInfo info)
        {
            Task<remotecontrolvehicleGetChannelsResult> task = new Task<remotecontrolvehicleGetChannelsResult>(() =>
            {
                IList<remotecontrolvehicleChannelsItem> channelList = new List<remotecontrolvehicleChannelsItem>();
                foreach (var desc in ChannelDescriptors)
                {
                    channelList.Add(convert(desc));
                }

                return remotecontrolvehicleGetChannelsResult.CreateSuccessResult(channelList);
            });

            task.Start();
            return task.AsAsyncOperation();
        }

        public IAsyncOperation<remotecontrolvehicleGetReceiverNameResult> GetReceiverNameAsync(AllJoynMessageInfo info)
        {
            Task<remotecontrolvehicleGetReceiverNameResult> task = new Task<remotecontrolvehicleGetReceiverNameResult>(() =>
            {
                return remotecontrolvehicleGetReceiverNameResult.CreateSuccessResult(receiverName);
            });

            task.Start();
            return task.AsAsyncOperation();
        }

        // Implement this function to handle requests for the value of the Manufacturer property.
        //
        // Currently, info will always be null, because no information is available about the requestor.
        public IAsyncOperation<remotecontrolvehicleGetManufacturerResult> GetManufacturerAsync(AllJoynMessageInfo info)
        {
            Task<remotecontrolvehicleGetManufacturerResult> task = new Task<remotecontrolvehicleGetManufacturerResult>(() =>
            {
                return remotecontrolvehicleGetManufacturerResult.CreateSuccessResult(manufacturerName);
            });

            task.Start();
            return task.AsAsyncOperation();
        }

        public IAsyncOperation<remotecontrolvehicleGetDeviceTypeResult> GetDeviceTypeAsync(AllJoynMessageInfo info)
        {
            Task<remotecontrolvehicleGetDeviceTypeResult> task = new Task<remotecontrolvehicleGetDeviceTypeResult>(() =>
            {
                return remotecontrolvehicleGetDeviceTypeResult.CreateSuccessResult((uint)deviceType);
            });

            task.Start();
            return task.AsAsyncOperation();

        }
        public IAsyncOperation<remotecontrolvehicleGetAnalogChannelDataResult> GetAnalogChannelDataAsync(AllJoynMessageInfo info, uint interfaceMemberChannelId)
        {
            Task<remotecontrolvehicleGetAnalogChannelDataResult> task = new Task<remotecontrolvehicleGetAnalogChannelDataResult>(() =>
            {
                remotecontrolvehicleAnalogChannelData interfaceMemberAnalogChannelData = new remotecontrolvehicleAnalogChannelData();
                var channelInfo = implementation.GetChannelInfo(interfaceMemberChannelId);
                interfaceMemberAnalogChannelData.Value1 = channelInfo.maximum;
                interfaceMemberAnalogChannelData.Value2 = channelInfo.minimum;
                interfaceMemberAnalogChannelData.Value3 = channelInfo.value;
                interfaceMemberAnalogChannelData.Value4 = channelInfo.trimMaximum;
                interfaceMemberAnalogChannelData.Value5 = channelInfo.trimMinimum;
                return remotecontrolvehicleGetAnalogChannelDataResult.CreateSuccessResult(interfaceMemberAnalogChannelData);
            });

            task.Start();
            return task.AsAsyncOperation();
        }

        public IAsyncOperation<remotecontrolvehicleSetAnalogChannelStateResult> SetAnalogChannelStateAsync(AllJoynMessageInfo info, uint interfaceMemberChannelId, double interfaceMemberValue)
        {
            Task<remotecontrolvehicleSetAnalogChannelStateResult> task = new Task<remotecontrolvehicleSetAnalogChannelStateResult>(() =>
            {
                implementation.SetAnalogChannelValue(interfaceMemberChannelId, interfaceMemberValue);
                return remotecontrolvehicleSetAnalogChannelStateResult.CreateSuccessResult();
            });

            task.Start();
            return task.AsAsyncOperation();
        }

        public IAsyncOperation<remotecontrolvehicleSetToggleChannelStateResult> SetToggleChannelStateAsync(AllJoynMessageInfo info, uint interfaceMemberChannelId, uint interfaceMemberValue)
        {
            Task<remotecontrolvehicleSetToggleChannelStateResult> task = new Task<remotecontrolvehicleSetToggleChannelStateResult>(() =>
            {
                implementation.SetToggleChannelValue(interfaceMemberChannelId, interfaceMemberValue);
                return remotecontrolvehicleSetToggleChannelStateResult.CreateSuccessResult();
            });

            task.Start();
            return task.AsAsyncOperation();
        }

        public IAsyncOperation<remotecontrolvehicleSetMultipleAnalogChannelStatesResult> SetMultipleAnalogChannelStatesAsync(AllJoynMessageInfo info, IReadOnlyList<uint> interfaceMemberChannelIds, IReadOnlyList<double> interfaceMemberValues)
        {
            Task<remotecontrolvehicleSetMultipleAnalogChannelStatesResult> task = new Task<remotecontrolvehicleSetMultipleAnalogChannelStatesResult>(() =>
            {
                implementation.SetMultipleChannelValues(interfaceMemberChannelIds, interfaceMemberValues);
                return remotecontrolvehicleSetMultipleAnalogChannelStatesResult.CreateSuccessResult();
            });

            task.Start();
            return task.AsAsyncOperation();
        }

        public void Start()
        {
            rcProducer.Start();
        }

        private remotecontrolvehicleChannelsItem convert(ChannelDescriptor descr)
        {
            remotecontrolvehicleChannelsItem item = new remotecontrolvehicleChannelsItem();
            item.Value1 = (uint)descr.type;
            item.Value2 = (uint)descr.channelId;
            item.Value3 = (uint)descr.category;
            item.Value4 = descr.channelName;

            return item;
        }
    }
}


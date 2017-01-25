using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.ooeygui.remotecontrolvehicle;
using Windows.Devices.AllJoyn;
using Windows.Foundation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ComponentModel;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace RCVNuget
{
    public enum ReceiverLostReason
    {
        Unknown,
        Timeout,
        Removed,
        Shutdown
    };

    public delegate void ReceiverLostHandler(object sender, ReceiverLostReason reason);

    public class RCReceiver :  INotifyPropertyChanged
    {
        remotecontrolvehicleConsumer rcConsumer;
        AllJoynBusAttachment alljoynBus = new AllJoynBusAttachment();
        public event PropertyChangedEventHandler PropertyChanged;

        public event ReceiverLostHandler LostEvent;


        internal RCReceiver(remotecontrolvehicleConsumer rcc)
        {
            rcConsumer = rcc;
            rcConsumer.Session.Lost += RcConsumer_SessionLost;
            //rcConsumer.SessionMemberAdded

            Task.Run(async () =>
            {
                receiverName = await GetReceiverNameTask();
                OnPropertyChanged("receiverName");
                manufacturer = await GetManufacturerNameTask();
                OnPropertyChanged("manufacturer");
                deviceType = await GetDeviceTypeTask();
                OnPropertyChanged("deviceType");
            });
        }

        public string manufacturer { get; internal set; }
        public string receiverName { get; internal set; }
        public DeviceType deviceType { get; internal set; }

        public IAsyncOperation<string> GetReceiverNameAsync()
        {
            return GetReceiverNameTask().AsAsyncOperation<string>();
        }

        private async Task<string> GetReceiverNameTask()
        {
            var nameResult = await rcConsumer.GetReceiverNameAsync();
            if (nameResult.Status == AllJoynStatus.Ok)
            {
                return nameResult.ReceiverName;
            }

            return "";
        }
        public IAsyncOperation<string> GetManufacturerNameAsync()
        {
            return GetManufacturerNameTask().AsAsyncOperation<string>();
        }

        private async Task<string> GetManufacturerNameTask()
        { 
            var nameResult = await rcConsumer.GetManufacturerAsync();
            if (nameResult.Status == AllJoynStatus.Ok)
            {
                return nameResult.Manufacturer;
            }

            return "";
        }

        public IAsyncOperation<DeviceType> GetDeviceTypeAsync()
        {
            return GetDeviceTypeTask().AsAsyncOperation<DeviceType>();
        }

        private async Task<DeviceType> GetDeviceTypeTask()
        {
            var deviceResult = await rcConsumer.GetDeviceTypeAsync();
            if (deviceResult.Status == AllJoynStatus.Ok)
            {
                return (DeviceType)deviceResult.DeviceType;
            }

            return DeviceType.Unknown;
        }

        public IAsyncOperation<AnalogChannelInfo> GetAnalogChannelDataAsync(uint channelId)
        {
            return GetAnalogChannelDataTask(channelId).AsAsyncOperation<AnalogChannelInfo>();
        }

        private async Task<AnalogChannelInfo> GetAnalogChannelDataTask(uint channelId)
        {
            var channelResult = await rcConsumer.GetAnalogChannelDataAsync(channelId);
            if (channelResult.Status == AllJoynStatus.Ok)
            {
                var channelInfo = new AnalogChannelInfo();
                channelInfo.maximum = channelResult.AnalogChannelData.Value1;
                channelInfo.minimum = channelResult.AnalogChannelData.Value2;
                channelInfo.value = channelResult.AnalogChannelData.Value3;
                channelInfo.trimMaximum = channelResult.AnalogChannelData.Value4;
                channelInfo.trimMaximum = channelResult.AnalogChannelData.Value5;
                return channelInfo;
            }

            return new AnalogChannelInfo();
        }

        public IAsyncAction SetAnalogChannelStateAsync(uint channelId, double state)
        {
            return SetAnalogChannelStateTask(channelId, state).AsAsyncAction();
        }

        private async Task SetAnalogChannelStateTask(uint channelId, double state)
        {
            await rcConsumer.SetAnalogChannelStateAsync(channelId, state);
        }

        public IAsyncAction SetToggleChannelStateAsync(uint channelId, uint state)
        {
            return SetToggleChannelStateTask(channelId, state).AsAsyncAction();
        }

        private async Task SetToggleChannelStateTask(uint channelId, uint state)
        {
            await rcConsumer.SetToggleChannelStateAsync(channelId, state);
        }

        public IAsyncAction SetMultipleAnalogChannelStatesAsync(IReadOnlyList<uint> channelIds, IReadOnlyList<double> values)
        {
            return SetMultipleAnalogChannelStatesTask(channelIds, values).AsAsyncAction();
        }

        private async Task SetMultipleAnalogChannelStatesTask(IReadOnlyList<uint> channelIds, IReadOnlyList<double> values)
        {
            await rcConsumer.SetMultipleAnalogChannelStatesAsync(channelIds, values);
        }

        private void RcConsumer_SessionLost(AllJoynSession sender, AllJoynSessionLostEventArgs args)
        {
            ReceiverLostHandler handler = LostEvent;
            if (handler != null)
            {
                ReceiverLostReason reason = ReceiverLostReason.Unknown;
                switch (args.Reason)
                {
                    case AllJoynSessionLostReason.LinkTimeout:
                        reason = ReceiverLostReason.Timeout;
                        break;
                    case AllJoynSessionLostReason.RemovedByProducer:
                        reason = ReceiverLostReason.Removed;
                        break;
                    case AllJoynSessionLostReason.ProducerLeftSession:
                        reason = ReceiverLostReason.Shutdown;
                        break;
                    default:
                        reason = ReceiverLostReason.Unknown;
                        break;
                }
                var ignore = CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    handler(this, reason);
                });
            }
        }
        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var ignore = CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    handler(this, new PropertyChangedEventArgs(name));
                });
            }
        }
    }
}

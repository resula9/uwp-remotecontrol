using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.ooeygui.remotecontrolvehicle;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.Devices.AllJoyn;
using System.Diagnostics;
using System.ComponentModel;
using Windows.ApplicationModel.Core;
using Windows.Devices.Enumeration;

namespace RCVNuget
{
    public delegate void RCReceiverDiscovered(object finder, RCReceiver receiver);
    public delegate void RCReceiverLost(object finder, RCReceiver receiver);

    public sealed class RCReceiverFinder : INotifyPropertyChanged
    {
        AllJoynBusAttachment bus = new AllJoynBusAttachment();
        DeviceWatcher watcher;

        Dictionary<int, RCReceiver> discoveredReceivers = new Dictionary<int, RCReceiver>();

        public event PropertyChangedEventHandler PropertyChanged;

        public event RCReceiverDiscovered discovered;
        public event RCReceiverLost lost;

        public IList<RCReceiver> receivers
        {
            get
            {
                return discoveredReceivers.Values.ToList<RCReceiver>();
            }
        }

        public RCReceiverFinder()
        {
            watcher = AllJoynBusAttachment.GetWatcher(new List<string> { "com.ooeygui.remotecontrolvehicle" });
            startWatcher();
        }

        private void startWatcher()
        {
            watcher.Added += Watcher_Added;
            watcher.Start();
        }

        private void stopWatcher()
        {
            watcher.Added -= Watcher_Added;
            watcher.Stop();
        }

        private async void Watcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            try
            {
                //AllJoynAboutDataView aboutData = await bus.GetAboutDataAsync(await AllJoynServiceInfo.FromIdAsync(args.Id));

                var consumer = await remotecontrolvehicleConsumer.FromIdAsync(args.Id, bus);

                if (consumer != null)
                {
                    consumer.Session.Lost += Consumer_SessionLost;
                    var newReceiver = new RCReceiver(consumer);
                    discoveredReceivers.Add(consumer.Session.Id, newReceiver);

                    if (discovered != null)
                    {
                        RCReceiverDiscovered handler = discovered;
                        var ignore = CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            handler(this, newReceiver);
                        });

                    }

                    OnPropertyChanged("receivers");
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        private void Consumer_SessionLost(AllJoynSession sender, AllJoynSessionLostEventArgs args)
        {
            if (discoveredReceivers.ContainsKey(sender.Id))
            {
                var lostReceiver = discoveredReceivers[sender.Id];
                discoveredReceivers.Remove(sender.Id);

                if (discovered != null)
                {
                    RCReceiverLost handler = lost;
                    var ignore = CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        handler(this, lostReceiver);
                    });
                }
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

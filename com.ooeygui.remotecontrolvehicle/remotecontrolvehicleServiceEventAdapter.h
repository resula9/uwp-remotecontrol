//-----------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//   Changes to this file may cause incorrect behavior and will be lost if
//   the code is regenerated.
//
//   For more information, see: http://go.microsoft.com/fwlink/?LinkID=623246
// </auto-generated>
//-----------------------------------------------------------------------------
#pragma once

namespace com { namespace ooeygui { namespace remotecontrolvehicle {

// This class, and the associated EventArgs classes, exist for the benefit of JavaScript developers who
// do not have the ability to implement IremotecontrolvehicleService. Instead, remotecontrolvehicleServiceEventAdapter
// provides the Interface implementation and exposes a set of compatible events to the developer.
public ref class remotecontrolvehicleServiceEventAdapter sealed : [Windows::Foundation::Metadata::Default] IremotecontrolvehicleService
{
public:
    // Method Invocation Events
    event Windows::Foundation::TypedEventHandler<remotecontrolvehicleServiceEventAdapter^, remotecontrolvehicleGetAnalogChannelDataCalledEventArgs^>^ GetAnalogChannelDataCalled 
    { 
        Windows::Foundation::EventRegistrationToken add(Windows::Foundation::TypedEventHandler<remotecontrolvehicleServiceEventAdapter^, remotecontrolvehicleGetAnalogChannelDataCalledEventArgs^>^ handler) 
        { 
            return _GetAnalogChannelDataCalled += ref new Windows::Foundation::EventHandler<Platform::Object^>
            ([handler](Platform::Object^ sender, Platform::Object^ args)
            {
                handler->Invoke(safe_cast<remotecontrolvehicleServiceEventAdapter^>(sender), safe_cast<remotecontrolvehicleGetAnalogChannelDataCalledEventArgs^>(args));
            }, Platform::CallbackContext::Same);
        } 
        void remove(Windows::Foundation::EventRegistrationToken token) 
        { 
            _GetAnalogChannelDataCalled -= token; 
        } 
    internal: 
        void raise(remotecontrolvehicleServiceEventAdapter^ sender, remotecontrolvehicleGetAnalogChannelDataCalledEventArgs^ args) 
        { 
            _GetAnalogChannelDataCalled(sender, args);
        } 
    }

    event Windows::Foundation::TypedEventHandler<remotecontrolvehicleServiceEventAdapter^, remotecontrolvehicleSetAnalogChannelStateCalledEventArgs^>^ SetAnalogChannelStateCalled 
    { 
        Windows::Foundation::EventRegistrationToken add(Windows::Foundation::TypedEventHandler<remotecontrolvehicleServiceEventAdapter^, remotecontrolvehicleSetAnalogChannelStateCalledEventArgs^>^ handler) 
        { 
            return _SetAnalogChannelStateCalled += ref new Windows::Foundation::EventHandler<Platform::Object^>
            ([handler](Platform::Object^ sender, Platform::Object^ args)
            {
                handler->Invoke(safe_cast<remotecontrolvehicleServiceEventAdapter^>(sender), safe_cast<remotecontrolvehicleSetAnalogChannelStateCalledEventArgs^>(args));
            }, Platform::CallbackContext::Same);
        } 
        void remove(Windows::Foundation::EventRegistrationToken token) 
        { 
            _SetAnalogChannelStateCalled -= token; 
        } 
    internal: 
        void raise(remotecontrolvehicleServiceEventAdapter^ sender, remotecontrolvehicleSetAnalogChannelStateCalledEventArgs^ args) 
        { 
            _SetAnalogChannelStateCalled(sender, args);
        } 
    }

    event Windows::Foundation::TypedEventHandler<remotecontrolvehicleServiceEventAdapter^, remotecontrolvehicleSetToggleChannelStateCalledEventArgs^>^ SetToggleChannelStateCalled 
    { 
        Windows::Foundation::EventRegistrationToken add(Windows::Foundation::TypedEventHandler<remotecontrolvehicleServiceEventAdapter^, remotecontrolvehicleSetToggleChannelStateCalledEventArgs^>^ handler) 
        { 
            return _SetToggleChannelStateCalled += ref new Windows::Foundation::EventHandler<Platform::Object^>
            ([handler](Platform::Object^ sender, Platform::Object^ args)
            {
                handler->Invoke(safe_cast<remotecontrolvehicleServiceEventAdapter^>(sender), safe_cast<remotecontrolvehicleSetToggleChannelStateCalledEventArgs^>(args));
            }, Platform::CallbackContext::Same);
        } 
        void remove(Windows::Foundation::EventRegistrationToken token) 
        { 
            _SetToggleChannelStateCalled -= token; 
        } 
    internal: 
        void raise(remotecontrolvehicleServiceEventAdapter^ sender, remotecontrolvehicleSetToggleChannelStateCalledEventArgs^ args) 
        { 
            _SetToggleChannelStateCalled(sender, args);
        } 
    }

    event Windows::Foundation::TypedEventHandler<remotecontrolvehicleServiceEventAdapter^, remotecontrolvehicleSetMultipleAnalogChannelStatesCalledEventArgs^>^ SetMultipleAnalogChannelStatesCalled 
    { 
        Windows::Foundation::EventRegistrationToken add(Windows::Foundation::TypedEventHandler<remotecontrolvehicleServiceEventAdapter^, remotecontrolvehicleSetMultipleAnalogChannelStatesCalledEventArgs^>^ handler) 
        { 
            return _SetMultipleAnalogChannelStatesCalled += ref new Windows::Foundation::EventHandler<Platform::Object^>
            ([handler](Platform::Object^ sender, Platform::Object^ args)
            {
                handler->Invoke(safe_cast<remotecontrolvehicleServiceEventAdapter^>(sender), safe_cast<remotecontrolvehicleSetMultipleAnalogChannelStatesCalledEventArgs^>(args));
            }, Platform::CallbackContext::Same);
        } 
        void remove(Windows::Foundation::EventRegistrationToken token) 
        { 
            _SetMultipleAnalogChannelStatesCalled -= token; 
        } 
    internal: 
        void raise(remotecontrolvehicleServiceEventAdapter^ sender, remotecontrolvehicleSetMultipleAnalogChannelStatesCalledEventArgs^ args) 
        { 
            _SetMultipleAnalogChannelStatesCalled(sender, args);
        } 
    }

    // Property Read Events
    event Windows::Foundation::TypedEventHandler<remotecontrolvehicleServiceEventAdapter^, remotecontrolvehicleGetReceiverNameRequestedEventArgs^>^ GetReceiverNameRequested 
    { 
        Windows::Foundation::EventRegistrationToken add(Windows::Foundation::TypedEventHandler<remotecontrolvehicleServiceEventAdapter^, remotecontrolvehicleGetReceiverNameRequestedEventArgs^>^ handler) 
        { 
            return _GetReceiverNameRequested += ref new Windows::Foundation::EventHandler<Platform::Object^>
            ([handler](Platform::Object^ sender, Platform::Object^ args)
            {
                handler->Invoke(safe_cast<remotecontrolvehicleServiceEventAdapter^>(sender), safe_cast<remotecontrolvehicleGetReceiverNameRequestedEventArgs^>(args));
            }, Platform::CallbackContext::Same);
        } 
        void remove(Windows::Foundation::EventRegistrationToken token) 
        { 
            _GetReceiverNameRequested -= token; 
        } 
    internal: 
        void raise(remotecontrolvehicleServiceEventAdapter^ sender, remotecontrolvehicleGetReceiverNameRequestedEventArgs^ args) 
        { 
            _GetReceiverNameRequested(sender, args);
        } 
    }

    event Windows::Foundation::TypedEventHandler<remotecontrolvehicleServiceEventAdapter^, remotecontrolvehicleGetManufacturerRequestedEventArgs^>^ GetManufacturerRequested 
    { 
        Windows::Foundation::EventRegistrationToken add(Windows::Foundation::TypedEventHandler<remotecontrolvehicleServiceEventAdapter^, remotecontrolvehicleGetManufacturerRequestedEventArgs^>^ handler) 
        { 
            return _GetManufacturerRequested += ref new Windows::Foundation::EventHandler<Platform::Object^>
            ([handler](Platform::Object^ sender, Platform::Object^ args)
            {
                handler->Invoke(safe_cast<remotecontrolvehicleServiceEventAdapter^>(sender), safe_cast<remotecontrolvehicleGetManufacturerRequestedEventArgs^>(args));
            }, Platform::CallbackContext::Same);
        } 
        void remove(Windows::Foundation::EventRegistrationToken token) 
        { 
            _GetManufacturerRequested -= token; 
        } 
    internal: 
        void raise(remotecontrolvehicleServiceEventAdapter^ sender, remotecontrolvehicleGetManufacturerRequestedEventArgs^ args) 
        { 
            _GetManufacturerRequested(sender, args);
        } 
    }

    event Windows::Foundation::TypedEventHandler<remotecontrolvehicleServiceEventAdapter^, remotecontrolvehicleGetDeviceTypeRequestedEventArgs^>^ GetDeviceTypeRequested 
    { 
        Windows::Foundation::EventRegistrationToken add(Windows::Foundation::TypedEventHandler<remotecontrolvehicleServiceEventAdapter^, remotecontrolvehicleGetDeviceTypeRequestedEventArgs^>^ handler) 
        { 
            return _GetDeviceTypeRequested += ref new Windows::Foundation::EventHandler<Platform::Object^>
            ([handler](Platform::Object^ sender, Platform::Object^ args)
            {
                handler->Invoke(safe_cast<remotecontrolvehicleServiceEventAdapter^>(sender), safe_cast<remotecontrolvehicleGetDeviceTypeRequestedEventArgs^>(args));
            }, Platform::CallbackContext::Same);
        } 
        void remove(Windows::Foundation::EventRegistrationToken token) 
        { 
            _GetDeviceTypeRequested -= token; 
        } 
    internal: 
        void raise(remotecontrolvehicleServiceEventAdapter^ sender, remotecontrolvehicleGetDeviceTypeRequestedEventArgs^ args) 
        { 
            _GetDeviceTypeRequested(sender, args);
        } 
    }

    event Windows::Foundation::TypedEventHandler<remotecontrolvehicleServiceEventAdapter^, remotecontrolvehicleGetChannelsRequestedEventArgs^>^ GetChannelsRequested 
    { 
        Windows::Foundation::EventRegistrationToken add(Windows::Foundation::TypedEventHandler<remotecontrolvehicleServiceEventAdapter^, remotecontrolvehicleGetChannelsRequestedEventArgs^>^ handler) 
        { 
            return _GetChannelsRequested += ref new Windows::Foundation::EventHandler<Platform::Object^>
            ([handler](Platform::Object^ sender, Platform::Object^ args)
            {
                handler->Invoke(safe_cast<remotecontrolvehicleServiceEventAdapter^>(sender), safe_cast<remotecontrolvehicleGetChannelsRequestedEventArgs^>(args));
            }, Platform::CallbackContext::Same);
        } 
        void remove(Windows::Foundation::EventRegistrationToken token) 
        { 
            _GetChannelsRequested -= token; 
        } 
    internal: 
        void raise(remotecontrolvehicleServiceEventAdapter^ sender, remotecontrolvehicleGetChannelsRequestedEventArgs^ args) 
        { 
            _GetChannelsRequested(sender, args);
        } 
    }

    // Property Write Events
    // IremotecontrolvehicleService Implementation
    virtual Windows::Foundation::IAsyncOperation<remotecontrolvehicleGetAnalogChannelDataResult^>^ GetAnalogChannelDataAsync(_In_ Windows::Devices::AllJoyn::AllJoynMessageInfo^ info, _In_ uint32 interfaceMemberChannelId);
    virtual Windows::Foundation::IAsyncOperation<remotecontrolvehicleSetAnalogChannelStateResult^>^ SetAnalogChannelStateAsync(_In_ Windows::Devices::AllJoyn::AllJoynMessageInfo^ info, _In_ uint32 interfaceMemberChannelId, _In_ double interfaceMemberValue);
    virtual Windows::Foundation::IAsyncOperation<remotecontrolvehicleSetToggleChannelStateResult^>^ SetToggleChannelStateAsync(_In_ Windows::Devices::AllJoyn::AllJoynMessageInfo^ info, _In_ uint32 interfaceMemberChannelId, _In_ uint32 interfaceMemberValue);
    virtual Windows::Foundation::IAsyncOperation<remotecontrolvehicleSetMultipleAnalogChannelStatesResult^>^ SetMultipleAnalogChannelStatesAsync(_In_ Windows::Devices::AllJoyn::AllJoynMessageInfo^ info, _In_ Windows::Foundation::Collections::IVectorView<uint32>^ interfaceMemberChannelIds, _In_ Windows::Foundation::Collections::IVectorView<double>^ interfaceMemberValues);

    virtual Windows::Foundation::IAsyncOperation<remotecontrolvehicleGetReceiverNameResult^>^ GetReceiverNameAsync(_In_ Windows::Devices::AllJoyn::AllJoynMessageInfo^ info);
    virtual Windows::Foundation::IAsyncOperation<remotecontrolvehicleGetManufacturerResult^>^ GetManufacturerAsync(_In_ Windows::Devices::AllJoyn::AllJoynMessageInfo^ info);
    virtual Windows::Foundation::IAsyncOperation<remotecontrolvehicleGetDeviceTypeResult^>^ GetDeviceTypeAsync(_In_ Windows::Devices::AllJoyn::AllJoynMessageInfo^ info);
    virtual Windows::Foundation::IAsyncOperation<remotecontrolvehicleGetChannelsResult^>^ GetChannelsAsync(_In_ Windows::Devices::AllJoyn::AllJoynMessageInfo^ info);


private:
    event Windows::Foundation::EventHandler<Platform::Object^>^ _GetAnalogChannelDataCalled;
    event Windows::Foundation::EventHandler<Platform::Object^>^ _SetAnalogChannelStateCalled;
    event Windows::Foundation::EventHandler<Platform::Object^>^ _SetToggleChannelStateCalled;
    event Windows::Foundation::EventHandler<Platform::Object^>^ _SetMultipleAnalogChannelStatesCalled;
    event Windows::Foundation::EventHandler<Platform::Object^>^ _GetReceiverNameRequested;
    event Windows::Foundation::EventHandler<Platform::Object^>^ _GetManufacturerRequested;
    event Windows::Foundation::EventHandler<Platform::Object^>^ _GetDeviceTypeRequested;
    event Windows::Foundation::EventHandler<Platform::Object^>^ _GetChannelsRequested;
};

} } } 

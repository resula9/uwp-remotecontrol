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

// Methods
public ref class remotecontrolvehicleGetAnalogChannelDataCalledEventArgs sealed
{
public:
    remotecontrolvehicleGetAnalogChannelDataCalledEventArgs(_In_ Windows::Devices::AllJoyn::AllJoynMessageInfo^ info, _In_ uint32 interfaceMemberChannelId);

    property Windows::Devices::AllJoyn::AllJoynMessageInfo^ MessageInfo
    {
        Windows::Devices::AllJoyn::AllJoynMessageInfo^ get() { return m_messageInfo; }
    }

    property remotecontrolvehicleGetAnalogChannelDataResult^ Result
    {
        remotecontrolvehicleGetAnalogChannelDataResult^ get() { return m_result; }
        void set(_In_ remotecontrolvehicleGetAnalogChannelDataResult^ value) { m_result = value; }
    }

    property uint32 ChannelId
    {
        uint32 get() { return m_interfaceMemberChannelId; }
    }

    Windows::Foundation::Deferral^ GetDeferral();

    static Windows::Foundation::IAsyncOperation<remotecontrolvehicleGetAnalogChannelDataResult^>^ GetResultAsync(remotecontrolvehicleGetAnalogChannelDataCalledEventArgs^ args)
    {
        args->InvokeAllFinished();
        auto t = concurrency::create_task(args->m_tce);
        return concurrency::create_async([t]() -> concurrency::task<remotecontrolvehicleGetAnalogChannelDataResult^>
        {
            return t;
        });
    }
    
private:
    void Complete();
    void InvokeAllFinished();
    void InvokeCompleteHandler();

    bool m_raised;
    int m_completionsRequired;
    concurrency::task_completion_event<remotecontrolvehicleGetAnalogChannelDataResult^> m_tce;
    std::mutex m_lock;
    Windows::Devices::AllJoyn::AllJoynMessageInfo^ m_messageInfo;
    remotecontrolvehicleGetAnalogChannelDataResult^ m_result;
    uint32 m_interfaceMemberChannelId;
};

public ref class remotecontrolvehicleSetAnalogChannelStateCalledEventArgs sealed
{
public:
    remotecontrolvehicleSetAnalogChannelStateCalledEventArgs(_In_ Windows::Devices::AllJoyn::AllJoynMessageInfo^ info, _In_ uint32 interfaceMemberChannelId, _In_ double interfaceMemberValue);

    property Windows::Devices::AllJoyn::AllJoynMessageInfo^ MessageInfo
    {
        Windows::Devices::AllJoyn::AllJoynMessageInfo^ get() { return m_messageInfo; }
    }

    property remotecontrolvehicleSetAnalogChannelStateResult^ Result
    {
        remotecontrolvehicleSetAnalogChannelStateResult^ get() { return m_result; }
        void set(_In_ remotecontrolvehicleSetAnalogChannelStateResult^ value) { m_result = value; }
    }

    property uint32 ChannelId
    {
        uint32 get() { return m_interfaceMemberChannelId; }
    }

    property double Value
    {
        double get() { return m_interfaceMemberValue; }
    }

    Windows::Foundation::Deferral^ GetDeferral();

    static Windows::Foundation::IAsyncOperation<remotecontrolvehicleSetAnalogChannelStateResult^>^ GetResultAsync(remotecontrolvehicleSetAnalogChannelStateCalledEventArgs^ args)
    {
        args->InvokeAllFinished();
        auto t = concurrency::create_task(args->m_tce);
        return concurrency::create_async([t]() -> concurrency::task<remotecontrolvehicleSetAnalogChannelStateResult^>
        {
            return t;
        });
    }
    
private:
    void Complete();
    void InvokeAllFinished();
    void InvokeCompleteHandler();

    bool m_raised;
    int m_completionsRequired;
    concurrency::task_completion_event<remotecontrolvehicleSetAnalogChannelStateResult^> m_tce;
    std::mutex m_lock;
    Windows::Devices::AllJoyn::AllJoynMessageInfo^ m_messageInfo;
    remotecontrolvehicleSetAnalogChannelStateResult^ m_result;
    uint32 m_interfaceMemberChannelId;
    double m_interfaceMemberValue;
};

public ref class remotecontrolvehicleSetToggleChannelStateCalledEventArgs sealed
{
public:
    remotecontrolvehicleSetToggleChannelStateCalledEventArgs(_In_ Windows::Devices::AllJoyn::AllJoynMessageInfo^ info, _In_ uint32 interfaceMemberChannelId, _In_ uint32 interfaceMemberValue);

    property Windows::Devices::AllJoyn::AllJoynMessageInfo^ MessageInfo
    {
        Windows::Devices::AllJoyn::AllJoynMessageInfo^ get() { return m_messageInfo; }
    }

    property remotecontrolvehicleSetToggleChannelStateResult^ Result
    {
        remotecontrolvehicleSetToggleChannelStateResult^ get() { return m_result; }
        void set(_In_ remotecontrolvehicleSetToggleChannelStateResult^ value) { m_result = value; }
    }

    property uint32 ChannelId
    {
        uint32 get() { return m_interfaceMemberChannelId; }
    }

    property uint32 Value
    {
        uint32 get() { return m_interfaceMemberValue; }
    }

    Windows::Foundation::Deferral^ GetDeferral();

    static Windows::Foundation::IAsyncOperation<remotecontrolvehicleSetToggleChannelStateResult^>^ GetResultAsync(remotecontrolvehicleSetToggleChannelStateCalledEventArgs^ args)
    {
        args->InvokeAllFinished();
        auto t = concurrency::create_task(args->m_tce);
        return concurrency::create_async([t]() -> concurrency::task<remotecontrolvehicleSetToggleChannelStateResult^>
        {
            return t;
        });
    }
    
private:
    void Complete();
    void InvokeAllFinished();
    void InvokeCompleteHandler();

    bool m_raised;
    int m_completionsRequired;
    concurrency::task_completion_event<remotecontrolvehicleSetToggleChannelStateResult^> m_tce;
    std::mutex m_lock;
    Windows::Devices::AllJoyn::AllJoynMessageInfo^ m_messageInfo;
    remotecontrolvehicleSetToggleChannelStateResult^ m_result;
    uint32 m_interfaceMemberChannelId;
    uint32 m_interfaceMemberValue;
};

public ref class remotecontrolvehicleSetMultipleAnalogChannelStatesCalledEventArgs sealed
{
public:
    remotecontrolvehicleSetMultipleAnalogChannelStatesCalledEventArgs(_In_ Windows::Devices::AllJoyn::AllJoynMessageInfo^ info, _In_ Windows::Foundation::Collections::IVectorView<uint32>^ interfaceMemberChannelIds, _In_ Windows::Foundation::Collections::IVectorView<double>^ interfaceMemberValues);

    property Windows::Devices::AllJoyn::AllJoynMessageInfo^ MessageInfo
    {
        Windows::Devices::AllJoyn::AllJoynMessageInfo^ get() { return m_messageInfo; }
    }

    property remotecontrolvehicleSetMultipleAnalogChannelStatesResult^ Result
    {
        remotecontrolvehicleSetMultipleAnalogChannelStatesResult^ get() { return m_result; }
        void set(_In_ remotecontrolvehicleSetMultipleAnalogChannelStatesResult^ value) { m_result = value; }
    }

    property Windows::Foundation::Collections::IVectorView<uint32>^ ChannelIds
    {
        Windows::Foundation::Collections::IVectorView<uint32>^ get() { return m_interfaceMemberChannelIds; }
    }

    property Windows::Foundation::Collections::IVectorView<double>^ Values
    {
        Windows::Foundation::Collections::IVectorView<double>^ get() { return m_interfaceMemberValues; }
    }

    Windows::Foundation::Deferral^ GetDeferral();

    static Windows::Foundation::IAsyncOperation<remotecontrolvehicleSetMultipleAnalogChannelStatesResult^>^ GetResultAsync(remotecontrolvehicleSetMultipleAnalogChannelStatesCalledEventArgs^ args)
    {
        args->InvokeAllFinished();
        auto t = concurrency::create_task(args->m_tce);
        return concurrency::create_async([t]() -> concurrency::task<remotecontrolvehicleSetMultipleAnalogChannelStatesResult^>
        {
            return t;
        });
    }
    
private:
    void Complete();
    void InvokeAllFinished();
    void InvokeCompleteHandler();

    bool m_raised;
    int m_completionsRequired;
    concurrency::task_completion_event<remotecontrolvehicleSetMultipleAnalogChannelStatesResult^> m_tce;
    std::mutex m_lock;
    Windows::Devices::AllJoyn::AllJoynMessageInfo^ m_messageInfo;
    remotecontrolvehicleSetMultipleAnalogChannelStatesResult^ m_result;
    Windows::Foundation::Collections::IVectorView<uint32>^ m_interfaceMemberChannelIds;
    Windows::Foundation::Collections::IVectorView<double>^ m_interfaceMemberValues;
};

// Readable Properties
public ref class remotecontrolvehicleGetReceiverNameRequestedEventArgs sealed
{
public:
    remotecontrolvehicleGetReceiverNameRequestedEventArgs(_In_ Windows::Devices::AllJoyn::AllJoynMessageInfo^ info);

    property Windows::Devices::AllJoyn::AllJoynMessageInfo^ MessageInfo
    {
        Windows::Devices::AllJoyn::AllJoynMessageInfo^ get() { return m_messageInfo; }
    }

    property remotecontrolvehicleGetReceiverNameResult^ Result
    {
        remotecontrolvehicleGetReceiverNameResult^ get() { return m_result; }
        void set(_In_ remotecontrolvehicleGetReceiverNameResult^ value) { m_result = value; }
    }

    Windows::Foundation::Deferral^ GetDeferral();

    static Windows::Foundation::IAsyncOperation<remotecontrolvehicleGetReceiverNameResult^>^ GetResultAsync(remotecontrolvehicleGetReceiverNameRequestedEventArgs^ args)
    {
        args->InvokeAllFinished();
        auto t = concurrency::create_task(args->m_tce);
        return concurrency::create_async([t]() -> concurrency::task<remotecontrolvehicleGetReceiverNameResult^>
        {
            return t;
        });
    }

private:
    void Complete();
    void InvokeAllFinished();
    void InvokeCompleteHandler();

    bool m_raised;
    int m_completionsRequired;
    concurrency::task_completion_event<remotecontrolvehicleGetReceiverNameResult^> m_tce;
    std::mutex m_lock;
    Windows::Devices::AllJoyn::AllJoynMessageInfo^ m_messageInfo;
    remotecontrolvehicleGetReceiverNameResult^ m_result;
};

public ref class remotecontrolvehicleGetManufacturerRequestedEventArgs sealed
{
public:
    remotecontrolvehicleGetManufacturerRequestedEventArgs(_In_ Windows::Devices::AllJoyn::AllJoynMessageInfo^ info);

    property Windows::Devices::AllJoyn::AllJoynMessageInfo^ MessageInfo
    {
        Windows::Devices::AllJoyn::AllJoynMessageInfo^ get() { return m_messageInfo; }
    }

    property remotecontrolvehicleGetManufacturerResult^ Result
    {
        remotecontrolvehicleGetManufacturerResult^ get() { return m_result; }
        void set(_In_ remotecontrolvehicleGetManufacturerResult^ value) { m_result = value; }
    }

    Windows::Foundation::Deferral^ GetDeferral();

    static Windows::Foundation::IAsyncOperation<remotecontrolvehicleGetManufacturerResult^>^ GetResultAsync(remotecontrolvehicleGetManufacturerRequestedEventArgs^ args)
    {
        args->InvokeAllFinished();
        auto t = concurrency::create_task(args->m_tce);
        return concurrency::create_async([t]() -> concurrency::task<remotecontrolvehicleGetManufacturerResult^>
        {
            return t;
        });
    }

private:
    void Complete();
    void InvokeAllFinished();
    void InvokeCompleteHandler();

    bool m_raised;
    int m_completionsRequired;
    concurrency::task_completion_event<remotecontrolvehicleGetManufacturerResult^> m_tce;
    std::mutex m_lock;
    Windows::Devices::AllJoyn::AllJoynMessageInfo^ m_messageInfo;
    remotecontrolvehicleGetManufacturerResult^ m_result;
};

public ref class remotecontrolvehicleGetDeviceTypeRequestedEventArgs sealed
{
public:
    remotecontrolvehicleGetDeviceTypeRequestedEventArgs(_In_ Windows::Devices::AllJoyn::AllJoynMessageInfo^ info);

    property Windows::Devices::AllJoyn::AllJoynMessageInfo^ MessageInfo
    {
        Windows::Devices::AllJoyn::AllJoynMessageInfo^ get() { return m_messageInfo; }
    }

    property remotecontrolvehicleGetDeviceTypeResult^ Result
    {
        remotecontrolvehicleGetDeviceTypeResult^ get() { return m_result; }
        void set(_In_ remotecontrolvehicleGetDeviceTypeResult^ value) { m_result = value; }
    }

    Windows::Foundation::Deferral^ GetDeferral();

    static Windows::Foundation::IAsyncOperation<remotecontrolvehicleGetDeviceTypeResult^>^ GetResultAsync(remotecontrolvehicleGetDeviceTypeRequestedEventArgs^ args)
    {
        args->InvokeAllFinished();
        auto t = concurrency::create_task(args->m_tce);
        return concurrency::create_async([t]() -> concurrency::task<remotecontrolvehicleGetDeviceTypeResult^>
        {
            return t;
        });
    }

private:
    void Complete();
    void InvokeAllFinished();
    void InvokeCompleteHandler();

    bool m_raised;
    int m_completionsRequired;
    concurrency::task_completion_event<remotecontrolvehicleGetDeviceTypeResult^> m_tce;
    std::mutex m_lock;
    Windows::Devices::AllJoyn::AllJoynMessageInfo^ m_messageInfo;
    remotecontrolvehicleGetDeviceTypeResult^ m_result;
};

public ref class remotecontrolvehicleGetChannelsRequestedEventArgs sealed
{
public:
    remotecontrolvehicleGetChannelsRequestedEventArgs(_In_ Windows::Devices::AllJoyn::AllJoynMessageInfo^ info);

    property Windows::Devices::AllJoyn::AllJoynMessageInfo^ MessageInfo
    {
        Windows::Devices::AllJoyn::AllJoynMessageInfo^ get() { return m_messageInfo; }
    }

    property remotecontrolvehicleGetChannelsResult^ Result
    {
        remotecontrolvehicleGetChannelsResult^ get() { return m_result; }
        void set(_In_ remotecontrolvehicleGetChannelsResult^ value) { m_result = value; }
    }

    Windows::Foundation::Deferral^ GetDeferral();

    static Windows::Foundation::IAsyncOperation<remotecontrolvehicleGetChannelsResult^>^ GetResultAsync(remotecontrolvehicleGetChannelsRequestedEventArgs^ args)
    {
        args->InvokeAllFinished();
        auto t = concurrency::create_task(args->m_tce);
        return concurrency::create_async([t]() -> concurrency::task<remotecontrolvehicleGetChannelsResult^>
        {
            return t;
        });
    }

private:
    void Complete();
    void InvokeAllFinished();
    void InvokeCompleteHandler();

    bool m_raised;
    int m_completionsRequired;
    concurrency::task_completion_event<remotecontrolvehicleGetChannelsResult^> m_tce;
    std::mutex m_lock;
    Windows::Devices::AllJoyn::AllJoynMessageInfo^ m_messageInfo;
    remotecontrolvehicleGetChannelsResult^ m_result;
};

// Writable Properties
} } } 
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
#include "pch.h"

using namespace concurrency;
using namespace Microsoft::WRL;
using namespace Platform;
using namespace Windows::Foundation;
using namespace Windows::Devices::AllJoyn;
using namespace com::ooeygui::remotecontrolvehicle;

std::map<alljoyn_busobject, WeakReference*> remotecontrolvehicleProducer::SourceObjects;
std::map<alljoyn_interfacedescription, WeakReference*> remotecontrolvehicleProducer::SourceInterfaces;

remotecontrolvehicleProducer::remotecontrolvehicleProducer(AllJoynBusAttachment^ busAttachment)
    : m_busAttachment(busAttachment),
    m_sessionListener(nullptr),
    m_busObject(nullptr),
    m_sessionPort(0),
    m_sessionId(0)
{
    m_weak = new WeakReference(this);
    ServiceObjectPath = ref new String(L"/Service");
    m_signals = ref new remotecontrolvehicleLegacySignals();
    m_busAttachmentStateChangedToken.Value = 0;
}

remotecontrolvehicleProducer::~remotecontrolvehicleProducer()
{
    UnregisterFromBus();
    delete m_weak;
}

void remotecontrolvehicleProducer::UnregisterFromBus()
{
    if ((nullptr != m_busAttachment) && (0 != m_busAttachmentStateChangedToken.Value))
    {
        m_busAttachment->StateChanged -= m_busAttachmentStateChangedToken;
        m_busAttachmentStateChangedToken.Value = 0;
    }
    if ((nullptr != m_busAttachment) && (nullptr != SessionPortListener))
    {
        alljoyn_busattachment_unbindsessionport(AllJoynHelpers::GetInternalBusAttachment(m_busAttachment), m_sessionPort);
        alljoyn_sessionportlistener_destroy(SessionPortListener);
        SessionPortListener = nullptr;
    }
    if ((nullptr != m_busAttachment) && (nullptr != BusObject))
    {
        alljoyn_busattachment_unregisterbusobject(AllJoynHelpers::GetInternalBusAttachment(m_busAttachment), BusObject);
        alljoyn_busobject_destroy(BusObject);
        BusObject = nullptr;
    }
    if ((nullptr != m_busAttachment) && (nullptr != SessionListener))
    {
        alljoyn_busattachment_leavesession(AllJoynHelpers::GetInternalBusAttachment(m_busAttachment), m_sessionId);
        alljoyn_busattachment_setsessionlistener(AllJoynHelpers::GetInternalBusAttachment(m_busAttachment), m_sessionId, nullptr);
        alljoyn_sessionlistener_destroy(SessionListener);
        SessionListener = nullptr;
    }
}

bool remotecontrolvehicleProducer::OnAcceptSessionJoiner(_In_ alljoyn_sessionport sessionPort, _In_ PCSTR joiner, _In_ const alljoyn_sessionopts opts)
{
    UNREFERENCED_PARAMETER(sessionPort); UNREFERENCED_PARAMETER(joiner); UNREFERENCED_PARAMETER(opts);
    
    return true;
}

void remotecontrolvehicleProducer::OnSessionJoined(_In_ alljoyn_sessionport sessionPort, _In_ alljoyn_sessionid id, _In_ PCSTR joiner)
{
    UNREFERENCED_PARAMETER(joiner);

    // We initialize the Signals object after the session has been joined, because it needs
    // the session id.
    m_signals->Initialize(BusObject, id);
    m_sessionPort = sessionPort;
    m_sessionId = id;

    alljoyn_sessionlistener_callbacks callbacks =
    {
        AllJoynHelpers::SessionLostHandler<remotecontrolvehicleProducer>,
        AllJoynHelpers::SessionMemberAddedHandler<remotecontrolvehicleProducer>,
        AllJoynHelpers::SessionMemberRemovedHandler<remotecontrolvehicleProducer>
    };

    SessionListener = alljoyn_sessionlistener_create(&callbacks, m_weak);
    alljoyn_busattachment_setsessionlistener(AllJoynHelpers::GetInternalBusAttachment(m_busAttachment), id, SessionListener);
}

void remotecontrolvehicleProducer::OnSessionLost(_In_ alljoyn_sessionid sessionId, _In_ alljoyn_sessionlostreason reason)
{
    if (sessionId == m_sessionId)
    {
        AllJoynSessionLostEventArgs^ args = ref new AllJoynSessionLostEventArgs(static_cast<AllJoynSessionLostReason>(reason));
        SessionLost(this, args);
    }
}

void remotecontrolvehicleProducer::OnSessionMemberAdded(_In_ alljoyn_sessionid sessionId, _In_ PCSTR uniqueName)
{
    if (sessionId == m_sessionId)
    {
        auto args = ref new AllJoynSessionMemberAddedEventArgs(AllJoynHelpers::MultibyteToPlatformString(uniqueName));
        SessionMemberAdded(this, args);
    }
}

void remotecontrolvehicleProducer::OnSessionMemberRemoved(_In_ alljoyn_sessionid sessionId, _In_ PCSTR uniqueName)
{
    if (sessionId == m_sessionId)
    {
        auto args = ref new AllJoynSessionMemberRemovedEventArgs(AllJoynHelpers::MultibyteToPlatformString(uniqueName));
        SessionMemberRemoved(this, args);
    }
}

void remotecontrolvehicleProducer::BusAttachmentStateChanged(_In_ AllJoynBusAttachment^ sender, _In_ AllJoynBusAttachmentStateChangedEventArgs^ args)
{
    if (args->State == AllJoynBusAttachmentState::Connected)
    {   
        QStatus result = AllJoynHelpers::CreateProducerSession<remotecontrolvehicleProducer>(m_busAttachment, m_weak);
        if (ER_OK != result)
        {
            StopInternal(result);
            return;
        }
    }
    else if (args->State == AllJoynBusAttachmentState::Disconnected)
    {
        StopInternal(ER_BUS_STOPPING);
    }
}

void remotecontrolvehicleProducer::CallGetAnalogChannelDataHandler(_Inout_ alljoyn_busobject busObject, _In_ alljoyn_message message)
{
    auto source = SourceObjects.find(busObject);
    if (source == SourceObjects.end())
    {
        return;
    }

    remotecontrolvehicleProducer^ producer = source->second->Resolve<remotecontrolvehicleProducer>();
    if (producer->Service != nullptr)
    {
        AllJoynMessageInfo^ callInfo = ref new AllJoynMessageInfo(AllJoynHelpers::MultibyteToPlatformString(alljoyn_message_getsender(message)));

        uint32 inputArg0;
        (void)TypeConversionHelpers::GetAllJoynMessageArg(alljoyn_message_getarg(message, 0), "u", &inputArg0);

        remotecontrolvehicleGetAnalogChannelDataResult^ result = create_task(producer->Service->GetAnalogChannelDataAsync(callInfo, inputArg0)).get();
        create_task([](){}).then([=]
        {
            int32 status;

            if (nullptr == result)
            {
                alljoyn_busobject_methodreply_status(busObject, message, ER_BUS_NO_LISTENER);
                return;
            }

            status = result->Status;
            if (AllJoynStatus::Ok != status)
            {
                alljoyn_busobject_methodreply_status(busObject, message, static_cast<QStatus>(status));
                return;
            }

            size_t argCount = 1;
            alljoyn_msgarg outputs = alljoyn_msgarg_array_create(argCount);

            status = TypeConversionHelpers::SetAllJoynMessageArg(alljoyn_msgarg_array_element(outputs, 0), "(ddddd)", result->AnalogChannelData);
            if (AllJoynStatus::Ok != status)
            {
                alljoyn_busobject_methodreply_status(busObject, message, static_cast<QStatus>(status));
                alljoyn_msgarg_destroy(outputs);
                return;
            }

            alljoyn_busobject_methodreply_args(busObject, message, outputs, argCount);
            alljoyn_msgarg_destroy(outputs);
        }, result->m_creationContext).wait();
    }
}

void remotecontrolvehicleProducer::CallSetAnalogChannelStateHandler(_Inout_ alljoyn_busobject busObject, _In_ alljoyn_message message)
{
    auto source = SourceObjects.find(busObject);
    if (source == SourceObjects.end())
    {
        return;
    }

    remotecontrolvehicleProducer^ producer = source->second->Resolve<remotecontrolvehicleProducer>();
    if (producer->Service != nullptr)
    {
        AllJoynMessageInfo^ callInfo = ref new AllJoynMessageInfo(AllJoynHelpers::MultibyteToPlatformString(alljoyn_message_getsender(message)));

        uint32 inputArg0;
        (void)TypeConversionHelpers::GetAllJoynMessageArg(alljoyn_message_getarg(message, 0), "u", &inputArg0);
        double inputArg1;
        (void)TypeConversionHelpers::GetAllJoynMessageArg(alljoyn_message_getarg(message, 1), "d", &inputArg1);

        remotecontrolvehicleSetAnalogChannelStateResult^ result = create_task(producer->Service->SetAnalogChannelStateAsync(callInfo, inputArg0, inputArg1)).get();
        create_task([](){}).then([=]
        {
            int32 status;

            if (nullptr == result)
            {
                alljoyn_busobject_methodreply_status(busObject, message, ER_BUS_NO_LISTENER);
                return;
            }

            status = result->Status;
            if (AllJoynStatus::Ok != status)
            {
                alljoyn_busobject_methodreply_status(busObject, message, static_cast<QStatus>(status));
                return;
            }

            size_t argCount = 0;
            alljoyn_msgarg outputs = alljoyn_msgarg_array_create(argCount);

            alljoyn_busobject_methodreply_args(busObject, message, outputs, argCount);
            alljoyn_msgarg_destroy(outputs);
        }, result->m_creationContext).wait();
    }
}

void remotecontrolvehicleProducer::CallSetToggleChannelStateHandler(_Inout_ alljoyn_busobject busObject, _In_ alljoyn_message message)
{
    auto source = SourceObjects.find(busObject);
    if (source == SourceObjects.end())
    {
        return;
    }

    remotecontrolvehicleProducer^ producer = source->second->Resolve<remotecontrolvehicleProducer>();
    if (producer->Service != nullptr)
    {
        AllJoynMessageInfo^ callInfo = ref new AllJoynMessageInfo(AllJoynHelpers::MultibyteToPlatformString(alljoyn_message_getsender(message)));

        uint32 inputArg0;
        (void)TypeConversionHelpers::GetAllJoynMessageArg(alljoyn_message_getarg(message, 0), "u", &inputArg0);
        uint32 inputArg1;
        (void)TypeConversionHelpers::GetAllJoynMessageArg(alljoyn_message_getarg(message, 1), "u", &inputArg1);

        remotecontrolvehicleSetToggleChannelStateResult^ result = create_task(producer->Service->SetToggleChannelStateAsync(callInfo, inputArg0, inputArg1)).get();
        create_task([](){}).then([=]
        {
            int32 status;

            if (nullptr == result)
            {
                alljoyn_busobject_methodreply_status(busObject, message, ER_BUS_NO_LISTENER);
                return;
            }

            status = result->Status;
            if (AllJoynStatus::Ok != status)
            {
                alljoyn_busobject_methodreply_status(busObject, message, static_cast<QStatus>(status));
                return;
            }

            size_t argCount = 0;
            alljoyn_msgarg outputs = alljoyn_msgarg_array_create(argCount);

            alljoyn_busobject_methodreply_args(busObject, message, outputs, argCount);
            alljoyn_msgarg_destroy(outputs);
        }, result->m_creationContext).wait();
    }
}

void remotecontrolvehicleProducer::CallSetMultipleAnalogChannelStatesHandler(_Inout_ alljoyn_busobject busObject, _In_ alljoyn_message message)
{
    auto source = SourceObjects.find(busObject);
    if (source == SourceObjects.end())
    {
        return;
    }

    remotecontrolvehicleProducer^ producer = source->second->Resolve<remotecontrolvehicleProducer>();
    if (producer->Service != nullptr)
    {
        AllJoynMessageInfo^ callInfo = ref new AllJoynMessageInfo(AllJoynHelpers::MultibyteToPlatformString(alljoyn_message_getsender(message)));

        Windows::Foundation::Collections::IVectorView<uint32>^ inputArg0;
        (void)TypeConversionHelpers::GetAllJoynMessageArg(alljoyn_message_getarg(message, 0), "au", &inputArg0);
        Windows::Foundation::Collections::IVectorView<double>^ inputArg1;
        (void)TypeConversionHelpers::GetAllJoynMessageArg(alljoyn_message_getarg(message, 1), "ad", &inputArg1);

        remotecontrolvehicleSetMultipleAnalogChannelStatesResult^ result = create_task(producer->Service->SetMultipleAnalogChannelStatesAsync(callInfo, inputArg0, inputArg1)).get();
        create_task([](){}).then([=]
        {
            int32 status;

            if (nullptr == result)
            {
                alljoyn_busobject_methodreply_status(busObject, message, ER_BUS_NO_LISTENER);
                return;
            }

            status = result->Status;
            if (AllJoynStatus::Ok != status)
            {
                alljoyn_busobject_methodreply_status(busObject, message, static_cast<QStatus>(status));
                return;
            }

            size_t argCount = 0;
            alljoyn_msgarg outputs = alljoyn_msgarg_array_create(argCount);

            alljoyn_busobject_methodreply_args(busObject, message, outputs, argCount);
            alljoyn_msgarg_destroy(outputs);
        }, result->m_creationContext).wait();
    }
}

QStatus remotecontrolvehicleProducer::AddMethodHandler(_In_ alljoyn_interfacedescription interfaceDescription, _In_ PCSTR methodName, _In_ alljoyn_messagereceiver_methodhandler_ptr handler)
{
    alljoyn_interfacedescription_member member;
    if (!alljoyn_interfacedescription_getmember(interfaceDescription, methodName, &member))
    {
        return ER_BUS_INTERFACE_NO_SUCH_MEMBER;
    }

    return alljoyn_busobject_addmethodhandler(
        m_busObject,
        member,
        handler,
        m_weak);
}

QStatus remotecontrolvehicleProducer::AddSignalHandler(_In_ alljoyn_busattachment busAttachment, _In_ alljoyn_interfacedescription interfaceDescription, _In_ PCSTR methodName, _In_ alljoyn_messagereceiver_signalhandler_ptr handler)
{
    alljoyn_interfacedescription_member member;
    if (!alljoyn_interfacedescription_getmember(interfaceDescription, methodName, &member))
    {
        return ER_BUS_INTERFACE_NO_SUCH_MEMBER;
    }

    return alljoyn_busattachment_registersignalhandler(busAttachment, handler, member, NULL);
}

QStatus remotecontrolvehicleProducer::OnPropertyGet(_In_ PCSTR interfaceName, _In_ PCSTR propertyName, _Inout_ alljoyn_msgarg value)
{
    UNREFERENCED_PARAMETER(interfaceName);

    if (0 == strcmp(propertyName, "ReceiverName"))
    {
        auto task = create_task(Service->GetReceiverNameAsync(nullptr));
        auto result = task.get();

        return create_task([](){}).then([=]() -> QStatus
        {
            if (AllJoynStatus::Ok != result->Status)
            {
                return static_cast<QStatus>(result->Status);
            }
            return static_cast<QStatus>(TypeConversionHelpers::SetAllJoynMessageArg(value, "s", result->ReceiverName));
        }, result->m_creationContext).get();
    }
    if (0 == strcmp(propertyName, "Manufacturer"))
    {
        auto task = create_task(Service->GetManufacturerAsync(nullptr));
        auto result = task.get();

        return create_task([](){}).then([=]() -> QStatus
        {
            if (AllJoynStatus::Ok != result->Status)
            {
                return static_cast<QStatus>(result->Status);
            }
            return static_cast<QStatus>(TypeConversionHelpers::SetAllJoynMessageArg(value, "s", result->Manufacturer));
        }, result->m_creationContext).get();
    }
    if (0 == strcmp(propertyName, "DeviceType"))
    {
        auto task = create_task(Service->GetDeviceTypeAsync(nullptr));
        auto result = task.get();

        return create_task([](){}).then([=]() -> QStatus
        {
            if (AllJoynStatus::Ok != result->Status)
            {
                return static_cast<QStatus>(result->Status);
            }
            return static_cast<QStatus>(TypeConversionHelpers::SetAllJoynMessageArg(value, "u", result->DeviceType));
        }, result->m_creationContext).get();
    }
    if (0 == strcmp(propertyName, "Channels"))
    {
        auto task = create_task(Service->GetChannelsAsync(nullptr));
        auto result = task.get();

        return create_task([](){}).then([=]() -> QStatus
        {
            if (AllJoynStatus::Ok != result->Status)
            {
                return static_cast<QStatus>(result->Status);
            }
            return static_cast<QStatus>(TypeConversionHelpers::SetAllJoynMessageArg(value, "a(uuus)", result->Channels));
        }, result->m_creationContext).get();
    }

    return ER_BUS_NO_SUCH_PROPERTY;
}

QStatus remotecontrolvehicleProducer::OnPropertySet(_In_ PCSTR interfaceName, _In_ PCSTR propertyName, _In_ alljoyn_msgarg value)
{
    UNREFERENCED_PARAMETER(interfaceName);

    return ER_BUS_NO_SUCH_PROPERTY;
}

void remotecontrolvehicleProducer::Start()
{
    if (nullptr == m_busAttachment)
    {
        StopInternal(ER_FAIL);
        return;
    }

    QStatus result = AllJoynHelpers::CreateInterfaces(m_busAttachment, c_remotecontrolvehicleIntrospectionXml);
    if (result != ER_OK)
    {
        StopInternal(result);
        return;
    }

    result = AllJoynHelpers::CreateBusObject<remotecontrolvehicleProducer>(m_weak);
    if (result != ER_OK)
    {
        StopInternal(result);
        return;
    }

    alljoyn_interfacedescription interfaceDescription = alljoyn_busattachment_getinterface(AllJoynHelpers::GetInternalBusAttachment(m_busAttachment), "com.ooeygui.remotecontrolvehicle");
    if (interfaceDescription == nullptr)
    {
        StopInternal(ER_FAIL);
        return;
    }
    alljoyn_busobject_addinterface_announced(BusObject, interfaceDescription);

    result = AddMethodHandler(
        interfaceDescription, 
        "GetAnalogChannelData", 
        [](alljoyn_busobject busObject, const alljoyn_interfacedescription_member* member, alljoyn_message message) { UNREFERENCED_PARAMETER(member); CallGetAnalogChannelDataHandler(busObject, message); });
    if (result != ER_OK)
    {
        StopInternal(result);
        return;
    }

    result = AddMethodHandler(
        interfaceDescription, 
        "SetAnalogChannelState", 
        [](alljoyn_busobject busObject, const alljoyn_interfacedescription_member* member, alljoyn_message message) { UNREFERENCED_PARAMETER(member); CallSetAnalogChannelStateHandler(busObject, message); });
    if (result != ER_OK)
    {
        StopInternal(result);
        return;
    }

    result = AddMethodHandler(
        interfaceDescription, 
        "SetToggleChannelState", 
        [](alljoyn_busobject busObject, const alljoyn_interfacedescription_member* member, alljoyn_message message) { UNREFERENCED_PARAMETER(member); CallSetToggleChannelStateHandler(busObject, message); });
    if (result != ER_OK)
    {
        StopInternal(result);
        return;
    }

    result = AddMethodHandler(
        interfaceDescription, 
        "SetMultipleAnalogChannelStates", 
        [](alljoyn_busobject busObject, const alljoyn_interfacedescription_member* member, alljoyn_message message) { UNREFERENCED_PARAMETER(member); CallSetMultipleAnalogChannelStatesHandler(busObject, message); });
    if (result != ER_OK)
    {
        StopInternal(result);
        return;
    }

    
    SourceObjects[m_busObject] = m_weak;
    SourceInterfaces[interfaceDescription] = m_weak;
    
    unsigned int noneMechanismIndex = 0;
    bool authenticationMechanismsContainsNone = m_busAttachment->AuthenticationMechanisms->IndexOf(AllJoynAuthenticationMechanism::None, &noneMechanismIndex);
    QCC_BOOL interfaceIsSecure = alljoyn_interfacedescription_issecure(interfaceDescription);

    // If the current set of AuthenticationMechanisms supports authentication,
    // determine whether a secure BusObject is required.
    if (AllJoynHelpers::CanSecure(m_busAttachment->AuthenticationMechanisms))
    {
        // Register the BusObject as "secure" if the org.alljoyn.Bus.Secure XML annotation
        // is specified, or if None is not present in AuthenticationMechanisms.
        if (!authenticationMechanismsContainsNone || interfaceIsSecure)
        {
            result = alljoyn_busattachment_registerbusobject_secure(AllJoynHelpers::GetInternalBusAttachment(m_busAttachment), BusObject);
        }
        else
        {
            result = alljoyn_busattachment_registerbusobject(AllJoynHelpers::GetInternalBusAttachment(m_busAttachment), BusObject);
        }
    }
    else
    {
        // If the current set of AuthenticationMechanisms does not support authentication
        // but the interface requires security, report an error.
        if (interfaceIsSecure)
        {
            result = ER_BUS_NO_AUTHENTICATION_MECHANISM;
        }
        else
        {
            result = alljoyn_busattachment_registerbusobject(AllJoynHelpers::GetInternalBusAttachment(m_busAttachment), BusObject);
        }
    }

    if (result != ER_OK)
    {
        StopInternal(result);
        return;
    }

    m_busAttachmentStateChangedToken = m_busAttachment->StateChanged += ref new TypedEventHandler<AllJoynBusAttachment^,AllJoynBusAttachmentStateChangedEventArgs^>(this, &remotecontrolvehicleProducer::BusAttachmentStateChanged);
    m_busAttachment->Connect();
}

void remotecontrolvehicleProducer::Stop()
{
    StopInternal(AllJoynStatus::Ok);
}

void remotecontrolvehicleProducer::StopInternal(int32 status)
{
    UnregisterFromBus();
    Stopped(this, ref new AllJoynProducerStoppedEventArgs(status));
}

int32 remotecontrolvehicleProducer::RemoveMemberFromSession(_In_ String^ uniqueName)
{
    return alljoyn_busattachment_removesessionmember(
        AllJoynHelpers::GetInternalBusAttachment(m_busAttachment),
        m_sessionId,
        AllJoynHelpers::PlatformToMultibyteString(uniqueName).data());
}

PCSTR com::ooeygui::remotecontrolvehicle::c_remotecontrolvehicleIntrospectionXml = "<interface name=\"com.ooeygui.remotecontrolvehicle\">"
"  <property name=\"ReceiverName\" type=\"s\" access=\"read\" />"
"  <property name=\"Manufacturer\" type=\"s\" access=\"read\" />"
"  <property name=\"DeviceType\" type=\"u\" access=\"read\" />"
"  <property name=\"Channels\" type=\"a(uuus)\" access=\"read\" />"
"  <method name=\"GetAnalogChannelData\">"
"    <arg name=\"channelId\" type=\"u\" direction=\"in\" />"
"    <arg name=\"analogChannelData\" type=\"ddddd\" direction=\"out\" />"
"  </method>"
"  <method name=\"SetAnalogChannelState\">"
"    <arg name=\"channelId\" type=\"u\" direction=\"in\" />"
"    <arg name=\"value\" type=\"d\" direction=\"in\" />"
"  </method>"
"  <method name=\"SetToggleChannelState\">"
"    <arg name=\"channelId\" type=\"u\" direction=\"in\" />"
"    <arg name=\"value\" type=\"u\" direction=\"in\" />"
"  </method>"
"  <method name=\"SetMultipleAnalogChannelStates\">"
"    <arg name=\"channelIds\" type=\"au\" direction=\"in\" />"
"    <arg name=\"values\" type=\"ad\" direction=\"in\" />"
"  </method>"
"</interface>"
;
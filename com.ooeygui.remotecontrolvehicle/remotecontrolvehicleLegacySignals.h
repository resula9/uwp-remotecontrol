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

ref class remotecontrolvehicleLegacySignals;

public interface class IremotecontrolvehicleLegacySignals
{
};

public ref class remotecontrolvehicleLegacySignals sealed : [Windows::Foundation::Metadata::Default] IremotecontrolvehicleLegacySignals
{
public:
internal:
    void Initialize(_In_ alljoyn_busobject busObject, _In_ alljoyn_sessionid sessionId);

private:
    alljoyn_busobject m_busObject;
    alljoyn_sessionid m_sessionId;


};

} } } 
using Content.Shared.Speech;
using Robust.Shared.Prototypes;

namespace Content.Server.VoiceMask;

[RegisterComponent]
public sealed partial class VoiceMaskerComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)] public string LastSetName = "Неизвестный";
    [ViewVariables(VVAccess.ReadWrite)] public string? LastSetVoice; // Corvax-TTS

    [DataField]
    public ProtoId<SpeechVerbPrototype>? LastSpeechVerb;

    [DataField]
    public EntProtoId Action = "ActionChangeVoiceMask";

    [DataField]
    public EntityUid? ActionEntity;
}

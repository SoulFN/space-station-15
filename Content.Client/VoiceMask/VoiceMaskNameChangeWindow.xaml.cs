using System.Linq;
using Content.Shared.Corvax.TTS;
using Content.Client.UserInterface.Controls;
using Content.Shared.Speech;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Prototypes;

namespace Content.Client.VoiceMask;

[GenerateTypedNameReferences]
public sealed partial class VoiceMaskNameChangeWindow : FancyWindow
{
    private readonly List<TTSVoicePrototype> _voices; // Corvax-TTS

    public Action<string>? OnNameChange;
    public Action<string>? OnVoiceChange; // Corvax-TTS
    public Action<string?>? OnVerbChange;

    private List<(string, string)> _verbs = new();

    private string? _verb;

    public VoiceMaskNameChangeWindow(IPrototypeManager proto)
    {
        RobustXamlLoader.Load(this);

        NameSelectorSet.OnPressed += _ =>
        {
            OnNameChange?.Invoke(NameSelector.Text);
        };
        // Corvax-TTS-Start
        VoiceSelector.OnItemSelected += args =>
        {
            VoiceSelector.SelectId(args.Id);
            if (VoiceSelector.SelectedMetadata != null)
                OnVoiceChange!((string)VoiceSelector.SelectedMetadata);
        };
        _voices = IoCManager
            .Resolve<IPrototypeManager>()
            .EnumeratePrototypes<TTSVoicePrototype>()
            .Where(o => o.RoundStart)
            .OrderBy(o => Loc.GetString(o.Name))
            .ToList();
        for (var i = 0; i < _voices.Count; i++)
        {
            var name = Loc.GetString(_voices[i].Name);
            VoiceSelector.AddItem(name);
            VoiceSelector.SetItemMetadata(i, _voices[i].ID);
        }
        // Corvax-TTS-End
        SpeechVerbSelector.OnItemSelected += args =>
{
    OnVerbChange?.Invoke((string?) args.Button.GetItemMetadata(args.Id));
    SpeechVerbSelector.SelectId(args.Id);
};

        ReloadVerbs(proto);

        AddVerbs();
    }

    public void UpdateState(string name, string? voice, string? verb) // Corvax-TTS
    {
        NameSelector.Text = name;

        // Corvax-TTS-Start
        var voiceIdx = _voices.FindIndex(v => v.ID == voice);
        if (voiceIdx != -1)
            VoiceSelector.Select(voiceIdx);
        // Corvax-TTS-End

        _verb = verb;

        for (int id = 0; id < SpeechVerbSelector.ItemCount; id++)
        {
            if (string.Equals(verb, SpeechVerbSelector.GetItemMetadata(id)))
            {
                SpeechVerbSelector.SelectId(id);
                break;
            }
        }
    }

    private void ReloadVerbs(IPrototypeManager proto)
    {
        foreach (var verb in proto.EnumeratePrototypes<SpeechVerbPrototype>())
        {
            _verbs.Add((Loc.GetString(verb.Name), verb.ID));
        }
        _verbs.Sort((a, b) => a.Item1.CompareTo(b.Item1));
    }

    private void AddVerbs()
    {
        SpeechVerbSelector.Clear();

        AddVerb(Loc.GetString("chat-speech-verb-name-none"), null);
        foreach (var (name, id) in _verbs)
        {
            AddVerb(name, id);
        }
    }

    private void AddVerb(string name, string? verb)
    {
        var id = SpeechVerbSelector.ItemCount;
        SpeechVerbSelector.AddItem(name);
        if (verb is {} metadata)
            SpeechVerbSelector.SetItemMetadata(id, metadata);

        if (verb == _verb)
            SpeechVerbSelector.SelectId(id);
    }

    public void UpdateState(string name, string? verb)
    {

    }
}

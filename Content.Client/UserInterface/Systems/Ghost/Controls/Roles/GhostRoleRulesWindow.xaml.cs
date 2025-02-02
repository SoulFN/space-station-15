using Content.Shared.CCVar;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Configuration;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Client.UserInterface.Systems.Ghost.Controls.Roles
{
    [GenerateTypedNameReferences]
    public sealed partial class GhostRoleRulesWindow : DefaultWindow
    {
        [Dependency] private readonly IConfigurationManager _cfg = IoCManager.Resolve<IConfigurationManager>();
        private float _timer;
        public Action? TimeOver; // SPACE STORIES

        public GhostRoleRulesWindow(string rules, Action action) // SPACE STORIES
        {
            RobustXamlLoader.Load(this);
            var ghostRoleTime = _cfg.GetCVar(CCVars.GhostRoleTime);
            _timer = ghostRoleTime;

            if (ghostRoleTime > 0f)
            {
                action.Invoke(); // SPACE STORIES
                RequestButton.Text = Loc.GetString("stories-ghost-roles-window-request-role-button-timer", ("time", $"{_timer:0.0}"));
                TopBanner.SetMessage(FormattedMessage.FromMarkupPermissive(rules + "\n" + Loc.GetString("stories-ghost-roles-window-rules-footer", ("time", ghostRoleTime))));
                // RequestButton.Disabled = true; // SPACE STORIES
            }

            // RequestButton.OnPressed += requestAction; // SPACE STORIES
        }


        protected override void FrameUpdate(FrameEventArgs args)
        {
            base.FrameUpdate(args);
            // if (!RequestButton.Disabled) return; // SPACE STORIES
            if (_timer > 0.0)
            {
                _timer -= args.DeltaSeconds;
                RequestButton.Text = Loc.GetString("stories-ghost-roles-window-request-role-button-timer", ("time", $"{_timer:0.0}"));
            }
            else
            {
                // RequestButton.Disabled = false; // SPACE STORIES
                RequestButton.Text = Loc.GetString("stories-ghost-roles-window-request-role-button");
                TimeOver?.Invoke(); // SPACE STORIES
            }
        }
    }
}

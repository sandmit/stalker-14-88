﻿/*
 * Project: raincidation
 * File: RDDeathScreenSystem.cs
 * License: All rights reserved
 * Copyright: (c) 2025 TornadoTechnology
 *
 * For the full license text, see the LICENSE file in the project root.
 * Link: https://github.com/Rainlucid/raincidation
 */

using Content.Shared._RD.DeathScreen;
using Content.Shared.Ghost;
using Robust.Client.Audio;
using Robust.Client.Player;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Shared.Audio.Sources;
using Robust.Shared.Player;

namespace Content.Client._RD.DeathScreen;

public sealed class RDDeathScreenSystem : EntitySystem
{
    [Dependency] private readonly IAudioManager _audio = default!;
    [Dependency] private readonly IUserInterfaceManager _userInterface = default!;
    [Dependency] private readonly IResourceCache _resourceCache = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    private RDDeathScreenControl _ui = default!;
    private bool _remove;
    private IAudioSource? _source;

    public override void Initialize()
    {
        SubscribeNetworkEvent<RDDeathScreenShowEvent>(OnDeath);

        _ui = new RDDeathScreenControl();
        _ui.OnAnimationEnd += OnAnimationEnd;

        _playerManager.LocalPlayerAttached += OnPlayerAttached;
        _playerManager.LocalPlayerDetached += OnPlayerDetached;
    }

    public override void FrameUpdate(float frameTime)
    {
        base.FrameUpdate(frameTime);

        if (!_remove)
            return;

        _userInterface.RootControl.RemoveChild(_ui);
        _remove = false;
    }

    private void OnDeath(RDDeathScreenShowEvent ev)
    {
        _source = null;

        //Log.Debug($"Start death screen \"{ev}\"");

        if (ev.AudioPath != string.Empty)
        {
            _source = _audio.CreateAudioSource(_resourceCache.GetResource<AudioResource>(ev.AudioPath));
            if (_source is not null)
            {
                _source.Global = true;
                _source.Restart();
            }
        }

        _ui.AnimationStart(ev);
        _userInterface.RootControl.AddChild(_ui);
    }

   /*
   We really want to remove death screen if player got attached to other entity(ghost), or detached at all (lobby)
   Autho:r Kiriyaga7615
  */
    private void OnPlayerAttached(EntityUid uid)
    {
          EndDeathScreen();
    }

    private void OnPlayerDetached(EntityUid uid)
    {
        EndDeathScreen();
    }

    private void OnAnimationEnd()
    {
        _remove = true;
    }

    private void EndDeathScreen()
    {
        if (_userInterface.RootControl.Children.Contains(_ui))
            _userInterface.RootControl.RemoveChild(_ui);

        _source?.StopPlaying();
        _source = null;
        _remove = false;
    }
}

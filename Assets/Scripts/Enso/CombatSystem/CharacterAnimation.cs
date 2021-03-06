﻿using Framework.Animations;
using UnityEngine;

namespace Enso.CombatSystem
{
    public abstract class CharacterAnimation : ScriptableObject
    {
        public AnimationClipHolder ClipHolder;
        public FrameChecker AnimationFrameChecker;
        public bool CanBeCut = true;
    }
}

using System;
using Poker.Code.Data.Enums;
using UnityEngine;

namespace Poker.Code.Data.StaticData.Sounds
{
    [Serializable]
    public class AudioClipData
    {
        public AudioClip Clip;
        public SoundId Id;
    }
}
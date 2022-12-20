using System.Collections.Generic;
using System.Linq;
using Poker.Code.Data.Enums;
using Poker.Code.Data.StaticData.Sounds;
using UnityEngine;

namespace Poker.Code.Services.Sound
{
    public class SoundService : MonoBehaviour, ISoundService
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _effectsSource;
        
        private Dictionary<SoundId, AudioClipData> _sounds;

        public void Construct(SoundData soundData)
        {
            _sounds = soundData.AudioEffectClips.ToDictionary(s => s.Id);
            _musicSource.clip = soundData.BackgroundMusic;
        }

        private void Awake() => DontDestroyOnLoad(this);

        public void EnableBackgroundMusic() => _musicSource.Play();

        public void DisableBackgroundMusic() => _musicSource.Stop();

        public void PlayEffectSound(SoundId soundId) =>
            _effectsSource.PlayOneShot(_sounds[soundId].Clip);

        public void SetBackgroundVolume(float volume) =>
            _musicSource.volume = volume;

        public void SetEffectsVolume(float volume) =>
            _effectsSource.volume = volume;
    }
}
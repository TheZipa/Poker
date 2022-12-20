using Poker.Code.Data.Enums;
using Poker.Code.Data.StaticData.Sounds;
using Poker.Code.Infrastructure.ServiceContainer;

namespace Poker.Code.Services.Sound
{
    public interface ISoundService : IService
    {
        void Construct(SoundData soundData);
        void PlayEffectSound(SoundId soundId);
        void SetBackgroundVolume(float volume);
        void SetEffectsVolume(float volume);
        void EnableBackgroundMusic();
        void DisableBackgroundMusic();
    }
}
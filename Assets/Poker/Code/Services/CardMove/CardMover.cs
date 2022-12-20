using System.Threading.Tasks;
using DG.Tweening;
using Poker.Code.Core.Cards.View;
using Poker.Code.Data.Enums;
using Poker.Code.Services.Sound;
using UnityEngine;

namespace Poker.Code.Services.CardMove
{
    public class CardMover : ICardMover
    {
        private readonly ISoundService _soundService;
        private readonly float _animationSpeed;

        public CardMover(ISoundService soundService, float animationSpeed)
        {
            _soundService = soundService;
            _animationSpeed = animationSpeed;
        }

        public async Task MoveCard(CardView card, Vector3 toMovePos)
        {
            _soundService.PlayEffectSound(SoundId.CardDeal);
            await card.transform.DOMove(toMovePos, _animationSpeed).AsyncWaitForCompletion();
        }

        public async void RotateCard(CardView card, Vector3 rotation)
        {
            _soundService.PlayEffectSound(SoundId.CardDeal);
            await card.transform.DORotate(rotation, _animationSpeed).AsyncWaitForCompletion();
        }

        public async Task ShowCard(CardView card)
        {
            Vector3 startRotation = card.transform.rotation.eulerAngles;
            Vector3 startPosition = card.transform.position;

            _soundService.PlayEffectSound(SoundId.CardDeal);
            await card.transform
                .DOMove(new Vector3(startPosition.x, startPosition.y + 0.5f, startPosition.z), _animationSpeed)
                .AsyncWaitForCompletion();

            RotateCard(card, new Vector3(startRotation.x, startRotation.y, 180));

            await card.transform.DOMove(startPosition, _animationSpeed).AsyncWaitForCompletion();
        }
    }
}
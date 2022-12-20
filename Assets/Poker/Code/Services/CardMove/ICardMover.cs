using System.Threading.Tasks;
using Poker.Code.Core.Cards.View;
using Poker.Code.Infrastructure.ServiceContainer;
using UnityEngine;

namespace Poker.Code.Services.CardMove
{
    public interface ICardMover : IService
    {
        Task MoveCard(CardView card, Vector3 toMovePos);
        void RotateCard(CardView card, Vector3 rotation);
        Task ShowCard(CardView card);
    }
}
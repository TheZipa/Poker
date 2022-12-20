using System;
using System.Collections;
using Poker.Code.Core.Cards;
using Poker.Code.Core.Cards.CardStack;
using Poker.Code.Core.Cards.ModelCardDeck;
using Poker.Code.Core.Cards.View;
using Poker.Code.Core.Players;
using Poker.Code.Data.StaticData.Locations;
using Poker.Code.Services.CardMove;
using Poker.Code.Services.Factories.GameFactory;
using Poker.Code.Services.StaticData;
using UnityEngine;

namespace Poker.Code.Infrastructure.StateMachine.States
{
    public class CardDispenseState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IGameFactory _gameFactory;
        private readonly IStaticData _staticData;
        private readonly ICardMover _cardMover;

        private IModelCardDeck _modelCardDeck;
        private CardViewDeck _cardViewDeck;
        private ICardStack _cardStack;

        public CardDispenseState(IGameStateMachine gameStateMachine, ICoroutineRunner coroutineRunner,
            IGameFactory gameFactory, ICardMover cardMover, IStaticData staticData)
        {
            _coroutineRunner = coroutineRunner;
            _gameFactory = gameFactory;
            _gameStateMachine = gameStateMachine;
            _staticData = staticData;
            _cardMover = cardMover;
        }

        public void Enter()
        {
            SetDispenseDependencies();

            _modelCardDeck.Shuffle();

            _coroutineRunner.StartCoroutine(StartDispenseWithDelay(2.5f));
        }

        public void Exit()
        {
        }

        private IEnumerator StartDispenseWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            Player[] players = _gameFactory.AllPlayers;

            foreach (Player player in _gameFactory.AllPlayers)
                SetCardPair(player.CardPair);
            
            DispenseCardForPlayers(players, OnPlayerCardDispenseFinished);
        }

        private void SetCardStack(Action<CardView[]> onPrepared)
        {
            CardModel[] cardModels = new CardModel[5];
            CardView[] cardViews = new CardView[5];

            for (int i = 0; i < 5; i++)
            {
                GetModelViewCard(out CardView view, out CardModel model);
                cardModels[i] = model;
                cardViews[i] = view;
            }
            
            _cardStack.SetStack(cardModels, cardViews);
            onPrepared?.Invoke(cardViews);
        }
        
        private void SetCardPair(CardPair playerCardPair)
        {
            CardModel[] cardModels = new CardModel[2];
            CardView[] cardViews = new CardView[2];
            
            for (int i = 0; i < 2; i++)
            {
                GetModelViewCard(out CardView view, out CardModel model);
                cardViews[i] = view;
                cardModels[i] = model;
            }

            playerCardPair.SetCards(cardModels, cardViews);
        }

        private async void DispenseCardForPlayers(Player[] players, Action onComplete)
        {
            foreach (Player player in players)
            {
                CardView[] cardViews = player.CardPair.CardViews;
                Location leftCardLocation = player.CardPair.LeftCardLocation;
                Location rightCardLocation = player.CardPair.RightCardLocation;

                _cardMover.RotateCard(cardViews[0], leftCardLocation.Rotation.eulerAngles);
                await _cardMover.MoveCard(cardViews[0], leftCardLocation.Position);
                _cardMover.RotateCard(cardViews[1], rightCardLocation.Rotation.eulerAngles);
                await _cardMover.MoveCard(cardViews[1], rightCardLocation.Position);
            }
            
            onComplete.Invoke();
        }

        private void OnPlayerCardDispenseFinished()
        {
            ShowUserCards(_gameFactory.User.CardPair.CardViews);
            SetCardStack(OnStackPrepared);
        }
        
        private void OnStackPrepared(CardView[] cardViews) =>
            DispenseStack(cardViews, _staticData.LocationData.CardStackLocation, _staticData.GameConfig.CardStackOffset);

        private async void DispenseStack(CardView[] cardViews, Location location, float offset)
        {
            for (int i = 0; i < cardViews.Length; i++)
            {
                Vector3 stackPosition = location.Position;
                stackPosition.z += i * offset;
                _cardMover.RotateCard(cardViews[i], location.Rotation.eulerAngles);
                await _cardMover.MoveCard(cardViews[i], stackPosition);
            }
            
            _gameStateMachine.Enter<GameplayState>();
        }

        private async void ShowUserCards(CardView[] cardViews)
        {
            foreach (CardView cardView in cardViews)
                await _cardMover.ShowCard(cardView);
        }

        private void GetModelViewCard(out CardView view, out CardModel model)
        {
            model = _modelCardDeck.GetCard();
            view = _cardViewDeck.GetCard(model.Value, model.Suit);
        }

        private void SetDispenseDependencies()
        {
            _modelCardDeck = _gameFactory.ModelCardDeck;
            _cardViewDeck = _gameFactory.ViewCardDeck;
            _cardStack = _gameFactory.CardStack;
        }
    }
}
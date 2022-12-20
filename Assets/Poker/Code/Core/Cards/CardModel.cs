using System;
using System.Collections.Generic;
using Poker.Code.Data.Enums;

namespace Poker.Code.Core.Cards
{
    public class CardModel : IComparable<CardModel>
    {
        public readonly CardValue Value;
        public readonly CardSuit Suit;
        
        private readonly Dictionary<CardValue, char> _cardValueChars = new Dictionary<CardValue, char>()
        {
            [CardValue.Ace] = 'a',
            [CardValue.King] = 'k',
            [CardValue.Queen] = 'q',
            [CardValue.Jack] = 'j'
        };

        private readonly Dictionary<CardSuit, char> _cardSuitChars = new Dictionary<CardSuit, char>()
        {
            [CardSuit.Clubs] = 'c',
            [CardSuit.Diamonds] = 'd',
            [CardSuit.Hearts] = 'h',
            [CardSuit.Spades] = 's'
        };

        public CardModel(CardValue value, CardSuit suit)
        {
            Value = value;
            Suit = suit;
        }

        public override string ToString()
        {
            string card = String.Empty;

            if ((int)Value > 10) card += _cardValueChars[Value];
            else card += (int)Value;
            
            card += _cardSuitChars[Suit];
            return card;
        }

        public int CompareTo(CardModel obj) => (int)Value - (int)obj.Value;
    }
}
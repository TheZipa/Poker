using System;
using System.Collections.Generic;
using System.Linq;
using Poker.Code.Core.Cards;

namespace Poker.Code.Data.Extensions
{
    public static class GameplayExtensions
    {
        private static readonly Random _random = new Random();  
        
        public static void SortArrayByIndex<T>(this T[] array, int index)
        {
            int firstArrayLength = array.Length - index;
            int secondArrayLength = index;
            
            T[] firstArray = new T[firstArrayLength];
            T[] secondArray = new T[secondArrayLength];

            Array.Copy(array, index, firstArray, 0, firstArrayLength);
            Array.Copy(array, 0, secondArray, 0, secondArrayLength);

            firstArray.CopyTo(array, 0);
            secondArray.CopyTo(array, firstArrayLength);
        }

        public static string ToCombinationString(this CardModel[] totalCards)
        {
            string combinationString = String.Empty;

            foreach (CardModel card in totalCards)
                combinationString += card + " ";

            return combinationString;
        }
        
        public static void Shuffle<T>(this Stack<T> stack)
        {
            var values = stack.ToArray();
            stack.Clear();
            foreach (var value in values.OrderBy(x => _random.Next()))
                stack.Push(value);
        }

    }
}
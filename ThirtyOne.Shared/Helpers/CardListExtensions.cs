using System;
using System.Collections.Generic;
using System.Linq;
using ThirtyOne.Models;

namespace ThirtyOne.Helpers
{
    public static class CardListExtensions
    {
        public static int CalculateScore(this IEnumerable<Card> Cards)
        {
            return Cards
                .GroupBy(card => card.Suit) //group by suit
                .OrderByDescending(group => group.Sum(card => card.Value)) //per group make a sum of card values and order with descending 
                .First() // take the first group
                .Sum(card => card.Value);

        }

        public static string ToListString(this IEnumerable<Card> Cards)
        {
            return string.Join(",", Cards); 
        }
    }
}

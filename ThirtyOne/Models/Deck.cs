using System;
using System.Collections.Generic;

namespace ThirtyOne.Models
{
    public class Deck
    {
        public List<Card> Cards { get; set; } //property

        public int CardsLeft {
            get
            {
                return Cards.Count;
            }
        }
        //  public int CardsLeft => Cards.Count; }

        public Deck() {
            Cards = new List<Card>();
        }

        public void Initialize()
        {
            foreach (Suite suit in (Suite[])Enum.GetValues(typeof(Suite)))
            {
                for (int i = 1; i < 14; i++)
                {
                    Card card = new Card() { Rank = i, Suit = suit };
                    Cards.Add(card);
                }
            }
        }

        public void Shuffle(Random randomCard)
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                int from = i;
                int to = randomCard.Next(Cards.Count);
                Card card = Cards[to];
                Cards[to] = Cards[from];
                Cards[from] = card;
            }
        }

        public Card DrawCard()
        {
            if (Cards.Count == 0) return null;
            Card card = Cards[0];
            Cards.RemoveAt(0);
            return card;
        }
    }
}

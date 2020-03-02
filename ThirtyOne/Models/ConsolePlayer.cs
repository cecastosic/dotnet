﻿using System;
using System.Linq;
using ThirtyOne.Shared.Helpers;
using ThirtyOne.Shared.Models;

namespace ThirtyOne.Models
{
    public class ConsolePlayer : Player
    {
        public ConsolePlayer(string name) : base(name)
        {
        }

        public override void Turn(Game g)
        {
            Console.WriteLine("Your turn. Your hand: ");
            foreach (var c in Hand)
            {
                Console.WriteLine("\t" + c.ToString());
            }
            Console.WriteLine($"Hand score: {Hand.CalculateScore()} {Hand.ShowSuite()}\n"); // should show suit also
            if (g.Table.Count > 0)
            {
                Console.WriteLine("On the table there is " + g.Table.Last().ToString() + ". Do you want to draw from the Table (T) or the Deck (D) or Call/Knock (C)?");
                var c = Console.ReadLine().ToUpper();
                if (c == "T") DrawFromTable(g);
                else if (c == "D") DrawFromDeck(g);
                else
                {
                    this.HasKnocked = true;
                    return;
                }
            }
            else
            {
                DrawFromDeck(g);
            }
            Console.WriteLine("You drew a card. Your hand: ");
            for (var i = 0; i < Hand.Count; i++)
            {
                Console.WriteLine("\t" + (i + 1) + "\t" + Hand[i].ToString());
            }

            Console.WriteLine("Which card to drop? (1-4)");
            string input = Console.ReadLine();
            int action = int.Parse(input);
            DropCard(g, action - 1);
            Console.WriteLine("Your score: " + Hand.CalculateScore());
        }
    }
}

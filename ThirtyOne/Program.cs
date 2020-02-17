using System;
using ThirtyOne.Helpers;
using ThirtyOne.Models;

namespace ThirtyOne
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initial test
            // Deck deck = new Deck();
            // deck.Initialize();
            // Random randomCard = new Random();
            // deck.Shuffle(randomCard);
            // Card card = deck.DrawCard();

            //Game implementation
            Console.WriteLine("Let's play 31!");
            ComputerPlayer computerPlayer = new ComputerPlayer("Computer");
            Game game = new Game(computerPlayer, new ConsolePlayer("You"));
            bool isGameOver = false;
            while (!isGameOver)
            {
                Console.WriteLine($"{game.CurrentPlayer.Name} turn!");
                isGameOver = game.NextTurn();
            }

            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine($"--- GAME OVER, {game.Winner.Name} WON WITH {game.Winner.Hand.ToListString()} ---");
            Console.ReadLine();
        }
    }
}

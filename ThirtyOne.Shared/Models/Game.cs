using System;
using System.Collections.Generic;
using ThirtyOne.Helpers;
using System.Linq;
using Newtonsoft.Json;

namespace ThirtyOne.Models
{
    public class Game
    {
        public Deck Deck { get; set; }

        public List<Card> Table { get; set; }

        public List<Player> Players { get; set; }

        public int CurrentTurn { get; set; } 

        public Player CurrentPlayer { get
            {
                return Players[CurrentTurn];
            }
        }

        public Player Winner { get; set; }

        public GameState State { get; set; }

        public Game() {
            Players = new List<Player>();
            State = GameState.WaitingToStart;
        }

        private void InitialDeal()
        {
            //Deal
            foreach (var player in Players)
            {
                for (int i = 0; i < 3; i++) player.Hand.Add(Deck.DrawCard());
            }

            Table.Add(Deck.DrawCard());
        }

        public void StartGame() {
            Deck = new Deck();
            Table = new List<Card>();

            Deck.Initialize();
            Random randomCard = new Random();
            Deck.Shuffle(randomCard);
            Winner = null;
            CurrentTurn = 0;
            InitialDeal();
            State = GameState.InProgress;
        }

        public Game(params Player[] players) : this()
        {
            Players = players.ToList();
            StartGame();
        }

        public bool EvaluateIfGameOver(bool called)
        {
            var winPlayer = (called) ?
                Players.Where(player => player.Hand.Count == 3).OrderByDescending(player => player.Hand.CalculateScore()).First() //The game has been called, player with the highest score is the winner
                : Players.Where(player => player.Hand.Count == 3 && player.Hand.CalculateScore() == 31).FirstOrDefault(); //Game has not been called, but a player has 31 and wins.

            if (winPlayer != null)
            {
                this.Winner = winPlayer;
                this.State = GameState.GameOver;
                return true;
            }
            return false;
        }

        public bool NextTurn()
        {
            //Ask player to do their turn
            CurrentPlayer.Turn(this);

            if (EvaluateIfGameOver(false)) // false as not called, a winner has 31 cards
            {
                //Player won, report
                return true;
            }

            //Move to the next player
            CurrentTurn++;
            if (CurrentTurn >= Players.Count) CurrentTurn = 0;
            if (CurrentPlayer.HasKnocked)
            {
                //Next player had already knocked - let's evaluate the call
                EvaluateIfGameOver(true);
                //Game over!
                return true;
            }

            if (Deck.CardsLeft == 0)
            {
                //If there's no more cards in the deck, let's take those from the table
                Deck.Cards.AddRange(Table); // addrange - adding list into a list
                Table.Clear();
            }

            if (CurrentPlayer is ComputerPlayer) return NextTurn(); //If the next player is the computer, execute that turn right away.
            else return false;
        }

        public string SerializeGame()
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            return JsonConvert.SerializeObject(this, jsonSerializerSettings);
        }

        public static Game DeserializeGame(string json)
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            return JsonConvert.DeserializeObject<Game>(json, jsonSerializerSettings);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThirtyOne.Shared.Models;
using ThirtyOne.Web.Helpers;
using ThirtyOne.Web.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ThirtyOne.Web.Controllers
{
    public class GameController : Controller
    {

        private readonly GameService _gameService;

        public GameController()
        {
            _gameService = new GameService();
        }

        // GET: /<controller>/
        public IActionResult New(string PlayerName)
        {
            Game game = new Game();

            Random randomNum = new Random();
            game.GameId = randomNum.Next(1000, 9999);
            while (_gameService.GameExist(game.GameId)) //Check if the ID is already in use
                game.GameId = randomNum.Next(1000, 9999);

            WebPlayer human = new WebPlayer(PlayerName);
            game.Players.Add(human);
            ComputerPlayer computer = new ComputerPlayer("Computer");
            game.Players.Add(computer);

            game.StartGame();

            _gameService.SaveGame(game);

            return RedirectToAction("Index", new { Id = game.GameId.ToString() });
        }

        public IActionResult Index(int Id)
        {
            try
            {
                Game game = _gameService.LoadGame(Id);

                WebPlayer human = game.Players.First() as WebPlayer;

                GameViewModel viewModel = new GameViewModel() { CurrentGame = game, CurrentPlayer = human };

                return View(viewModel);
            } catch (FileNotFoundException) //exc
            {
                //Game does not exist, send back to start.
                return Redirect("/");
            }
            catch
            {
                throw;
            }

        }

        public IActionResult MakeAMove(int Id, PlayerAction Move)
        {
            Game game = _gameService.LoadGame(Id);
            WebPlayer human = game.CurrentPlayer as WebPlayer;

            switch (Move) {
                case PlayerAction.DrawFromDeck:
                    human.DrawFromDeck(game);
                    human.LastAction = "drew from deck";
                    break;
                case PlayerAction.DrawFromTable:
                    human.DrawFromTable(game);
                    human.LastAction = "draw from table";
                    break;
                case PlayerAction.Knock:
                    human.HasKnocked = true;
                    human.LastAction = "knocked";
                    game.NextTurn(); // computer's turn
                    return GameOver(game);
            }
            _gameService.SaveGame(game);
            return RedirectToAction("Index", new { Id = game.GameId });
        }

        private IActionResult GameOver(Game game)
        {
            _gameService.DeleteGame(game.GameId);
            return View("GameOver", game);
        }

        public IActionResult DropCard(int Id, int Card)
        {
            Game game = _gameService.LoadGame(Id);
            WebPlayer human = game.CurrentPlayer as WebPlayer;
            human.LastAction += " then dropped " + human.Hand[Card].ToString();
            human.DropCard(game, Card);

            if (game.NextTurn()) return GameOver(game);

            //Save game state
            _gameService.SaveGame(game);

            return RedirectToAction("Index", new { Id = game.GameId });
        }
    }
}

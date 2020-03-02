using System;
using System.IO;
using System.Runtime.InteropServices;
using ThirtyOne.Shared.Models;

namespace ThirtyOne.Web.Helpers
{
    public class GameService
    {
        private readonly string AppDataPath;

        public GameService()
        {
            // where you want to save json file with started game
            AppDataPath = Path.Combine(
                Environment.GetEnvironmentVariable(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "LocalAppData" : "HOME"), "ThirtyOneGame");
            Directory.CreateDirectory(AppDataPath);
        }

        private string GetFilePath(int GameId)
        {
            return Path.Combine(AppDataPath, GameId.ToString() + ".json");
        }

        public void SaveGame(Game game)
        {
            string filename = GetFilePath(game.GameId);
            File.WriteAllText(filename, game.SerializeGame());
        }

        public Game LoadGame(int id)
        {
            return Game.DeserializeGame(File.ReadAllText(GetFilePath(id)));
        }

        public void DeleteGame(int id)
        {
            File.Delete(GetFilePath(id));
        }

        public bool GameExist(int id)
        {
            return File.Exists(GetFilePath(id));
        }

        public int CleanupOldGames()
        {
            int counter = 0;
            foreach (var file in Directory.GetFiles(AppDataPath, "*.json"))
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.LastAccessTime.AddDays(1) < DateTime.Now)
                {
                    File.Delete(file);
                    counter++;
                }
            }
            return counter;
        }

    }
}


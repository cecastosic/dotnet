using ThirtyOne.Shared.Models;
using ThirtyOne.Web.Models;

namespace ThirtyOne.Web.Models
{
    public class GameViewModel
    {
        public Game CurrentGame { get; set; }

        public WebPlayer CurrentPlayer { get; set; }
    }
}
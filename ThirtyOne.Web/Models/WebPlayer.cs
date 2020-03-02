using System;
using ThirtyOne.Shared.Models;

namespace ThirtyOne.Web.Models
{
    public class WebPlayer : Player
    {
        public WebPlayer(string name) : base(name)
        {
            // It should inherit from the Player base class,
            // implement a constructor that takes a name and passes
            // it straight to the base class constructor
        }

        public override void Turn(Game game)
        {
            //Do nothing
        }

    }
}

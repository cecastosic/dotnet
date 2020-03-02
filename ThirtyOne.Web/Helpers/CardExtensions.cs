using ThirtyOne.Shared.Models;

namespace ThirtyOne.Web.Helpers
{
    public static class CardExtensions
    {
        public static string GetImageName(this Card card)
        {
            return $"{card.Rank}_of_{card.Suit.ToString().ToLower()}.png";
        }
    }
}

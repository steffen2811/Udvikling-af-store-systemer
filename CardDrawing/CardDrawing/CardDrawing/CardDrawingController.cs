using Microsoft.AspNetCore.Mvc;

namespace CardDrawing.CardDrawing
{
    [Route("/carddrawing")]
    public class CardDrawingController : Controller
    {
        public CardDrawing deck = new CardDrawing();
        [HttpGet("/")]
        public CardDrawing.Card Get()
        {
            CardDrawing.Card card = deck.GetRandomCard();
            return card;

        } 

    }
}

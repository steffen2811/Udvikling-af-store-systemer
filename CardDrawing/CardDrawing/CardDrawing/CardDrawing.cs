using System.Xml.Linq;
using static CardDrawing.CardDrawing.CardDrawing;

namespace CardDrawing.CardDrawing
{
    public class CardDrawing
    {
        private const int NO_OF_JOKERS = 3;

        private Card[] cards = new Card[52 + NO_OF_JOKERS];
        public CardDrawing() 
        {
            int idx = 0;
            foreach (CardValue value in (CardValue[])Enum.GetValues(typeof(CardValue)))
            {
                if (value == CardValue.Joker)
                {
                    for (int i = 0; i < NO_OF_JOKERS; i++) // Add jokers.
                    {
                        cards[idx++] = new Card(value, 0);
                    }
                }
                else
                {
                    foreach (Suit suit in (Suit[])Enum.GetValues(typeof(Suit)))
                    {
                        cards[idx++] = new Card(value, suit);
                    }
                }
            }
        }

        public Card GetRandomCard()
        {
            Random rnd = new Random();
            return cards[rnd.Next(cards.Length)];
        }

        public enum Suit
        {
            Clubs,
            Diamonds,
            Spades,
            Hearts,
        }

        public enum CardValue
        {
            Joker,
            Ace,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King
        }

        public record Card
        {
            public Card(CardValue value, Suit suit)
            {
                this.value = value;
                this.suit = suit;
            }
            private Suit suit { get; set; }
            private CardValue value { get; set; }
            public string cardValueName
            { 
                get { return ((CardValue)this.value).ToString(); }
            }
            public string cardSuitName
            {
                get 
                {
                    if (this.value == CardValue.Joker)
                        return "Irrelevant";
                    else
                        return ((Suit)this.suit).ToString(); 
                }
            }
        }
    }
}

namespace PlateRecognizerService.Controllers
{
    public class PlateRecognizer
    {
        public string plate { get; set; }
        public double score { get; set; }

        public PlateRecognizer(string plate, double score)
        {
            this.plate = plate;
            this.score = score;
        }
    }
}

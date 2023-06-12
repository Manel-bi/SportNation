namespace SportNation2.Data.Models
{
    public class CreateCompetitionViewModel
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<(int id, String Name)> Sports { get; set; }
    }
}

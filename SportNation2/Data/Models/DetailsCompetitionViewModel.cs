namespace SportNation2.Data.Models
{
    public class DetailsCompetitionViewModel
    {
        public int IdEvent { get; set; }
        public string Name { get; set; }
        public int MinimumAge { get; set; }
        public int MaximumAge { get; set; }
        //public string Genre { get; set; }
        public int MaxParticipants { get; set; }
        public int CompetitionId { get; set; }
    }
}

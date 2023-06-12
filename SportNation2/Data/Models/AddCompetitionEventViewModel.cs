using static SportNation2.Infrastructure.Enumerations;

namespace SportNation2.Data.Models
{
    public class AddCompetitionEventViewModel
    {
       
            public string Name { get; set; }
            public int MinAge { get; set; }
            public int MaxAge { get; set; }
            public int MaxParticipants { get; set; }
            public CompetitionGenre Genre { get; set; }
            public int CompetitionId { get; set; }
            public List<string> CompetitionEvents { get; set; } = new List<string>();
        
    }
}

using SportNation2.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportNation2.Services
{
    public interface ICompetitionService
    {

        /// <summary>
        /// Création de compétition
        /// </summary>
        /// <param name="sport">le sport choisi</param>
        /// <param name="startDate">date de la compétition</param>
        /// <returns>ID de la nouvelle compétition</returns>
        Task<int> CreateCompetitionAsync(int sportId, string name, DateTime startDate);
        
        Task CreateCompetitionEvent(int competitionId, AddCompetitionEventViewModel args);
        Task<List<string>> GetCompetitionEvents(int competitionId);
        Task<IEnumerable<CompetitionListViewModel>> GetNextCompetitionsAsync();
        Task<IEnumerable<CompetitionListViewModel>> GetNextBackCompetitionsAsync();

        Task<IEnumerable<DetailsCompetitionViewModel>> GetNextCompetitionsEvent(int competitionID);

        Task<List<(int Id, string Name)>> GetSportsAsync();
        Task ParticipateInCompetitionEvent(int eventId, int userId);
    }
}

    


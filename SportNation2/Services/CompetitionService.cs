using Microsoft.EntityFrameworkCore;
using SportNation2.Data;
using SportNation2.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

[assembly:InternalsVisibleTo("SportNation2.Tests")]
namespace SportNation2.Services
{
    public class CompetitionService : ICompetitionService
    {
        private readonly AppDbContext _dbContext;

        public Task<IEnumerable<DetailsCompetitionViewModel>> GetNextCompetitionEvents => throw new NotImplementedException();

        public CompetitionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task<int> CreateCompetitionAsync(int sportId , string name, DateTime startDate)
        {
            if (!CreateCompetitionDataIsValid(name, sportId, startDate))
            {
                throw new InvalidDataException("Données invalides");
            }



            var comp = new Competition()
            {
                Name = name,
                SportId = sportId,
                Date = startDate,
            };



            var entry = await _dbContext.AddAsync(comp);
            await _dbContext.SaveChangesAsync();



            return entry.Entity.Id;
        }



        internal bool CreateCompetitionDataIsValid(string name, int sportId, DateTime startDate)
        {
            //valider l'id du sport
            if (sportId <= 0)
            {
                return false;
            }
            if (!_dbContext.Sports.Any(s => s.Id == sportId))
            {
                return false;
            }
            //valider name
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            //valider startDate
            if (startDate < DateTime.Now.AddDays(1))
            {
                return false;
            }



            return true;
        }


        public async Task<List<string>> GetCompetitionEvents(int competitionId)
        {
            var competition = await _dbContext.Competitions.FindAsync(competitionId);
            

            var events = await _dbContext.CompetitionEvents
                .Where(e => e.CompetitionId == competitionId)
                .Select(e => e.Name)
                .ToListAsync();

            return events;
        }

      

        public async Task CreateCompetitionEvent(int competitionId, AddCompetitionEventViewModel competitionEventArguments)
        {
            var competition = await _dbContext.Competitions.FindAsync(competitionId);
          
            var competitionEvent = new CompetitionEvent()
            {
                Name = competitionEventArguments.Name,
                MinimumAge = competitionEventArguments.MinAge,
                MaximumAge = competitionEventArguments.MaxAge,
                Genre = competitionEventArguments.Genre,
                MaxParticipants = competitionEventArguments.MaxParticipants,
                CompetitionId = competitionId
            };

            await _dbContext.CompetitionEvents.AddAsync(competitionEvent);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<(int Id, string Name)>> GetSportsAsync()
        {
            var sports = await _dbContext.Sports
                .Select(s => new { s.Id, s.SportName })
                .ToListAsync();

            return sports.Select(s => (s.Id, s.SportName)).ToList();
        }

        public async Task<IEnumerable<CompetitionListViewModel>> GetNextCompetitionsAsync()
        {
            var competitions = await _dbContext.Competitions
        .Where(c => c.Date > DateTime.Now)
        .Select(c => new CompetitionListViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Date = c.Date
        })
        .ToListAsync();

            return competitions;
        }

        public async Task<IEnumerable<CompetitionListViewModel>> GetNextBackCompetitionsAsync()
        {
            var competitions = await _dbContext.Competitions
        .Where(c => c.Date < DateTime.Now)
        .Select(c => new CompetitionListViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Date = c.Date
        })
        .ToListAsync();

            return competitions;
        }

        public async Task<IEnumerable<DetailsCompetitionViewModel>> GetNextCompetitionsEvent(int competitionId)
        {
            var events = await _dbContext.CompetitionEvents
                .Where(e => e.CompetitionId == competitionId)
                .Select(e => new DetailsCompetitionViewModel
                {
                    IdEvent = e.Id,
                    Name = e.Name,
                    MinimumAge = e.MinimumAge,
                    MaximumAge = e.MaximumAge,
                    MaxParticipants = e.MaxParticipants
                })
                .ToListAsync();

            return events;
        }

        public async Task ParticipateInCompetitionEvent(int eventId, int userId)
        {
            var competitionEvent = await _dbContext.CompetitionEvents.FindAsync(eventId);
           // var user = await _dbContext.Users.FindAsync(userId);
            var user = 3;

            if (competitionEvent == null || user == null)
            {
                throw new InvalidOperationException("Événement de compétition ou utilisateur introuvable");
            }

            // nthabit si l'utilisateur est déjà inscrit à cet événement
            if (_dbContext.Participations.Any(p => p.CompetitionEventId == eventId && p.UserId == userId))
            {
                throw new InvalidOperationException("L'utilisateur est déjà inscrit à cet événement de compétition");
            }

            // namil participation w nhot user w el competitionevent
            var participation = new Participation
            {
                UserId = user,
                CompetitionEvent = competitionEvent
            };
            //nhot fi bd
            await _dbContext.Participations.AddAsync(participation);
            await _dbContext.SaveChangesAsync();
        }



    }


}

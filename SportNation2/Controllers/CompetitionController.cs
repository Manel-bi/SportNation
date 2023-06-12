using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportNation2.Data.Models;
using SportNation2.Services;
using System.Security.Claims;
using static SportNation2.Infrastructure.Enumerations;

namespace SportNation2.Controllers
{
    public class CompetitionController : Controller
    {
        private readonly ICompetitionService competitionService;
        

        public CompetitionController(ICompetitionService competitionService)
        {
            this.competitionService = competitionService;
          

        }



        public async Task<IActionResult> Index()
        {
            IEnumerable<CompetitionListViewModel> result =
                await competitionService.GetNextCompetitionsAsync();
            return View(result);
        }
        public async Task<IActionResult> BackCompetition()
        {
            IEnumerable<CompetitionListViewModel> result =
                await competitionService.GetNextBackCompetitionsAsync();
            return View(result);
        }



        //GET
        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create()
        {
            var model = new CreateCompetitionViewModel()
            {
                Date = DateTime.Now,
                Sports = await competitionService.GetSportsAsync(),
            };



            return View(model);
        }



        //POST
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Create(string name, DateTime date, int sport)
        {
            try
            {
                //creation de la compétition
                await competitionService.CreateCompetitionAsync(sport, name, date);



                //redirection vers la liste des compétitions
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                //méthode erreur 400
                //return BadRequest(e.Message);



                //méthode validation du formulaire
                ModelState.AddModelError("Name", e.Message); // erreur de validation du nom
                var model = new CreateCompetitionViewModel()
                {
                    Name = name,
                    Date = date,
                    Sports = await competitionService.GetSportsAsync(),
                };

                return View(model);

            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> CreateCompetitionEvent(int competitionId)
        {
            var model = new AddCompetitionEventViewModel
            {
                CompetitionId = competitionId
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> CreateCompetitionEvent(AddCompetitionEventViewModel model)
        {


            var args = new AddCompetitionEventViewModel()
            {
                Name = model.Name,
                MinAge = model.MinAge,
                MaxAge = model.MaxAge,
                MaxParticipants = model.MaxParticipants,
                Genre = model.Genre,
                CompetitionId = model.CompetitionId
            };

            await competitionService.CreateCompetitionEvent(model.CompetitionId, args);
            //model.CompetitionEvents = await competitionService.GetCompetitionEvents(model.CompetitionId);


            return RedirectToAction("Index");





        }
        [HttpGet]
        public async Task<IActionResult> DetailCompetionEvent(int competitionId)
        {
            IEnumerable<DetailsCompetitionViewModel> res =
                await competitionService.GetNextCompetitionsEvent(competitionId);
            return View( res);


        }
        [HttpGet]
        //[Authorize(Policy = "Basic")] 
        public async Task<IActionResult> Participate(int eventId)
        { // Nrécupéri el id 
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            int parseduserId = Convert.ToInt32(userId);


            // N3ayet el héthika bich yamel el lezem
            await competitionService.ParticipateInCompetitionEvent(eventId, parseduserId);


            return RedirectToAction("Index", "Competition");

        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwimSuite.Models;
using SwimSuite.Services;

namespace SwimSuite.Controllers;

[Authorize]
[Route("clubs/{clubId:guid}/trainers")]
public class TrainersController(ITrainerService trainerService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(Guid clubId, CancellationToken cancellationToken)
    {
        var list = await trainerService.GetListAsync(clubId, cancellationToken);

        if (list is null)
        {
            return NotFound();
        }

        return View(list);
    }

    [HttpGet("create")]
    public IActionResult Create(Guid clubId)
    {
        return View(new TrainerCreateViewModel { ClubId = clubId });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Guid clubId, TrainerCreateViewModel model, CancellationToken cancellationToken)
    {
        model.ClubId = clubId;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var trainer = await trainerService.CreateAsync(model, cancellationToken);

        if (trainer is null)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index), new { clubId });
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwimSuite.Models;
using SwimSuite.Services;

namespace SwimSuite.Controllers;

[Authorize]
public class ClubsController(IClubService clubService) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var clubs = await clubService.GetAllAsync(cancellationToken);
        return View(clubs);
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var club = await clubService.GetByIdAsync(id, cancellationToken);

        if (club is null)
        {
            return NotFound();
        }

        return View(club);
    }

    public IActionResult Create()
    {
        return View(new ClubCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ClubCreateViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var club = await clubService.CreateAsync(model, cancellationToken);

        return RedirectToAction(nameof(Details), new { id = club.Id });
    }
}

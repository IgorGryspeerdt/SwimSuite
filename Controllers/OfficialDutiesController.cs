using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SwimSuite.Data;
using SwimSuite.Models;
using SwimSuite.Services;

namespace SwimSuite.Controllers;

[Authorize]
[Route("clubs/{clubId:guid}/official-duties")]
public class OfficialDutiesController(
    IOfficialDutyService officialDutyService,
    ApplicationDbContext context) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(Guid clubId, CancellationToken cancellationToken)
    {
        var list = await officialDutyService.GetListAsync(clubId, cancellationToken);

        if (list is null)
        {
            return NotFound();
        }

        return View(list);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create(Guid clubId, CancellationToken cancellationToken)
    {
        if (!await ClubExistsAsync(clubId, cancellationToken))
        {
            return NotFound();
        }

        ViewData["Officials"] = await BuildOfficialOptionsAsync(clubId, cancellationToken);
        return View(new OfficialDutyCreateViewModel { ClubId = clubId });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Guid clubId, OfficialDutyCreateViewModel model, CancellationToken cancellationToken)
    {
        model.ClubId = clubId;

        if (model.OfficialId == Guid.Empty)
        {
            ModelState.AddModelError(nameof(model.OfficialId), "Choose an official.");
        }

        if (!ModelState.IsValid)
        {
            ViewData["Officials"] = await BuildOfficialOptionsAsync(clubId, cancellationToken);
            return View(model);
        }

        var duty = await officialDutyService.CreateAsync(model, cancellationToken);

        if (duty is null)
        {
            ModelState.AddModelError(nameof(model.OfficialId), "Choose an official that belongs to this club.");
            ViewData["Officials"] = await BuildOfficialOptionsAsync(clubId, cancellationToken);
            return View(model);
        }

        return RedirectToAction(nameof(Index), new { clubId });
    }

    private async Task<bool> ClubExistsAsync(Guid clubId, CancellationToken cancellationToken)
    {
        return await context.Clubs.AnyAsync(club => club.Id == clubId, cancellationToken);
    }

    private async Task<SelectList> BuildOfficialOptionsAsync(Guid clubId, CancellationToken cancellationToken)
    {
        var officials = await context.Officials
            .AsNoTracking()
            .Where(official => official.ClubId == clubId && official.IsActive)
            .OrderBy(official => official.LastName)
            .ThenBy(official => official.FirstName)
            .ToListAsync(cancellationToken);

        return new SelectList(officials, nameof(Official.Id), nameof(Official.DisplayName));
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwimSuite.Models;
using SwimSuite.Services;

namespace SwimSuite.Controllers;

[Authorize]
[Route("clubs/{clubId:guid}/officials")]
public class OfficialsController(IOfficialService officialService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(Guid clubId, CancellationToken cancellationToken)
    {
        var list = await officialService.GetListAsync(clubId, cancellationToken);

        if (list is null)
        {
            return NotFound();
        }

        return View(list);
    }

    [HttpGet("create")]
    public IActionResult Create(Guid clubId)
    {
        return View(new OfficialCreateViewModel { ClubId = clubId });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Guid clubId, OfficialCreateViewModel model, CancellationToken cancellationToken)
    {
        model.ClubId = clubId;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var official = await officialService.CreateAsync(model, cancellationToken);

        if (official is null)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index), new { clubId });
    }
}

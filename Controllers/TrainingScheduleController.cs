using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SwimSuite.Models;
using SwimSuite.Services;

namespace SwimSuite.Controllers;

[Authorize]
[Route("clubs/{clubId:guid}/training")]
public class TrainingScheduleController(ITrainingScheduleService trainingScheduleService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(Guid clubId, CancellationToken cancellationToken)
    {
        var schedule = await trainingScheduleService.GetScheduleAsync(clubId, cancellationToken);

        if (schedule is null)
        {
            return NotFound();
        }

        return View(schedule);
    }

    [HttpGet("groups/create")]
    public IActionResult CreateGroup(Guid clubId)
    {
        return View(new TrainingGroupCreateViewModel { ClubId = clubId });
    }

    [HttpPost("groups/create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateGroup(Guid clubId, TrainingGroupCreateViewModel model, CancellationToken cancellationToken)
    {
        model.ClubId = clubId;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var group = await trainingScheduleService.CreateGroupAsync(model, cancellationToken);

        if (group is null)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index), new { clubId });
    }

    [HttpGet("blocks/create")]
    public async Task<IActionResult> CreateBlock(Guid clubId, CancellationToken cancellationToken)
    {
        var schedule = await trainingScheduleService.GetScheduleAsync(clubId, cancellationToken);

        if (schedule is null)
        {
            return NotFound();
        }

        ViewData["TrainingGroups"] = BuildTrainingGroupOptions(schedule.TrainingGroups);

        return View(new TrainingBlockCreateViewModel { ClubId = clubId });
    }

    [HttpPost("blocks/create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateBlock(Guid clubId, TrainingBlockCreateViewModel model, CancellationToken cancellationToken)
    {
        model.ClubId = clubId;

        if (model.EndTime <= model.StartTime)
        {
            ModelState.AddModelError(nameof(model.EndTime), "End time must be later than start time.");
        }

        var schedule = await trainingScheduleService.GetScheduleAsync(clubId, cancellationToken);

        if (schedule is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewData["TrainingGroups"] = BuildTrainingGroupOptions(schedule.TrainingGroups);
            return View(model);
        }

        var block = await trainingScheduleService.CreateBlockAsync(model, cancellationToken);

        if (block is null)
        {
            ModelState.AddModelError(nameof(model.TrainingGroupId), "Choose a training group that belongs to this club.");
            ViewData["TrainingGroups"] = BuildTrainingGroupOptions(schedule.TrainingGroups);
            return View(model);
        }

        return RedirectToAction(nameof(Index), new { clubId });
    }

    private static SelectList BuildTrainingGroupOptions(IReadOnlyList<TrainingGroup> groups)
    {
        return new SelectList(groups, nameof(TrainingGroup.Id), nameof(TrainingGroup.Name));
    }
}

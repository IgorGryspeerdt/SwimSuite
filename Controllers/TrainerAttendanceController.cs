using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwimSuite.Models;
using SwimSuite.Services;

namespace SwimSuite.Controllers;

[Authorize]
[Route("clubs/{clubId:guid}/training/{trainingBlockId:guid}/attendance")]
public class TrainerAttendanceController(ITrainerAttendanceService attendanceService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Edit(Guid clubId, Guid trainingBlockId, CancellationToken cancellationToken)
    {
        var model = await attendanceService.GetForTrainingBlockAsync(clubId, trainingBlockId, cancellationToken);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid clubId, Guid trainingBlockId, TrainerAttendanceEditViewModel model, CancellationToken cancellationToken)
    {
        model.ClubId = clubId;
        model.TrainingBlockId = trainingBlockId;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var saved = await attendanceService.SaveAsync(model, cancellationToken);

        if (!saved)
        {
            return NotFound();
        }

        return RedirectToAction("Index", "TrainingSchedule", new { clubId });
    }
}

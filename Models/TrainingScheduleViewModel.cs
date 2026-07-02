namespace SwimSuite.Models;

public class TrainingScheduleViewModel
{
    public Club Club { get; set; } = new();

    public IReadOnlyList<TrainingGroup> TrainingGroups { get; set; } = [];

    public IReadOnlyList<TrainingBlock> TrainingBlocks { get; set; } = [];
}

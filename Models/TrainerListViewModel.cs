namespace SwimSuite.Models;

public class TrainerListViewModel
{
    public Club Club { get; set; } = new();

    public IReadOnlyList<Trainer> Trainers { get; set; } = [];
}

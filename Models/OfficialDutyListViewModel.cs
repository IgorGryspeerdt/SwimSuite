namespace SwimSuite.Models;

public class OfficialDutyListViewModel
{
    public Club Club { get; set; } = new();

    public IReadOnlyList<OfficialDuty> Duties { get; set; } = [];
}

namespace SwimSuite.Models;

public class OfficialListViewModel
{
    public Club Club { get; set; } = new();

    public IReadOnlyList<Official> Officials { get; set; } = [];
}

namespace Adherium.Domain.Treatments;

public class AdherenceCalculator
{
    private const int ExpectedDailyPuffs = 4;

    /// <summary>
    /// Care plan: 2 puffs, 2x per day = 4 puffs expected daily.
    /// Score = (total puffs / total expected) * 1000, capped at 1000.
    /// Each puff always increases the score.
    /// </summary>
    public static decimal CalculateScore(List<TreatmentEvent> puffInhaledEvents, DateTime today)
    {
        if (puffInhaledEvents.Count == 0)
            return 0m;

        var start = puffInhaledEvents.Min(e => e.Timestamp.Date);

        var totalTreatmentDays = (today.Date - start).Days + 1;

        var totalPuffsExpected = totalTreatmentDays * ExpectedDailyPuffs;
        var totalPuffs = puffInhaledEvents.Count;

        var score = (decimal)totalPuffs / totalPuffsExpected;

        return Math.Round(Math.Min(score, 1m) * 1000);
    }
}

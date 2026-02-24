using Adherium.Domain.Treatments;
using Shouldly;

namespace Adherium.Api.UnitTests.Domain;

public class AdherenceCalculatorTests
{
    private static readonly DateTime Today = new(2024, 1, 15, 12, 0, 0);

    private static TreatmentEvent CreatePuff(DateTime timestamp) =>
        new()
        {
            Id = Guid.NewGuid(),
            TreatmentId = Guid.NewGuid(),
            Timestamp = timestamp,
            Type = EventType.PuffInhaled,
        };

    private static List<TreatmentEvent> CreatePuffs(DateTime day, int count) =>
        Enumerable.Range(0, count)
            .Select(i => CreatePuff(day.AddMinutes(i)))
            .ToList();

    [Fact]
    public void CalculateScore_OnePuffOnFirstDay_Returns250()
    {
        // Arrange
        var events = CreatePuffs(Today, 1);

        // Act
        var score = AdherenceCalculator.CalculateScore(events, Today);

        // Assert
        score.ShouldBe(250); // 1/4 = 25%
    }

    [Fact]
    public void CalculateScore_FourPuffsOnFirstDay_Returns1000()
    {
        // Arrange
        var events = CreatePuffs(Today, 4);

        // Act
        var score = AdherenceCalculator.CalculateScore(events, Today);

        // Assert
        score.ShouldBe(1000); // 4/4 = 100%
    }

    [Fact]
    public void CalculateScore_MoreThanFourPuffs_CappedAt1000()
    {
        // Arrange
        var events = CreatePuffs(Today, 10);

        // Act
        var score = AdherenceCalculator.CalculateScore(events, Today);

        // Assert
        score.ShouldBe(1000);
    }

    [Fact]
    public void CalculateScore_PerfectAdherenceOverTwoDays_Returns1000()
    {
        // Arrange
        var day1 = Today;
        var day2 = Today.AddDays(1);
        var events = CreatePuffs(day1, 4).Concat(CreatePuffs(day2, 4)).ToList();

        // Act
        var score = AdherenceCalculator.CalculateScore(events, day2);

        // Assert
        score.ShouldBe(1000); // 8/8 = 100%
    }

    [Fact]
    public void CalculateScore_NoPuffsOnSecondDay_Returns500()
    {
        // Arrange
        var day1 = Today;
        var day2 = Today.AddDays(1);
        var events = CreatePuffs(day1, 4);

        // Act
        var score = AdherenceCalculator.CalculateScore(events, day2);

        // Assert
        score.ShouldBe(500); // 4/8 = 50%
    }

    [Fact]
    public void CalculateScore_MissedOneDay_ReflectsInScore()
    {
        // Arrange
        var monday = Today;
        var wednesday = Today.AddDays(2);
        var events = CreatePuffs(monday, 4).Concat(CreatePuffs(wednesday, 4)).ToList();

        // Act
        var score = AdherenceCalculator.CalculateScore(events, wednesday);

        // Assert
        score.ShouldBe(667); // 8/12 = 66.7%
    }

    [Fact]
    public void CalculateScore_EachPuffIncreasesScore()
    {
        // Arrange
        var events = new List<TreatmentEvent>();
        var previousScore = 0m;

        // Act & Assert
        for (int i = 1; i <= 4; i++)
        {
            events.Add(CreatePuff(Today.AddMinutes(i)));
            var currentScore = AdherenceCalculator.CalculateScore(events, Today);

            currentScore.ShouldBeGreaterThan(previousScore);
            previousScore = currentScore;
        }
    }

    [Fact]
    public void CalculateScore_EachPuffOnNewDayIncreasesScore()
    {
        // Arrange
        var day1 = Today;
        var day2 = Today.AddDays(1);
        var events = CreatePuffs(day1, 4);
        var previousScore = AdherenceCalculator.CalculateScore(events, day2);

        // Act & Assert
        for (int i = 1; i <= 4; i++)
        {
            events.Add(CreatePuff(day2.AddMinutes(i)));
            var currentScore = AdherenceCalculator.CalculateScore(events, day2);

            currentScore.ShouldBeGreaterThan(previousScore);
            previousScore = currentScore;
        }
    }
    
    [Fact]
    public void CalculateScore_NoEvents_ReturnsZero()
    {
        // Arrange
        var events = new List<TreatmentEvent>();

        // Act
        var score = AdherenceCalculator.CalculateScore(events, Today);

        // Assert
        score.ShouldBe(0);
    }
}

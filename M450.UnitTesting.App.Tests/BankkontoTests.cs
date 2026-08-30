using M450.UnitTesting.App;

namespace M450.UnitTesting.App.Tests;

public class BankkontoTests
{
    public static int BankTage(DateTime from, DateTime to)
    {
        int dayFrom = from.Day == 31 ? 30 : from.Day;
        int dayTo = (to.Day == 31 && dayFrom == 30) ? 30 : to.Day;
        
        return (to.Year - from.Year) * 360
            + (to.Month - from.Month) * 30
            + (dayTo - dayFrom);
    }
    
    [Fact]
    public void AccountFlow_YearEnd_CalculatesInterestCorrectly()
    {
        // Arrange
        var d1 = new DateTime(2024, 3, 1);
        var d2 = new DateTime(2024, 7, 1);
        var d3 = new DateTime(2024, 8, 1);
        var d4 = new DateTime(2024, 10, 1);
        var d5 = new DateTime(2024, 12, 31);

        Bankkonto.AktivZins = 0.036;
        Bankkonto.PassivZins = 0.072;
        var konto = new Bankkonto(0);

        // Act
        konto.ZahleEin(1000);
        konto.SchreibeZinsGut(BankTage(d1, d2)); // 120 Tage

        konto.ZahleEin(1000);
        konto.SchreibeZinsGut(BankTage(d2, d3)); // 30 Tage
        
        konto.Beziehe(3000);
        konto.SchreibeZinsGut(BankTage(d3, d4)); // 60 Tage
        
        konto.ZahleEin(2000);
        konto.SchreibeZinsGut(BankTage(d4, d5)); // 90 Tage
        
        konto.SchliesseKontoAb();

        // Assert
        Assert.Equal(1015.0, konto.Guthaben, precision: 4);
    }
}
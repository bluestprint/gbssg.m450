namespace M450.UnitTesting.App.Tests;

public sealed class KontoTests
{
    [Fact]
    public void Konstruktor_ErstelltKontenMitEindeutigenNummern()
    {
        // Arrange

        // Act
        var erstesKonto = NeuesKonto();
        var zweitesKonto = NeuesKonto();

        // Assert
        Assert.NotEqual(erstesKonto.KontoNummer, zweitesKonto.KontoNummer);
    }

    [Fact]
    public void ZahleEin_MitPositivemBetrag_ErhoehtGuthaben()
    {
        // Arrange
        var konto = NeuesKonto(startGuthaben: 100m);

        // Act
        konto.ZahleEin(25m);

        // Assert
        Assert.Equal(125m, konto.Guthaben);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void GeldOperationen_MitNichtPositivemBetrag_WerfenException(int betrag)
    {
        // Arrange
        var konto = NeuesKonto();

        // Act
        var einzahlenException = Record.Exception(() => konto.ZahleEin(betrag));
        var beziehenException = Record.Exception(() => konto.Beziehe(betrag));

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(einzahlenException);
        Assert.IsType<ArgumentOutOfRangeException>(beziehenException);
    }

    [Fact]
    public void SchreibeZinsGut_BeiPositivemGuthaben_VerwendetAktivZins()
    {
        // Arrange
        var konto = NeuesKonto(aktivZins: 3.6m, startGuthaben: 1_000m);

        // Act
        konto.SchreibeZinsGut(30);

        // Assert
        Assert.Equal(3m, konto.AufgelaufenerZins);
        Assert.Equal(1_000m, konto.Guthaben);
    }

    [Fact]
    public void SchreibeZinsGut_BeiNegativemGuthaben_VerwendetPassivZins()
    {
        // Arrange
        var konto = NeuesKonto(passivZins: 7.2m, startGuthaben: -1_000m);

        // Act
        konto.SchreibeZinsGut(30);

        // Assert
        Assert.Equal(-6m, konto.AufgelaufenerZins);
        Assert.Equal(-1_000m, konto.Guthaben);
    }

    [Fact]
    public void SchreibeZinsGut_Mehrmals_BerechnetKeinenZinseszins()
    {
        // Arrange
        var konto = NeuesKonto(aktivZins: 3.6m, startGuthaben: 1_000m);

        // Act
        konto.SchreibeZinsGut(30);
        konto.SchreibeZinsGut(30);

        // Assert
        Assert.Equal(6m, konto.AufgelaufenerZins);
        Assert.Equal(1_000m, konto.Guthaben);
    }

    [Fact]
    public void SchliesseKontoAb_VerbuchtenZinsUndSetztZinsspeicherZurueck()
    {
        // Arrange
        var konto = NeuesKonto(aktivZins: 3.6m, startGuthaben: 1_000m);
        konto.SchreibeZinsGut(30);

        // Act
        konto.SchliesseKontoAb();

        // Assert
        Assert.Equal(1_003m, konto.Guthaben);
        Assert.Equal(0m, konto.AufgelaufenerZins);
    }

    [Fact]
    public void Konstruktor_MitNegativemZinssatz_WirftException()
    {
        // Arrange

        // Act
        var aktivZinsException = Record.Exception(() => new Privatkonto(500m, aktivZins: -0.1m, passivZins: 5m));
        var passivZinsException = Record.Exception(() => new Privatkonto(500m, aktivZins: 1m, passivZins: -0.1m));

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(aktivZinsException);
        Assert.IsType<ArgumentOutOfRangeException>(passivZinsException);
    }

    [Fact]
    public void SchreibeZinsGut_MitNegativenTagen_WirftException()
    {
        // Arrange
        var konto = NeuesKonto();

        // Act
        var exception = Record.Exception(() => konto.SchreibeZinsGut(-1));

        // Assert
        Assert.IsType<ArgumentOutOfRangeException>(exception);
    }

    [Fact]
    public void Kontoabschluss_NachBuchungenImJahresverlauf_VerbuchtenGesamtzins()
    {
        // Arrange
        var konto = NeuesKonto(aktivZins: 3.6m, passivZins: 7.2m);

        // Act
        // 1. März: Konto eröffnen und CHF 1'000 einzahlen.
        konto.ZahleEin(1_000m);

        // 1. Juli: Zins für 120 Tage berechnen und CHF 1'000 einzahlen.
        konto.SchreibeZinsGut(120);
        konto.ZahleEin(1_000m);

        // 1. August: Zins für 30 Tage berechnen und CHF 3'000 beziehen.
        konto.SchreibeZinsGut(30);
        konto.Beziehe(3_000m);

        // 1. Oktober: Passivzins für 60 Tage berechnen und CHF 2'000 einzahlen.
        konto.SchreibeZinsGut(60);
        konto.ZahleEin(2_000m);

        // 31. Dezember: Zins für 90 Tage berechnen und das Konto abschliessen.
        konto.SchreibeZinsGut(90);
        konto.SchliesseKontoAb();

        // Assert
        // Buchungssaldo CHF 1'000 + Gesamtzins CHF 15 = CHF 1'015.
        Assert.Equal(1_015m, konto.Guthaben);
        Assert.Equal(0m, konto.AufgelaufenerZins);
    }

    // Konto ist abstrakt, daher wird hier stellvertretend ein Privatkonto mit einem
    // grosszügigen Maximalbetrag verwendet, damit dessen Überziehungsgrenze das
    // gemeinsame Basisverhalten (Zins, Kontoabschluss, Ein-/Auszahlung) nicht beeinflusst.
    private static Privatkonto NeuesKonto(
        decimal aktivZins = 1.5m,
        decimal passivZins = 5m,
        decimal startGuthaben = 0m)
    {
        return new Privatkonto(1_000_000m, aktivZins, passivZins, startGuthaben);
    }
}

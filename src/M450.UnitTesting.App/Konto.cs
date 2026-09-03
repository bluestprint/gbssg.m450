namespace M450.UnitTesting.App;

public abstract class Konto : IKonto
{
    public string KontoNummer { get; } = Guid.NewGuid().ToString("N");
    public decimal Guthaben { get; protected set; }

    public decimal AktivZins { get; }
    public decimal PassivZins { get; }
    public decimal AufgelaufenerZins;

    protected Konto(decimal aktivZins, decimal passivZins, decimal startGuthaben = 0)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(aktivZins);
        ArgumentOutOfRangeException.ThrowIfNegative(passivZins);
        AktivZins = aktivZins;
        PassivZins = passivZins;
        Guthaben = startGuthaben;
    }

    public decimal ZahleEin(decimal betrag)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(betrag);
        Guthaben += betrag;
        return Guthaben;
    }

    public abstract decimal Beziehe(decimal betrag);

    public decimal SchreibeZinsGut(int anzTage)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(anzTage);
        decimal zinssatz = Guthaben >= 0 ? AktivZins : PassivZins;
        AufgelaufenerZins += Guthaben * (zinssatz / 100) * anzTage / 360;
        return AufgelaufenerZins;
    }

    public void SchliesseKontoAb()
    {
        Guthaben += AufgelaufenerZins;
        AufgelaufenerZins = 0;
    }
}

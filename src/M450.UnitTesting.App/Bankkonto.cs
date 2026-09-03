namespace M450.UnitTesting.App;

using System;

public class Bankkonto
{
    public string KontoNummer { get; } = Guid.NewGuid().ToString("N");
    public decimal Guthaben { get; private set; }

    public decimal AktivZins { get; }
    public decimal PassivZins { get; }

    public decimal AufgelaufenerZins;

    public Bankkonto( decimal aktivZins, decimal passivZins, decimal guthaben = 0 )
    {
        ArgumentOutOfRangeException.ThrowIfNegative(aktivZins);
        ArgumentOutOfRangeException.ThrowIfNegative(passivZins);

        AktivZins = aktivZins;
        PassivZins = passivZins;
        Guthaben = guthaben;
    }

    public decimal ZahleEin(decimal betrag)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(betrag);

        Guthaben += betrag;
        return Guthaben;
    }

    public decimal Beziehe(decimal betrag)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(betrag);

        Guthaben -= betrag;
        return Guthaben;
    }

    public decimal Transferiere(Bankkonto konto, decimal betrag)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(betrag);
        if (konto == this)
        {
            throw new ArgumentException("Quell- und Zielkonto dürfen nicht identisch sein.", nameof(konto));
        }

        Guthaben -= betrag;
        konto.ZahleEin(betrag);
        return Guthaben;
    }

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
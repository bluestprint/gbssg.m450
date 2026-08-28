namespace M450.UnitTesting.App;

using System;

public class Bankkonto
{
    public string Kontonummer { get; } = Guid.NewGuid().ToString("N");
    public double Guthaben { get; private set; } = 0;

    public static double AktivZins { get; set; }
    public static double PassivZins { get; set; }

    public Bankkonto(double guthaben)
    {
        Guthaben = guthaben;
    }

    public double ZahleEin(double betrag)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(betrag);
        
        Guthaben += betrag;
        return Guthaben;
    }

    public double Beziehe(double betrag)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(betrag);

        if (betrag > Guthaben)
        {
            throw new InvalidOperationException("Zu wenig Guthaben auf dem Konto.");
        }
        
        Guthaben -= betrag;
        return Guthaben;
    }

    public double Transferiere(Bankkonto konto, double betrag)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(betrag);

        if (betrag > Guthaben)
        {
            throw new InvalidOperationException("Zu wenig Guthaben auf dem Konto.");
        }
        
        Guthaben -= betrag;
        konto.ZahleEin(betrag);
        return Guthaben;
    }

    public double SchreibeZinsGut(int anzTage)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(anzTage);
    
        double zins = Guthaben >= 0 ? AktivZins : PassivZins;
        double zinsBetrag = Guthaben * zins * anzTage / 360;
        Guthaben += zinsBetrag;
    
        return Guthaben;
    }

    public void SchliesseKontoAb()
    {
        if (Guthaben != 0)
        {
            throw new InvalidOperationException("Konto kann nur mit 0 geschlossen werden.");
        }
        
        Guthaben = double.NaN;
    }
}
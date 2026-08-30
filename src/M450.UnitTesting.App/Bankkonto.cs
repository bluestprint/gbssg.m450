namespace M450.UnitTesting.App;

using System;

public class Bankkonto
{
    public string Kontonummer { get; } = Guid.NewGuid().ToString("N");
    public double Guthaben { get; private set; } = 0;

    public static double AktivZins { get; set; }
    public static double PassivZins { get; set; }

    private double aufgelaufenerZins = 0;

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
        
        Guthaben -= betrag;
        return Guthaben;
    }

    public double Transferiere(Bankkonto konto, double betrag)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(betrag);
        
        Guthaben -= betrag;
        konto.ZahleEin(betrag);
        return Guthaben;
    }

    public double SchreibeZinsGut(int anzTage)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(anzTage);

        double zinssatz = Guthaben >= 0 ? AktivZins : PassivZins;
        aufgelaufenerZins += Guthaben * zinssatz * anzTage / 360;
        
        return aufgelaufenerZins;
    }

    public void SchliesseKontoAb()
    {
        Guthaben += aufgelaufenerZins;
        aufgelaufenerZins = 0;
    }
}
namespace Attivita.Cli;

public class AttivitaRicorrente : Attivita
{
    /// <summary>
    /// 
    /// </summary>
    private TimeSpan Intervallo { get; }

    #region Ctor & Destructor
    /// <summary>
    /// Standard Constructor
    /// </summary>
    public AttivitaRicorrente(string titolo, TimeSpan intervallo, IOrologio? orologio = null)
        : base(titolo, orologio)
    {
        if (intervallo <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(intervallo), "L'intervallo deve essere positivo.");
        }
        Intervallo = intervallo;
    }
    #endregion

    public override void Completa()
    {
        if (Scadenza.HasValue)
        {
            Scadenza = Scadenza.Value + Intervallo;
        }
        else
        {
            Scadenza = Adesso + Intervallo;

        }
    }
}

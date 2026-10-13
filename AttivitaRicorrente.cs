namespace Attivita.Cli;

public class AttivitaRicorrente : Attivita
{
    private readonly TimeSpan _intervallo;

    /// <summary>
    /// 
    /// </summary>
    public TimeSpan Intervallo => _intervallo;

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
        _intervallo = intervallo;
    }

    private AttivitaRicorrente(Guid id, String titolo, Priorita priorita, DateTime? scadenza, DateTime creataIl, TimeSpan intervallo, Boolean completata, IOrologio? orologio)
    : base(id, titolo, priorita, scadenza, creataIl, completata, orologio)
    {
        if (intervallo <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(Intervallo), "L'intervallo deve essere positivo.");
        }
        _intervallo = intervallo;
    }
    #endregion

    internal static AttivitaRicorrente Ripristina(Guid id, String titolo, Priorita priorita, DateTime? scadenza, DateTime creataIl, TimeSpan intervallo, Boolean completata, IOrologio? orologio = null)
    {
        return new AttivitaRicorrente(id, titolo, priorita, scadenza, creataIl, intervallo, completata, orologio);
    }

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

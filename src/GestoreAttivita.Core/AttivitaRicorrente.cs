namespace GestoreAttivita.Core;

/// <summary>
/// Attività che si ripete: completarla non la chiude, ma ne sposta la scadenza in avanti.
/// </summary>
public class AttivitaRicorrente : Attivita
{
    private readonly int _intervallo;

    /// <summary>
    public int Intervallo => _intervallo;

    #region Costruttori
    /// <summary>
    /// Crea un attivita ricorrente nuova, con la data di creazione presa dall'orologio
    /// </summary>
    public AttivitaRicorrente(String titolo, int intervallo, Priorita? priorita = null, IOrologio? orologio = null)
    : base(titolo, orologio, priorita)
    {
        _intervallo = Valida(intervallo);
    }

    private AttivitaRicorrente(Guid id, String titolo, Priorita priorita, DateOnly? scadenza, DateTimeOffset creataIl, int intervallo, Boolean completata, IOrologio? orologio = null)
    : base(id, titolo, priorita, scadenza, creataIl, completata, orologio)
    {
        _intervallo = Valida(intervallo);
    }
    #endregion

    internal static AttivitaRicorrente Ripristina(Guid id, String titolo, Priorita priorita, DateOnly? scadenza, DateTimeOffset creataIl, int intervallo, Boolean completata, IOrologio? orologio = null)
    {
        return new AttivitaRicorrente(id, titolo, priorita, scadenza, creataIl, intervallo, completata, orologio);
    }

    public override void Completa()
    {
        var nuova = Scadenza ?? Oggi;
        do { nuova = nuova.AddDays(Intervallo); } while (nuova <= Oggi);
        Scadenza = nuova;
    }

    private static int Valida(int giorni) => giorni > 0 ? giorni : throw new ArgumentOutOfRangeException(nameof(giorni), "L'intervallo deve essere positivo.");
}

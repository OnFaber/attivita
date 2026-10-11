namespace Attivita.Cli;

/// <summary>
/// Un'attività da svolgere, con titolo, priorità e una scadenza facoltativa.
/// </summary>
/// <remarks>
/// L'attività protegge il proprio stato: titolo e data di creazione non cambiano
/// dopo la costruzione, e si segna come completata solo tramite <see cref="Completa"/>.
/// Il tempo corrente arriva da un <see cref="IOrologio"/>, così nei test si può
/// simulare il passare dei giorni.
/// </remarks>
public class Attivita
{
    /// <summary>Titolo, sempre non vuoto (garantito dal costruttore).</summary>
    private readonly String _titolo;

    /// <summary>Stato di completamento; cambia solo tramite <see cref="Completa"/>.</summary>
    private Boolean _completata;

    /// <summary>Istante di creazione, letto dall'orologio nel costruttore.</summary>
    private readonly DateTime _creataIl;

    /// <summary>Scadenza; <c>null</c> se l'attività non ne ha una.</summary>
    private DateTime? _scadenza;

    /// <summary>Fonte dell'ora corrente.</summary>
    private readonly IOrologio _orologio;

    /// <summary>
    /// Ora corrente secondo l'orologio dell'attività.
    /// </summary>
    /// <remarks>
    /// È <c>protected</c> perché le sottoclassi (ad esempio <see cref="AttivitaRicorrente"/>)
    /// possano leggere l'ora senza che l'orologio diventi pubblico.
    /// </remarks>
    protected DateTime Adesso => _orologio.Adesso;

    #region Ctor & Destructor
    /// <summary>
    /// Crea una nuova attività non completata e senza scadenza.
    /// </summary>
    /// <param name="titolo">Il titolo dell'attività; non può essere vuoto.</param>
    /// <param name="orologio">
    /// L'orologio da usare; se <c>null</c> viene usato l'orologio di sistema.
    /// Nei test passa un <see cref="OrologioFisso"/>.
    /// </param>
    /// <param name="priorita">La priorità iniziale.</param>
    /// <exception cref="ArgumentException">Se <paramref name="titolo"/> è vuoto o composto solo da spazi.</exception>
    public Attivita(string titolo, IOrologio? orologio = null, Priorita priorita = Priorita.Media)
    {
        if (string.IsNullOrWhiteSpace(titolo))
        {
            throw new ArgumentException("Il titolo non può essere vuoto.", nameof(titolo));
        }

        _titolo = titolo;
        Priorita = priorita;
        _orologio = orologio ?? new OrologioDiSistema();
        _creataIl = this._orologio.Adesso;
    }
    #endregion

    /// <summary>
    /// Il titolo dell'attività. Non cambia dopo la creazione.
    /// </summary>
    public String Titolo => this._titolo;

    /// <summary>
    /// Il momento in cui l'attività è stata creata, secondo il suo orologio.
    /// </summary>
    public DateTime CreataIl => this._creataIl;

    /// <summary>
    /// Indica se l'attività è stata completata.
    /// </summary>
    /// <remarks>
    /// È in sola lettura: per completare l'attività usa <see cref="Completa"/>.
    /// </remarks>
    public Boolean Completata => this._completata;

    /// <summary>
    /// La priorità dell'attività. Si può cambiare in qualsiasi momento.
    /// </summary>
    public Priorita Priorita { get; set; }

    /// <summary>
    /// La scadenza dell'attività, oppure <c>null</c> se non ne ha una.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Se si imposta una data precedente a <see cref="CreataIl"/>.
    /// </exception>
    public DateTime? Scadenza
    {
        get => _scadenza;
        set
        {
            if (value.HasValue && (value < this.CreataIl))
            {
                throw new ArgumentException(
                    "La scadenza non può essere precedente alla data di creazione."
                );
            }
            this._scadenza = value;
        }
    }

    /// <summary>
    /// Segna l'attività come completata.
    /// </summary>
    /// <remarks>
    /// Le sottoclassi possono ridefinire questo comportamento: ad esempio
    /// <see cref="AttivitaRicorrente"/> non chiude l'attività ma ne sposta la scadenza.
    /// </remarks>
    public virtual void Completa()
    {
        this._completata = true;
    }

    /// <summary>
    /// Indica se l'attività ha superato la scadenza senza essere stata completata.
    /// </summary>
    /// <returns>
    /// <c>true</c> se c'è una scadenza, è già passata e l'attività non è completata;
    /// <c>false</c> negli altri casi, compreso quando non è impostata una scadenza.
    /// </returns>
    public Boolean EScaduta() => this._scadenza < this._orologio.Adesso && !this._completata;
}

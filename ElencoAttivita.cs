namespace Attivita.Cli;

public class ElencoAttivita
{

    /// <summary>
    /// Description
    /// </summary>
    private readonly List<Attivita> _attivita = [];

    /// <summary>
    /// Description
    /// </summary>
    /// <value><c>Attivita</c></value>
    public IReadOnlyList<Attivita> Tutte => _attivita;

    public void Aggiungi(Attivita attivita)
    {
        ArgumentNullException.ThrowIfNull(attivita);
        if (_attivita.Contains(attivita))
        {
            throw new InvalidOperationException($"l'attività \"{attivita.Titolo}\" è già nell'elenco");
        }
        this._attivita.Add(attivita);
    }

    public IEnumerable<Attivita> Scadute()
    {
        return this._attivita.Where(a => a.EScaduta()).OrderByDescending(a => a.Priorita);
    }

    public IEnumerable<Attivita> DaFare()
    {
        return this._attivita.Where(a => !a.Completata).OrderBy(a => a.Scadenza == null).ThenBy(a => a.Scadenza);
    }
}

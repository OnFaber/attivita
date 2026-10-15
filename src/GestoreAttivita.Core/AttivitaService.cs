namespace GestoreAttivita.Core;

public class AttivitaService
{
    private readonly IArchivio _archivio;
    private readonly ElencoAttivita _elenco;

    public AttivitaService(IArchivio archivio)
    {
        _archivio = archivio;
        _elenco = new ElencoAttivita();

        foreach (var attivita in archivio.Carica())
        {
            _elenco.Aggiungi(attivita);
        }
    }

    public void Aggiungi(Attivita a)
    {
        _elenco.Aggiungi(a);
        _archivio.Salva(_elenco.Tutte);
    }

    public Attivita Completa(string prefisso)
    {
        var trovate = _elenco.TrovaPerPrefisso(prefisso);

        switch (trovate.Count)
        {
            case 0:
                throw new InvalidOperationException($"Nessuna attvità con codice \"{prefisso}\" .");
            case > 1:
                throw new InvalidOperationException($"Il codice \"{prefisso}\" corrisponde a {trovate.Count} attività: scrivi qualche carattere in più.");
        }
        var attivita = trovate.First();
        attivita.Completa();
        _archivio.Salva(_elenco.Tutte);
        return attivita;
    }

    public IEnumerable<Attivita> DaFare() => _elenco.DaFare();
}

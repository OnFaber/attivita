namespace Attivita.Cli;

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

    public void Completa(int numero)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Attivita> DaFare() => _elenco.DaFare();
}

namespace Attivita.Cli;

public interface IArchivio
{
    void Salva(IEnumerable<Attivita> attivita);
    IReadOnlyList<Attivita> Carica();
}

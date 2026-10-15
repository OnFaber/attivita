namespace GestoreAttivita.Core;

public interface IArchivio
{
    void Salva(IEnumerable<Attivita> attivita);
    IReadOnlyList<Attivita> Carica();
}

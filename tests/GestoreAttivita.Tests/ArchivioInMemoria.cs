using GestoreAttivita.Core;

namespace GestoreAttivita.Tests;

public class ArchivioInMemoria : IArchivio
{
    private List<Attivita> _dati;

    public int Salvataggi { get; private set; }

    public ArchivioInMemoria(params Attivita[] iniziali) => _dati = [.. iniziali];

    public IReadOnlyList<Attivita> Carica() => _dati;

    public void Salva(IEnumerable<Attivita> attivita)
    {
        _dati = attivita.ToList();
        Salvataggi++;
    }
}

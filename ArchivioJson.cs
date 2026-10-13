using System.Text;
using System.Text.Json;

namespace Attivita.Cli;

public class ArchivioJson : IArchivio
{
    private readonly String _percorso;
    private readonly IOrologio? _orologio;

    public ArchivioJson(String percorso, IOrologio? orologio = null)
    {
        _percorso = percorso;
        _orologio = orologio;
    }

    public IReadOnlyList<Attivita> Carica()
    {
        if (!File.Exists(_percorso))
        {
            return [];
        }
        String json = File.ReadAllText(this._percorso);
        var caricate =
            JsonSerializer.Deserialize<List<AttivitaSalvata>>(json) ?? new List<AttivitaSalvata>();
        var risultato = new List<Attivita>();
        foreach (var a in caricate)
        {
            Priorita priorita = Enum.Parse<Priorita>(a.Priorita);

            if (a.Tipo == "ricorrente")
            {
                if (a.IntervalloGiorni is null)
                {
                    throw new InvalidDataException("Intervallo mancante per attività ricorrente.");
                }
                risultato.Add(
                    AttivitaRicorrente.Ripristina(
                        a.id,
                        a.Titolo,
                        priorita,
                        a.Scadenza,
                        a.CreataIl,
                        TimeSpan.FromDays(a.IntervalloGiorni.Value),
                        a.Completata,
                        _orologio
                    )
                );
            }
            else
            {
                risultato.Add(
                    Attivita.Ripristina(
                        a.id,
                        a.Titolo,
                        priorita,
                        a.Scadenza,
                        a.CreataIl,
                        a.Completata,
                        _orologio
                    )
                );
            }
        }
        return risultato;
    }

    public void Salva(IEnumerable<Attivita> attivita)
    {
        String fileName = this._percorso;
        var options = new JsonSerializerOptions { WriteIndented = true };

        var attivitaSalvate = attivita
            .Select(a =>
            {
                String tipo = "normale";
                int? intervalloGiorni = null;

                if (a is AttivitaRicorrente attivitaRicorrente)
                {
                    tipo = "ricorrente";
                    intervalloGiorni = (int?)attivitaRicorrente.Intervallo.TotalDays;
                }

                return new AttivitaSalvata(
                    a.Id,
                    a.Titolo,
                    tipo,
                    a.Completata,
                    a.CreataIl,
                    a.Scadenza,
                    a.Priorita.ToString(),
                    intervalloGiorni
                );
            })
            .ToList();

        String jsonString = JsonSerializer.Serialize(attivitaSalvate, options);
        File.WriteAllText(fileName, jsonString);
    }
}

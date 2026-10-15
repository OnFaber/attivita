using System.Text;
using System.Text.Json;

namespace Attivita.Cli;

public class ArchivioJson : IArchivio
{
    private const string TipoNormale = "normale";
    private const string TipoRicorrente = "ricorrente";

    private static readonly JsonSerializerOptions Opzioni = new() { WriteIndented = true };

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

        List<AttivitaSalvata> caricate;

        try
        {
            var json = File.ReadAllText(this._percorso);
            caricate =
                JsonSerializer.Deserialize<List<AttivitaSalvata>>(json)
                ?? new List<AttivitaSalvata>();
        }
        catch (JsonException e)
        {
            throw new InvalidDataException($"Il file {_percorso} non è un JSON valido.", e);
        }

        return caricate.Select(Ricostruisci).ToList();
    }

    private Attivita Ricostruisci(AttivitaSalvata a)
    {
        if (!Enum.TryParse<Priorita>(a.Priorita, out var priorita))
        {
            throw new InvalidDataException(
                $"Priorità {a.Priorita} non valida per l'attività {a.Id}."
            );
        }

        return a.Tipo switch
        {
            TipoRicorrente => AttivitaRicorrente.Ripristina(
                a.Id,
                a.Titolo,
                priorita,
                a.Scadenza,
                a.CreataIl,
                a.IntervalloGiorni
                    ?? throw new InvalidDataException(
                        $"Intervallo mancante per l'attività ricorrente {a.Id}"
                    ),
                a.Completata,
                _orologio
            ),
            TipoNormale => Attivita.Ripristina(
                a.Id,
                a.Titolo,
                priorita,
                a.Scadenza,
                a.CreataIl,
                a.Completata,
                _orologio
            ),
            _ => throw new InvalidDataException(
                $"Tipo '{a.Tipo}' sconosciuto per l'attività {a.Id}"
            ),
        };
    }

    public void Salva(IEnumerable<Attivita> attivita)
    {
        var attivitaSalvate = attivita.Select(InSalvata).ToList();
        var jsonString = JsonSerializer.Serialize(attivitaSalvate, Opzioni);

        var tmp = _percorso + ".tmp";
        File.WriteAllText(tmp, jsonString);
        File.Move(tmp, _percorso, overwrite: true);
    }

    private static AttivitaSalvata InSalvata(Attivita a) =>
        a switch
        {
            AttivitaRicorrente r => new AttivitaSalvata(
                r.Id,
                r.Titolo,
                TipoRicorrente,
                r.Completata,
                r.CreataIl,
                r.Scadenza,
                r.Priorita.ToString(),
                r.Intervallo
            ),
            _ => new(
                a.Id,
                a.Titolo,
                TipoNormale,
                a.Completata,
                a.CreataIl,
                a.Scadenza,
                a.Priorita.ToString(),
                null
            ),
        };
}

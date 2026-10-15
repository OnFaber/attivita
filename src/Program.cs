using System.CommandLine;

namespace Attivita.Cli;

public class Program
{
    static int Main(string[] args)
    {
        Argument<String> titoloArgument = new("titolo")
        {
            Description = "Il titolo dell'attività"
        };
        Argument<String> codiceArgument = new("codice")
        {
            Description = "Il codice dell'attività"
        };

        Option<Priorita?> prioritaOption = new("--priorita")
        {
            Description = "Priorità: Bassa, Media, Alta, Urgente"
        };


        RootCommand rootCommand = new("Applicazione per tenere traccia delle attività");
        Command mainCommand = new("attivita", "Lavora con le attività.");
        Command listaCommand = new("lista", "Restituisce tutte le attività.");
        Command aggiungiCommand = new("aggiungi", "Aggiungi un attivita alla lista");
        Command completaCommand = new("completa", "Segna un'attivita come svolta");

        String percorso = Path.Combine(
                              Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                              "attivita",
                              "archivio.json"
                          );

        Directory.CreateDirectory(Path.GetDirectoryName(percorso)!);
        var servizio = new AttivitaService(new ArchivioJson(percorso));

        rootCommand.Subcommands.Add(mainCommand);
        mainCommand.Subcommands.Add(listaCommand);
        mainCommand.Subcommands.Add(aggiungiCommand);
        mainCommand.Subcommands.Add(completaCommand);

        aggiungiCommand.Arguments.Add(titoloArgument);
        aggiungiCommand.Options.Add(prioritaOption);

        aggiungiCommand.Aliases.Add("insert");

        listaCommand.SetAction(parseResult => Lista(servizio));
        aggiungiCommand.SetAction(parseResult =>
        {
            var a = AggiungiAttivita(
                        servizio,
                        parseResult.GetRequiredValue(titoloArgument),
                        parseResult.GetValue(prioritaOption)
                    );
            Console.WriteLine($"Aggiunta: {a.Id.ToString().Substring(0, 6)} {a.Titolo} [{a.Priorita}]");
        });

        completaCommand.Arguments.Add(codiceArgument);
        completaCommand.SetAction(parseResult =>
        {
            var a = servizio.Completa(
                        parseResult.GetRequiredValue(codiceArgument)
                    );
            Console.WriteLine(a.Completata
                              ? $"Completata: {a.Titolo}"
                              : $"{a.Titolo}: prossima scadenza {a.Scadenza:d}");
        });

        try
        {
            return rootCommand
                   .Parse(args)
                   .Invoke(new InvocationConfiguration { EnableDefaultExceptionHandler = false });
        }
        catch (Exception e) when (e is InvalidOperationException or ArgumentException)
        {
            Console.Error.WriteLine($"Errore: {e.Message}");
            return 1;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Errore imprevisto: {e.Message}");
            return 2;
        }
    }

    internal static Attivita AggiungiAttivita(
        AttivitaService servizio,
        String titolo,
        Priorita? priorita
    )
    {
        var attivita = new Attivita(titolo, priorita: priorita);
        servizio.Aggiungi(attivita);
        return attivita;
    }

    internal static void Lista(AttivitaService servizio)
    {
        foreach (var a in servizio.DaFare())
        {
            String codice = a.Id.ToString().Substring(0, 6);
            var scadenza = a.Scadenza?.ToString("d") ?? "--";
            var avviso = a.EScaduta() ? " SCADUTA" : "";
            Console.WriteLine($"{codice} [{a.Priorita}] {a.Titolo} (scade: {scadenza}){avviso}");
        }
    }
}

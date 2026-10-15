using System.CommandLine;

namespace Attivita.Cli;

public class Program
{
    internal static void AggiungiAttivita(
        AttivitaService servizio,
        String titolo,
        Priorita? priorita
    )
    {
        var attivita = new Attivita(titolo, priorita: priorita);
        servizio.Aggiungi(attivita);
    }

    internal static void Lista(AttivitaService servizio)
    {
        foreach (var a in servizio.DaFare())
        {
            String codice = a.Id.ToString().Substring(0, 6);
            var scadenza = a.Scadenza?.ToString("d") ?? "--";
            var avviso = a.EScaduta() ? " scaduta" : "";
            Console.WriteLine($"{codice} [{a.Priorita}] {a.Titolo} (scade: {scadenza}) {avviso}");
        }
    }

    static int Main(string[] args)
    {
        Argument<string> titoloArgument = new("titolo") { Description = "Il titolo dell'attività" };

        Option<Priorita?> prioritaOption = new("--priorita");

        RootCommand rootCommand = new("Applicazione per tenere traccia delle attività");
        Command mainCommand = new("attivita", "Lavora con le attività.");
        Command readCommand = new("lista", "Restituisce tutte le attività.");
        Command aggiungiCommand = new("aggiungi", "Aggiungi un attivita alla lista");
        Command completaCommand = new("completa", "Marca una attivita come completata");

        String percorso = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "attivita",
            "archivio.json"
        );

        Directory.CreateDirectory(Path.GetDirectoryName(percorso)!);
        var servizio = new AttivitaService(new ArchivioJson(percorso));

        rootCommand.Subcommands.Add(mainCommand);
        mainCommand.Subcommands.Add(readCommand);
        mainCommand.Subcommands.Add(aggiungiCommand);
        mainCommand.Subcommands.Add(completaCommand);

        aggiungiCommand.Arguments.Add(titoloArgument);
        aggiungiCommand.Options.Add(prioritaOption);

        aggiungiCommand.Aliases.Add("insert");

        readCommand.SetAction(parseResult => Lista(servizio));
        aggiungiCommand.SetAction(parseResult =>
            AggiungiAttivita(
                servizio,
                parseResult.GetRequiredValue(titoloArgument),
                parseResult.GetValue(prioritaOption)
            )
        );

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
}

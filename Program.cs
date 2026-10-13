namespace Attivita.Cli;

class Program
{
    static void Main(string[] args)
    {
        var orologio = new OrologioFisso(new DateTime(2026, 10, 10, 9, 0, 0));
        var elenco = new ElencoAttivita();

        var report = new Attivita("Consegnare il report", orologio, Priorita.Bassa) { Scadenza = new DateTime(2026, 10, 12) };
        var spazzatura = new Attivita("Buttare la spazzatura", orologio, Priorita.Alta) { Scadenza = new DateTime(2026, 10, 13) };
        var regalo = new Attivita("Dare i soldi per il regalo", orologio, Priorita.Media) { Scadenza = new DateTime(2026, 10, 13) };
        var ricorrente = new AttivitaRicorrente("Pagare l'affitto", TimeSpan.FromDays(30), orologio) { Scadenza = new DateTime(2026, 11, 1) };

        var archivioJson = new ArchivioJson("./archivio.json", orologio);
        var caricate = archivioJson.Carica();
        Console.WriteLine($"Trovate {caricate.Count}");

        ricorrente.Completa();
        report.Completa();



        var libro = new Attivita("Leggere un libro", orologio, Priorita.Media);
        elenco.Aggiungi(report);
        elenco.Aggiungi(spazzatura);
        elenco.Aggiungi(regalo);
        elenco.Aggiungi(libro);

        var tutte = new List<Attivita> { report, ricorrente, libro, spazzatura };

        archivioJson.Salva(tutte);

        void Stampa(string titolo, IEnumerable<Attivita> attivita)
        {
            Console.WriteLine($"--- {titolo} ---");
            foreach (var item in attivita)
            {
                Console.WriteLine(
                    $" [{item.Priorita}] {item.Titolo} (scade: {item.Scadenza?.ToString("d") ?? "nessuna"})"
                );
            }
        }

        Stampa("Da fare", elenco.DaFare());
        Stampa("Scadute", elenco.Scadute());

        orologio.Avanza(TimeSpan.FromDays(3));
        Stampa("Scadute dopo 3 giorni", elenco.Scadute());
        Stampa("Caricate scadute dopo 3 giorni", caricate.Where(a => a.EScaduta()));

        foreach (var a in caricate)
        {
            Console.WriteLine(
                $"{a.Titolo} -> {a.GetType().Name}, completata: {a.Completata}"
            );
        }

        Stampa("Attivita caricate dal JSON", caricate);
    }
}

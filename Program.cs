namespace Attivita.Cli;


class Program
{
    static void Main(string[] args)
    {
        var orologio = new OrologioFisso(new DateTime(2026, 10, 10, 9, 0, 0));
        var elenco = new ElencoAttivita();

        var report = new Attivita("Consegnare il report", orologio, Priorita.Bassa) { Scadenza = new DateTime(2026, 10, 12) };
        var spesa = new Attivita("Buttare la spazzatura", orologio, Priorita.Alta) { Scadenza = new DateTime(2026, 10, 13) };
        var regalo = new Attivita("Dare i soldi per il regalo", orologio, Priorita.Media) { Scadenza = new DateTime(2026, 10, 13) };

        var libro = new Attivita("Leggere un libro", orologio, Priorita.Media);

        elenco.Aggiungi(report);
        elenco.Aggiungi(spesa);
        elenco.Aggiungi(regalo);
        elenco.Aggiungi(libro);


        /// <summary>
        /// Description
        /// </summary>
        /// <returns><c></c></returns>
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
    }
}

namespace Attivita.Cli;

public record AttivitaSalvata(
    Guid id,
    String Titolo,
    String Tipo,
    Boolean Completata,
    DateTime CreataIl,
    DateTime? Scadenza,
    String Priorita,
    int? IntervalloGiorni
);

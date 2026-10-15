namespace Attivita.Cli;

public record AttivitaSalvata(
    Guid Id,
    String Titolo,
    String Tipo,
    Boolean Completata,
    DateTimeOffset CreataIl,
    DateOnly? Scadenza,
    String Priorita,
    int? IntervalloGiorni
);

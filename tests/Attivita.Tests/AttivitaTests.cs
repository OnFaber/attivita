namespace Attivita.Cli.Tests;

public class AttivitaTests
{
    private static OrologioFisso Orologio(int anno, int mese, int giorno) =>
        new(new DateTimeOffset(anno, mese, giorno, 9, 0, 0, TimeSpan.Zero));

    [Fact]
    public void Costruttore_SenzaPriorita_UseMedia()
    {
        var attivita = new Attivita("Test");
        Assert.Equal(Priorita.Media, attivita.Priorita);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Costruttore_TitoloVuoto_LanciaArgumentException(string titolo)
    {
        Assert.Throws<ArgumentException>(() => new Attivita(titolo));
    }

    [Fact]
    public void EScaduta_DopoLaScadenza_RestituisceTrue()
    {
        // Arrange
        var orologio = Orologio(2026, 10, 1);
        var attivita = new Attivita("Test", orologio) { Scadenza = new DateOnly(2026, 10, 10) };

        // Act
        orologio.Avanza(TimeSpan.FromDays(10));

        // Assert
        Assert.True(attivita.EScaduta());
    }
}

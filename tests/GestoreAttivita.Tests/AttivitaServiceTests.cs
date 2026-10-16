using GestoreAttivita.Core;
namespace GestoreAttivita.Tests;

public class AttivitaServiceTests
{

    [Fact]
    public void Completa_CodiceInesistente_LanciaInvalidOperationException()
    {
        // Arrange: un archivio con una sola attività e un service che lo usa
        var codice = "zzzzzz";
        Attivita attivita = new Attivita("Test");
        AttivitaService service = new AttivitaService(new ArchivioInMemoria(attivita));

        // Act + Assert: completare un codice che non esiste deve lanciare l'eccezione
        Assert.Throws<InvalidOperationException>(() => service.Completa(codice));
    }

    [Fact]
    public void Completa_CodiceGiusto_CompletaESalva()
    {
        // Arrange
        Attivita attivita = new Attivita("Test");
        ArchivioInMemoria archivio = new ArchivioInMemoria(attivita);
        AttivitaService service = new AttivitaService(archivio);
        string codice = attivita.Id.ToString().Substring(0, 6);

        // Act
        service.Completa(codice);

        // Assert
        Attivita salvata = Assert.Single(archivio.Carica());
        Assert.True(salvata.Completata);
        Assert.Equal(1, archivio.Salvataggi);
    }
}

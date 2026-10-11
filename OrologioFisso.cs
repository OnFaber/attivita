namespace Attivita.Cli;

public class OrologioFisso : IOrologio
{
    public DateTime Adesso { get; private set; }

    #region Ctor & Destructor
    /// <summary>
    /// Standard Constructor
    /// </summary>
    public OrologioFisso(DateTime adesso) => Adesso = adesso;
    #endregion

    public void Avanza(TimeSpan quanto) => Adesso += quanto;
}

namespace GestoreAttivita.Core;

public class OrologioFisso : IOrologio
{
    public DateTimeOffset Adesso { get; private set; }

    #region Ctor & Destructor
    /// <summary>
    /// Standard Constructor
    /// </summary>
    public OrologioFisso(DateTimeOffset adesso) => Adesso = adesso;
    #endregion

    public void Avanza(TimeSpan quanto) => Adesso += quanto;
}

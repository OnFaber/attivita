namespace GestoreAttivita.Core;

public class OrologioDiSistema : IOrologio
{
    #region Ctor & Destructor
    /// <summary>
    /// Standard Constructor
    /// </summary>
    public OrologioDiSistema()
    {
    }
    #endregion

    public DateTimeOffset Adesso => DateTimeOffset.UtcNow;
}

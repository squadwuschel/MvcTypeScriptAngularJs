namespace MvcTypeScript.ProxyCreator.Interfaces
{
    /// <summary>
    /// Interface das sich um das Speichern der jeweiligen Proxy Klassen kümmert.
    /// </summary>
    public interface IProxyWriter
    {
        /// <summary>
        /// Der Speicherpfad für die Angular Proxy Dateien 
        /// </summary>
        string SavePath { get; set; }

        /// <summary>
        /// Speichern der jeweiligen Proxy Klasse.
        /// </summary>
        /// <param name="javaScriptContent">Der Inhalt der Datei</param>
        /// <param name="filename">der Name der JavaScript Datei.</param>
        /// <returns>TRUE-> Speichern erfolgreich | FALSE->Fehler beim Speichern.</returns>
        bool SaveProxyContent(string javaScriptContent, string filename);
    }
}

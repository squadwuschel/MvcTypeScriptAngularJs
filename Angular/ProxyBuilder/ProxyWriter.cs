using System;
using System.IO;
using System.Web;
using MvcTypeScript.ProxyCreator.Interfaces;

namespace MvcTypeScript.ProxyCreator.ProxyBuilder
{
    public class ProxyWriter : IProxyWriter
    {
        #region Konstruktor
        public ProxyWriter(string savePath)
        {
            SavePath = savePath;
        }
        #endregion

        #region Member
        /// <summary>
        /// Der Speicherpfad für die Angular Proxy Dateien 
        /// </summary>
        public string SavePath { get; set; }
        #endregion

        #region Public Functions
        /// <summary>
        /// Speichern der jeweiligen Proxy Klasse.
        /// </summary>
        /// <param name="javaScriptContent">Der Inhalt der Datei</param>
        /// <param name="filename">der Name der JavaScript Datei.</param>
        /// <returns>TRUE-> Speichern erfolgreich | FALSE->Fehler beim Speichern.</returns>
        public bool SaveProxyContent(string javaScriptContent, string filename)
        {
            //Prüfen ob ein Ausgabeverzeichnis angegeben wurde.
            if (string.IsNullOrEmpty(SavePath))
            {
                //Wenn nicht das StandardAusgabeverzeichnsi für JavaScript wählen.
                SavePath = "Scripts";
            }

            string path = HttpContext.Current.Server.MapPath("~");
            string newPth = Path.Combine(path, SavePath);
            System.Diagnostics.Trace.WriteLine(string.Format("Ausgabepfad für '{0}': '{1}'", filename, newPth));

            if (!Directory.Exists(newPth))
            {
                Directory.CreateDirectory(newPth);
            }

            try
            {
                string completePath = Path.Combine(newPth, filename);
                System.Diagnostics.Trace.WriteLine(string.Format("Kompletter Pfad für '{0}': '{1}' ", filename, completePath));
                File.WriteAllText(completePath, javaScriptContent);
                return true;
            }
            catch (Exception exception)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Fehler beim Erstellen der Datei: '{0}' - Message: '{1}'\r\n \r\n {2}", filename, exception.Message, exception));
            }
            
            return false;
        }
        #endregion
    }
}

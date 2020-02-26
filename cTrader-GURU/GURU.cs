using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Newtonsoft.Json;

namespace cTrader_GURU
{
    /// <summary>
    /// Una classe di esempio per chi si avvicina per la prima volta a questo linguaggio
    /// </summary>    
    public class GURU
    {
        /// <summary>
        /// Un esempio di come si richiama una nuova classe
        /// <para>Wiki : https://github.com/cTraderGURU/cTrader-GURU-Utility/wiki/GURU.Ciao() </para>
        /// </summary>
        /// <returns>
        /// Una semplice stringa "Hello Worlds !"
        /// </returns>
        public string Ciao() => "Hello World !";

    }

    /// <summary>
    /// Una classe di esempio per chi si avvicina per la prima volta a questo linguaggio
    /// </summary>   
    public static class GURUs
    {

        /// <summary>
        /// Un esempio di come si richiama una classe statica
        /// <para>Wiki : https://github.com/cTraderGURU/cTrader-GURU-Utility/wiki/GURUs.Ciao() </para>
        /// </summary>
        /// <returns>
        /// Una semplice stringa "Hello Worlds !"
        /// </returns>
        public static string Ciao() => "Hello Worlds !";

    }

}

namespace cTrader_GURU.Web
{

    /// <summary>
    /// Classe con metodi per gestire le richieste GET - POST
    /// </summary>
    public class Browser
    {

        /// <summary>
        /// Aggiunge un parametro GET a un url, tiene in considerazione quasi ogni scenario.
        /// <para>Wiki : https://github.com/cTraderGURU/cTrader-GURU-Utility/wiki/Browser.AddGetParamToUrl() </para>
        /// </summary>
        /// <returns>
        /// Restituisce l'url aggiornato con il parametro aggiunto.
        /// </returns>
        /// <param name="url">L'indirizzo a cui dobbiamo aggiungere il parametro</param>
        /// <param name="pname">Il nome del parametro</param>
        /// <param name="pvalue">Il valore del parametro</param>
        /// <param name="enc">Sottoporre ad encoding il valore del parametro</param>
        public static string AddGetParamToUrl(string url, string pname, string pvalue, bool enc = true)
        {

            if (enc)
                pvalue = WebUtility.UrlEncode(pvalue);

            if (url.IndexOf("?") > -1)
            {

                url = url.Replace("?", string.Format("?{0}={1}&", pname, pvalue));

            }
            else if (url.IndexOf("#") > -1)
            {

                url = url.Replace("#", string.Format("?{0}={1}#", pname, pvalue));

            }
            else
            {

                url = string.Format("{0}?{1}={2}", url, pname, pvalue);

            }

            return url;

        }

        /// <summary>
        /// Esegue una ricerca in una stringa per trovare i tag html scelti
        /// <para>Wiki : https://github.com/cTraderGURU/cTrader-GURU-Utility/wiki/Browser.ParseTag() </para>
        /// </summary>
        /// <returns>
        /// Una collezione di risultati per il tag html cercato
        /// </returns>
        /// <param name="html">La stringa con il codice sorgente</param>
        /// <param name="regexTag">Il pattern Regx per la ricerca</param>
        public static MatchCollection ParseTag(string html, string regexTag)
        {

            MatchCollection mytags = Regex.Matches(html, regexTag, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

            return mytags;

        }

        /// <summary>
        /// Richiesta HTTP --> GET, tiene in considerazione i protocolli di sicurezza, il redirect e la cache.
        /// <para>Wiki : https://github.com/cTraderGURU/cTrader-GURU-Utility/wiki/Browser.GET() </para>
        /// </summary>
        /// <returns>
        /// Restituisce il source della pagina
        /// </returns>
        /// <param name="url">L'indirizzo della pagina per la richiesta</param>
        /// <param name="nocache">Effettua la richiesta con parametro aggiuntivo mutevole</param>
        /// <param name="useragent">User-Agent personalizzato</param>
        public static string GET(string url, bool nocache = true, string useragent = "cTrader GURU")
        {

            string responseInString = "";

            // --> Richiesta originale senza cache ?

            if (nocache)
                url = AddGetParamToUrl(url, "t", DateTime.Now.ToString("yyyyMMddHHmmssffff"));

            // --> Mi servono i permessi di sicurezza per il dominio, compreso i redirect

            Uri myuri = new Uri(url);

            string pattern = string.Format("{0}://{1}/.*", myuri.Scheme, myuri.Host);

            // --> Autorizzo tutte le pagine di questo dominio

            Regex urlRegEx = new Regex(@pattern);
            WebPermission p = new WebPermission(NetworkAccess.Connect, urlRegEx);
            p.Assert();

            // --> Protocollo di sicurezza https://

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;

            // --> Prelevo la pagina e la restituisco

            using (var wb = new WebClient())
            {

                wb.Headers.Add("User-Agent", useragent);

                wb.Encoding = Encoding.UTF8;
                responseInString = wb.DownloadString(myuri);
                // --> responseInString = Encoding.UTF8.GetString(wb.DownloadData(url));

            }

            return responseInString;

        }

        /// <summary>
        /// Richiesta HTTP --> POST con parametri, tiene in considerazione i protocolli di sicurezza, il redirect e la cache.
        /// <para>Wiki : https://github.com/cTraderGURU/cTrader-GURU-Utility/wiki/Browser.POST() </para>
        /// </summary>
        /// <returns>
        /// Restituisce il source della pagina
        /// </returns>
        /// <param name="url">L'indirizzo della pagina per la richiesta</param>
        /// <param name="data">Parametri post</param>
        /// <param name="nocache">Effettua la richiesta con parametro aggiuntivo mutevole</param>
        /// <param name="useragent">User-Agent personalizzato</param>
        public static string POST(string url, NameValueCollection data, bool nocache = false, string useragent = "cTrader GURU")
        {

            string responseInString = "";

            // --> Richiesta originale senza cache ?

            if (nocache)
                url = AddGetParamToUrl(url, "t", DateTime.Now.ToString("yyyyMMddHHmmssffff"));

            // --> Mi servono i permessi di sicurezza per il dominio, compreso i redirect

            Uri myuri = new Uri(url);

            string pattern = string.Format("{0}://{1}/.*", myuri.Scheme, myuri.Host);

            // --> Autorizzo tutte le pagine di questo dominio

            Regex urlRegEx = new Regex(@pattern);
            WebPermission p = new WebPermission(NetworkAccess.Connect, urlRegEx);
            p.Assert();

            // --> Protocollo di sicurezza https://

            ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;

            using (var wb = new WebClient())
            {

                wb.Headers.Add("User-Agent", useragent);

                var response = wb.UploadValues(myuri, "POST", data);
                responseInString = Encoding.UTF8.GetString(response);

            }

            return responseInString;

        }

    }

}

namespace cTrader_GURU.Update
{

    /// <summary>
    /// Parametri necessari per richiedere le informazioni alle API fornite da ctrader.guru
    /// </summary>
    public class JsonPostParam
    {
        /// <summary>
        /// Il nome del Broker con cui il client opera, utilizzato a fini statistici e di profilazione
        /// </summary>
        public string AccountBroker { get; set; } = "";

        /// <summary>
        /// Il numero di conto con cui il client opera, utilizzato a fini statistici e di profilazione
        /// </summary>
        public int AccountNumber { get; set; } = 0;

        /// <summary>
        /// La versione attualmente in uso nel terminale
        /// </summary>
        public string MyVersion { get; set; } = "";

        /// <summary>
        /// L'ID del prodotto a cui riferirci per la richiesta di informazioni
        /// </summary>
        public int ProductID { get; set; } = 0;

    }

    /// <summary>
    /// Le informazioni restituite dal server in merito al prodotto indicato
    /// </summary>
    public class JsonUpdateInfo
    {

        // --> Informazioni del prodotto

        /// <summary>
        /// L'ID del prodotto a cui riferirci per la richiesta di informazioni
        /// </summary>
        public int ProductID { get; set; } = 0;

        /// <summary>
        /// La data dell'ultimo aggiornamento
        /// </summary>
        public string Updated { get; set; } = "";

        /// <summary>
        /// Ultima versione disponibile
        /// </summary>
        public string Version { get; set; } = "";

        // --> In caso di errori restituiscono l'exception e la risposta del server

        /// <summary>
        /// In caso di errori viene riempita dalle informazioni di Exception nella fase di elaborazione
        /// </summary>
        public string Exp { get; set; } = "";

        /// <summary>
        /// In caso di errori viene riempita con le ultime informazioni ricevute dal server
        /// </summary>
        public string Server_Response { get; set; } = "";

    }

    /// <summary>
    /// Classe con metodi per gestire le richieste di controllo delle versioni dei prodotti
    /// </summary>
    public class Check
    {

        /// <summary>
        /// Dopo l'inizializzazione della classe Check viene riempita con le informazioni di aggiornamenti/errori
        /// </summary>
        public JsonUpdateInfo ProductInformation { get; } = new JsonUpdateInfo();

        private JsonPostParam MyRequestConfig = new JsonPostParam();
        private string MyEndPoint = "";

        // --> Costruttore con i valori necessari

        /// <summary>
        /// Richiesta HTTP --> POST con parametri, ottiene i dati relativi al prodotto e verifica se ci sono aggiornamenti
        /// <para>Wiki : https://github.com/cTraderGURU/cTrader-GURU-Utility/wiki/Update.Check() </para>
        /// </summary>
        /// <returns>
        /// Restituisce funzioni e informazioni di aggiornamento
        /// </returns>
        /// <param name="request_config">Parametri per la richiesta POST</param>
        /// <param name="current_endpoint">L'indirizzo della pagina API per la richiesta</param>
        public Check(JsonPostParam request_config, string current_endpoint = "https://ctrader.guru/_getinfo_/")
        {

            // --> Inizializzo i parametri necessari alla richiesta e alla verifica

            MyRequestConfig = request_config;
            MyEndPoint = current_endpoint.Trim();

            string server_response = "";

            try
            {

                var data = new NameValueCollection
                {
                    ["account_broker"] = request_config.AccountBroker,
                    ["account_number"] = request_config.AccountNumber.ToString(),
                    ["my_version"] = request_config.MyVersion,
                    ["productid"] = request_config.ProductID.ToString()
                };

                server_response = Web.Browser.POST(MyEndPoint, data, true);

                ProductInformation = JsonConvert.DeserializeObject<JsonUpdateInfo>(server_response);  // <-- Nel cBot o indicatore bisogna attivare "AccessRights = AccessRights.FullAccess"

            }
            catch (Exception Exp)
            {

                ProductInformation.Server_Response = server_response;
                ProductInformation.Exp = Exp.Message;

            }

        }

        /// <summary>
        /// Metodo che ci conferma o meno la presenza di nuovi aggiornamenti
        /// <para>Wiki : https://github.com/cTraderGURU/cTrader-GURU-Utility/wiki/Update.Check() </para>
        /// </summary>
        /// <returns>
        /// Restituisce un valore boleano per la presenza di aggiornamenti
        /// </returns>
        public bool HaveNewUpdate()
        {

            return (ProductInformation.ProductID == MyRequestConfig.ProductID && ProductInformation.Version != "" && MyRequestConfig.MyVersion != "" && new Version(MyRequestConfig.MyVersion).CompareTo(new Version(ProductInformation.Version)) < 0);

        }

        /// <summary>
        /// Metodo che ci conferma o meno della presenza di errori nella richiesta
        /// <para>Wiki : https://github.com/cTraderGURU/cTrader-GURU-Utility/wiki/Update.Check() </para>
        /// </summary>
        /// <returns>
        /// Restituisce un valore boleano per la presenza di errori
        /// </returns>
        public bool HaveError()
        {

            return (ProductInformation.Exp != "");

        }

    }

}

namespace cTrader_GURU.Font
{

    public enum FontFamilyType
    {

        [Description("Century Gothic")]
        CenturyGothic = 1,

        [Description("Cabin Regular")]
        CabinRegular = 2,

        [Description("Open Sans Light")]
        OpenSansLight = 3,

        [Description("Open Sans Regular")]
        OpenSansRegular = 4,

        [Description("Open Sans Bold")]
        OpenSansBold = 5

    };

    public class FontFamily
    {

        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbfont, uint cbfont, IntPtr pdv, [In] ref uint pcFonts);

        private System.Drawing.FontFamily _loadFont(byte[] fontArray, int dataLength)
        {

            IntPtr ptrData = Marshal.AllocCoTaskMem(dataLength);

            Marshal.Copy(fontArray, 0, ptrData, dataLength);

            uint cFonts = 0;

            AddFontMemResourceEx(ptrData, (uint)fontArray.Length, IntPtr.Zero, ref cFonts);

            PrivateFontCollection pfc = new PrivateFontCollection();

            pfc.AddMemoryFont(ptrData, dataLength);

            Marshal.FreeCoTaskMem(ptrData);

            return pfc.Families[0];

        }

        /// <summary>
        /// Mette a disposizione un font senza alcuna installazione
        /// <para>Wiki : https://github.com/cTraderGURU/cTrader-GURU-Utility/wiki/FontFamily.GetFontFamily() </para>
        /// </summary>
        /// <returns>
        /// Restituisce il Font richiesto tra quelli a disposizione in cTrader_GURU.Font.FontFamilyType
        /// </returns>
        /// <param name="myFFtype">Il tipo di carattere che si vuole utilizzare</param>
        public System.Drawing.FontFamily GetFontFamily(FontFamilyType myFFtype)
        {

            System.Drawing.FontFamily ff;

            switch (myFFtype)
            {

                case FontFamilyType.CenturyGothic:

                    ff = _loadFont(Properties.Resources.CenturyGothic, Properties.Resources.CenturyGothic.Length);
                    break;

                case FontFamilyType.CabinRegular:

                    ff = _loadFont(Properties.Resources.Cabin_Regular, Properties.Resources.Cabin_Regular.Length);
                    break;

                case FontFamilyType.OpenSansLight:

                    ff = _loadFont(Properties.Resources.OpenSans_Light, Properties.Resources.OpenSans_Light.Length);
                    break;

                case FontFamilyType.OpenSansRegular:

                    ff = _loadFont(Properties.Resources.OpenSans_Regular, Properties.Resources.OpenSans_Regular.Length);
                    break;

                case FontFamilyType.OpenSansBold:

                    ff = _loadFont(Properties.Resources.OpenSans_Bold, Properties.Resources.OpenSans_Bold.Length);
                    break;

                default:

                    throw new Exception("cTrader_GURU.Font.FontFamily.GetFontFamily() : Unknown parameter");

            }

            return ff;

        }

    }

}

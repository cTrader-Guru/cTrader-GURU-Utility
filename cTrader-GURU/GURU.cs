using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace cTrader_GURU
{

    // --> Esempio pratico per le giovani leve :)

    public class GURU
    {

        public string ciao() => "Hello World !";

    }

    public static class GURUs
    {

        public static string Ciao() => "Hello Worlds !";

    }

    // <-- Esempio pratico per le giovani leve :)

}

namespace cTrader_GURU.Web
{

    public class Browser
    {

        /// <summary>
        /// Aggiunge un parametro get a un url, tiene in considerazione quasi ogni scenario.
        /// </summary>
        /// <returns>
        /// Restituisce l'url aggiornato con il parametro aggiunto.
        /// </returns>
        /// <example>
        /// <code>
        /// using cTrader_GURU.Web;
        /// 
        /// string homepage = Browser.AddGetParamToUrl( "https://ctrader.guru/?id=1#reload", "page", "3" );
        /// </code>
        /// </example>
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
        /// Esegue una ricerca in una stringa per trovare i tag scelti
        /// </summary>
        /// <returns>
        /// Una collezione di risultati per il tag cercato
        /// </returns>
        /// <example>
        /// <code>
        /// using cTrader_GURU.Web;
        /// using System.Text.RegularExpressions;
        /// 
        /// MatchCollection mylinks = Browser.ParseTag("html source code", "href=['\"]([^'\"]+?)['\"]");
        /// 
        /// if (mylinks.Count > 0)
        /// {
        ///
        ///     foreach (Match mylink in mylinks)
        ///     {
        ///     
        ///         string myhref = mylink.Groups[1].Value;
        /// 
        ///     }
        ///     
        /// }
        /// </code>
        /// </example>
        /// <param name="html">La stringa con il codice sorgente</param>
        /// <param name="regexTag">Il pattern Regx per la ricerca</param>
        public static MatchCollection ParseTag(string html, string regexTag)
        {
            
            MatchCollection mytags = Regex.Matches(html, regexTag, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

            return mytags;

        }

        /// <summary>
        /// Richiesta HTTP --> GET
        /// Tiene in considerazione i protocolli di sicurezza, il redirect e la cache.
        /// </summary>
        /// <returns>
        /// Restituisce il source della pagina
        /// </returns>
        /// <example>
        /// <code>
        /// using cTrader_GURU.Web;
        /// 
        /// string homepage = Browser.GET( "https://ctrader.guru/" );
        /// </code>
        /// </example>
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
        /// Richiesta HTTP --> POST con parametri
        /// Tiene in considerazione i protocolli di sicurezza, il redirect e la cache.
        /// </summary>
        /// <returns>
        /// Restituisce il source della pagina
        /// </returns>
        /// <example>
        /// <code>
        /// using cTrader_GURU.Web;
        /// using System.Collections.Specialized;
        /// 
        /// var data = new NameValueCollection();
        /// data["username"] = "gUrU";
        /// 
        /// string response = Browser.POST( "https://ctrader.guru/", data );
        /// </code>
        /// </example>
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

namespace cTrader_GURU.Font {

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
        /// </summary>
        /// <returns>
        /// Restituisce il Font richiesto tra quelli a disposizione in cTrader_GURU.Font.FontFamilyType
        /// </returns>
        /// <example>
        /// <code>
        ///     private void Form1_Load(object sender, EventArgs e){
        ///
        ///         cTrader_GURU.Font.FontFamily myFF = new cTrader_GURU.Font.FontFamily();
        ///         System.Drawing.FontFamily ffCenturyGothic = myFF.GetFontFamily(cTrader_GURU.Font.FontFamilyType.CenturyGothic);
        ///
        ///         foreach (Control c in this.Controls ) {
        ///
        ///             if (c == null) continue;
        ///
        ///             string currentFontName = c.Font.FontFamily.Name.ToUpper();
        ///
        ///             if(currentFontName.Equals("MICROSOFT SANS SERIF") ) c.Font = new Font(ffCenturyGothic, c.Font.Size, c.Font.Style);
        ///
        ///         }
        ///         
        ///     }
        /// </code>
        /// </example>
        /// <param name="myFFtype">Il tipo di carattere che si vuole utilizzare</param>
        public System.Drawing.FontFamily GetFontFamily(FontFamilyType myFFtype ) {

                    System.Drawing.FontFamily ff;

                    switch (myFFtype) {

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

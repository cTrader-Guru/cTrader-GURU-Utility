using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

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
        /// string homepage = Browser.AddGetParamToUrl( 'https://ctrader.guru/?id=1#reload', 'page', '3' );
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
        /// string homepage = Browser.GET( 'https://ctrader.guru/' );
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
        /// string response = Browser.POST( 'https://ctrader.guru/', data );
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows_OnOff
{
    internal class Translator
    {

        private static readonly Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>
       {
             {"UnknownCommand", new Dictionary<string, string>{{ "en", "Unknown command." },{ "de", "Unbekannter Befehl." },}},
             {"shutdown", new Dictionary<string, string>{{ "en", "Shutdown command received. The computer is shutting down." },{ "de", "Herunterfahren-Befehl empfangen. Der Computer wird heruntergefahren." },}},
             {"sleep", new Dictionary<string, string>{{ "en", "Sleep command received. The computer is put into sleep." },{ "de", "Slepp-Befehl empfangen. Der Computer wird in Sleep versetzt." },}},
             {"hibernate", new Dictionary<string, string>{{ "en", "hibernate command received. The computer is put into Hibernate mode." },{ "de", "Slepp-Befehl empfangen. Der Computer wird in Hibernate mode versetzt." },}},
             {"duckdn_updaed",new Dictionary<string, string>{{"en","The external IP address was successfully updated" },{"de","Die externe IP Adresse wurde erfolgreich geupdated" }}},
             {"ServerStartedUp",new Dictionary<string, string>{{"en", "Your Windows_Service has been started." },{"de", "Dein Windows_Service wurde gestarted." }}},
             {"ServerShutDown",new Dictionary<string, string>{{"en", "Your Windows service has been stoped." },{"de", "Dein Windows_Service wurde gestoped." }}},
             {"NoTranslation",new Dictionary<string, string>{{"en", "No translation available for." },{"de", "Keine Übersetzung verfügbar für." }}},
             {"CommandServerStop",new Dictionary<string, string>{{"en", "Windows service will stopped." },{"de", "Windows Dienst wird angehalten." }}},
             {"ServerUninstall",new Dictionary<string, string>{{"en", "Service will uninstalling." },{"de", "Der Dienst wird deinstalliert." }}},
             {"ServerFolderDelete",new Dictionary<string, string>{{"en", "Service Folder will deleting." },{"de", "Der Dienst wird gelöscht." }}},
             {"Message_send",new Dictionary<string, string>{{"en", "Message successfully sent!." }, {"de", "Message erfolgreich gesendet!." }}},
             {"sendPhoto",new Dictionary<string, string>{{"en", "Photo sent successfully from Device!." },{"de", "Foto erfolgreich von dem Gerät gesendet!." }}},
             {"sendVideo",new Dictionary<string, string> {{"en", "Video sent successfully!." },{"de", "Video erfolgreich gesendet!." }}},
             {"sendDocument",new Dictionary<string, string>{{"en", "Document sent successfully!." },{"de", "Document erfolgreich gesendet!." }}},
             {"Error_set_download",new Dictionary<string, string>{{"en", "Please send the file with the command!." },{"de", "Bitte senden Sie die Datei mit dem Befehl!." } }},
             {"no_file",new Dictionary<string, string>{{"en", "File not found." },{"de", "Datei nicht gefunden." }}},
             {"userName_error",new Dictionary<string, string>{{"en", "The entered username was not found." },{"de", "die eingegebene Username wurde nicht gefunden ." } }},
        };
        public static string get_translate(string word, string language)
        {
            if (dictionary.TryGetValue(word, out var translation))
            {
                return translation[language];
            }
            else
            {
                dictionary.TryGetValue("NoTranslation", out var translation1);
                return $"{translation1[language]} '{word}'."; // Übersetzung nicht gefunden
            }
        }   
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Windows_OnOff
{
    internal class static_variables
    {
        public static string log_path = null;
        public static string language = null;
        public static string Service_name = null;
        public static string Service_full_path = null;
        public static string Service_folder_path = null;
        public static string xml_root_path = null;
        public static string user_name = "";
        public static bool Telegram_listener = false;
        public static bool Check_xml_processing = false;
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows_OnOff.Command
{
    internal class commands_properties
    {
        public const string Shutdown = "shutdown";
        public const string Sleep = "sleep";
        public const string Hibernate = "hibernate";
        public const string Serverinfo = "server_info";
        public const string S_stop = "s_stop";
        public const string S_uninstall = "s_uninstall";
        public const string S_delete = "s_delete";
        public const string Get_f = "get_f";
        public const string Get_folder = "get_folder";
        public const string Get_file = "get_file";
        public const string Get_p = "get_p";
        public const string Get_v = "get_v";
        public const string Get_d = "get_d";
        public const string Set_photo = "set_p";
        public const string Set_video = "set_v";
        public const string Set_document = "set_d";
        public const string Command_info = "command_info";
       

        public static string get_commands_json()
        {
            var commands_propertiesList = new List<string>()
            {
            commands_properties.Shutdown,
            commands_properties.Sleep,
            commands_properties.Hibernate,
            commands_properties.Serverinfo,
            commands_properties.S_stop,
            commands_properties.S_uninstall,
            commands_properties.S_delete,
            commands_properties.Get_f,
            commands_properties.Get_folder,
            commands_properties.Get_file,
            commands_properties.Get_p,
            commands_properties.Get_v,
            commands_properties.Get_d,
            commands_properties.Set_photo,
            commands_properties.Set_video,
            commands_properties.Set_document,
            commands_properties.Command_info
           };
            string jsonArray = JsonConvert.SerializeObject(commands_propertiesList, Formatting.Indented);
            return jsonArray;
        }
        public static List<string> get_commands_list()
        {
            var commands_propertiesList = new List<string>()
            {
            commands_properties.Shutdown,
            commands_properties.Sleep,
            commands_properties.Hibernate,
            commands_properties.Serverinfo,
            commands_properties.S_stop,
            commands_properties.S_uninstall,
            commands_properties.S_delete,
            commands_properties.Get_f,
            commands_properties.Get_folder,
            commands_properties.Get_file,
            commands_properties.Get_p,
            commands_properties.Get_v,
            commands_properties.Get_d,
            commands_properties.Set_photo,
            commands_properties.Set_video,
            commands_properties.Set_document,
            commands_properties.Command_info,

           };
            return commands_propertiesList;
        }
    }
}

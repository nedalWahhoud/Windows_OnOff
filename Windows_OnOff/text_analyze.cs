using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows_OnOff.Command;

namespace Windows_OnOff
{
    internal class text_analyze
    {
        public static string analyze_text_en(string text)
        {
            string result = null;

            List<string> get_commands_list = commands_properties.get_commands_list();

            for (int i = 0; i < get_commands_list.Count; i++)
            {
                string command = get_commands_list[i];
                Regex regex = new Regex($@"{Regex.Escape(command)}\S*", RegexOptions.IgnoreCase);
                if (regex.IsMatch(text))
                {
                    result = regex.Match(text).ToString();
                    break;
                }
            }

            return result ?? text;
        }
    }
}

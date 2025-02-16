using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows_OnOff.Command;
using static Windows_OnOff.data_processing;

namespace Windows_OnOff
{
    internal class commands_processing
    {
        private data_processing dp = new data_processing();
        private xml_processing xp = new xml_processing();

        public commands_processing()
        {

        }
        public async Task<string> commands(user_checker_class message)
        {
            // text analyze
         //   message.Command = text_analyze.analyze_text_en(message.Command);

            // command if command was sent with message
            // (:) This letter is this separator are permanent
            string rest_command = null;
            if (message.Command.Contains(':'))
            {
                // data with command
                rest_command = message.Command.Substring(message.Command.IndexOf(":") + 1);
                // if Backslash repeats 4 times
                if (rest_command.Contains("\\\\"))
                    rest_command = rest_command.Replace("\\\\", "\\");
                // command
                message.Command = message.Command.Substring(0, message.Command.IndexOf(":"));
            }

            string st = null;
            switch (message.Command.ToString().ToLower().Trim())
            {
                // Computer shutdown
                case commands_properties.Shutdown:
                    st = Execute_shutdown();
                    return st;
                // Computer sleep
                case commands_properties.Sleep:
                    st = Execute_sleepMode();
                    return st;
                case commands_properties.Hibernate:
                    st = Execute_hibernateMode();
                    return st;
                // Server info
                case commands_properties.Serverinfo:
                    st = Get_server_info();
                    return st;
                // Windows Server stop
                case commands_properties.S_stop:
                    st = Server_stop(5000);
                    return st;
                // Windows Server uninstall
                case commands_properties.S_uninstall:
                    Server_stop(6000);
                    st = Server_uninstall(3000);
                    return st;
                // Windows Server Folders unt Files delete
                case commands_properties.S_delete:
                    Server_stop(6000);
                    Server_uninstall(3000);
                    st = Server_folder_delete(9);
                    return st;
                // Windows Server all folders und files of a Path
                case commands_properties.Get_f:
                    st = Get_folders_files(rest_command, "all");
                    return st;
                // Windows Server only folders of a Path
                case commands_properties.Get_folder:
                    st = Get_folders_files(rest_command, "folder");
                    return st;
                // Windows Server only file of a Path
                case commands_properties.Get_file:
                    st = Get_folders_files(rest_command, "file");
                    return st;
                // send Photo to telgram_bot
                case commands_properties.Get_p:
                    st = Get_data(rest_command, "sendPhoto", "photo");
                    return st;
                // send Vido to telgram_bot
                case commands_properties.Get_v:
                    st = Get_data(rest_command, "sendVideo", "video");
                    return st;
                // send Vido to telgram_bot
                case commands_properties.Get_d:
                    st = Get_data(rest_command, "sendDocument", "document");
                    return st;
                case commands_properties.Command_info:
                    st = commands_properties.get_commands_json();
                    return st;
                case commands_properties.Set_photo:
                    st = await set_data(message.file_id, rest_command, "Image", ".jpg");
                    return st;
                case commands_properties.Set_video:
                    st = await set_data(message.file_id, rest_command, "Video", ".mp4");
                    return st;
                case commands_properties.Set_document:
                    st = await set_data(message.file_id, rest_command, "Document", ".txt");
                    return st;
                default:
                    return Translator.get_translate("UnknownCommand", static_variables.language);
            }
        }
        private string Execute_shutdown()
        {
            try
            {
                Task task = Task.Run(() =>
                {
                    Task.Delay(5000).Wait();
                    Process.Start("shutdown", "/s /t 0"); // Herunterfahren ausführen
                });
                return Translator.get_translate("shutdown", static_variables.language);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [DllImport("powrprof.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetSuspendState(bool Hibernate, bool ForceCritical, bool DisableWakeEvent);
        private string Execute_sleepMode()
        {
            try
            {
                Task task = Task.Run(() =>
                {
                    Task.Delay(5000).Wait();
                    SetSuspendState(false, false, false);
                });

                return Translator.get_translate("sleep", static_variables.language);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private string Execute_hibernateMode()
        {
            try
            {
                Task task = Task.Run(() =>
                {
                    Task.Delay(5000).Wait();
                    SetSuspendState(true, false, false);
                });

                return Translator.get_translate("hibernate", static_variables.language);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private string Get_server_info()
        {
            try
            {
                // Beispiel: Holt die Systeminformationen
                var osVersion = Environment.OSVersion;
                var machineName = Environment.MachineName;
                var processorCount = Environment.ProcessorCount;

                // Erstelle eine JSON-Antwort (oder ein beliebiges Format)
                var serverInfo = new
                {
                    OSVersion = osVersion.ToString(),
                    MachineName = machineName,
                    ProcessorCount = processorCount,
                    CurrentTime = DateTime.Now
                };

                return JsonConvert.SerializeObject(serverInfo);  // Nutze Json.NET für die Serialisierung
            }
            catch (Exception ex)
            {
                return $"Fehler beim Abrufen der Serverinformationen: {ex.Message}";
            }
        }
        private string Server_stop(int delay)
        {
            ServiceController service = new ServiceController(static_variables.Service_name);
            try
            {
                // stop telegram listener
                static_variables.Telegram_listener = false;
                dp._logWrite("Service stopping...");
                // execution delay 5 seconds to send the message
                Task task = Task.Run(() =>
                {
                    Task.Delay(delay).Wait();
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);

                });
                string tr = Translator.get_translate("CommandServerStop", static_variables.language);
                dp._logWrite(tr);
                return tr;
            }
            catch (Exception ex)
            {
                dp._logWrite($"Error during service stopping: {ex.Message}");
                return $"Error during service stopping: {ex.Message}";
            }
        }
        private string Server_uninstall(int delay)
        {
            try
            {
                Task task = Task.Run(() =>
                {
                    Task.Delay(delay).Wait();
                    ManagedInstallerClass.InstallHelper(new[] { "/u", static_variables.Service_full_path });
                });
                return Translator.get_translate("ServerUninstall", static_variables.language);
            }
            catch (Exception ex)
            {
                return $"Error deleting service{ex.Message}";
            }
        }
        private string Server_folder_delete(int timeout)
        {
            try
            {
                string command = xp.get_XmlElements("CommandServiceDelete").Value.ToString();
                command = $"timeout /t {timeout} && {command}";
                Task task = Task.Run(() =>
                {
                    Execute_interne_command(command);
                });
                return Translator.get_translate("ServerFolderDelete", static_variables.language);
            }
            catch (Exception ex)
            {
                dp._logWrite($"Error deleting service Folder: {ex.Message}");
                return $"Error deleting service Folder: {ex.Message}";
            }
        }
        private string Get_folders_files(string path, string GetType)
        {
            try
            {
                // space start und end delete
                path = path.Trim();

                List<string> data = new List<string>();

                if (GetType.ToLower() == "file")
                {
                    string[] files = Directory.GetFiles(path);
                    data.AddRange(files);
                }
                else if (GetType.ToLower() == "folder")
                {
                    string[] folders = Directory.GetDirectories(path);
                    data.AddRange(folders);
                }
                else
                {
                    string[] folders = Directory.GetDirectories(path);
                    data.AddRange(folders);
                    string[] files = Directory.GetFiles(path);
                    data.AddRange(files);
                }
                string json = JsonConvert.SerializeObject(data);

                return json;
            }
            catch (Exception ex)
            {
                return $"Error get_folders_files: {ex.Message}";
            }
        }
        private string Get_data(string path, string send_type, string stream_name)
        {
            try
            {
                // space start und end delete
                path = path.Trim();

                string response = null;
                // if array from paths sended
                if (path[0] == '[' && path[path.Length - 1] == ']')
                {
                    // delete brackets
                    path = path.Trim('[', ']');
                    // convert to Array
                    string[] paths = path.Split(',');
                    for (int i = 0; i < paths.Length; i++)
                    {
                        // if the paths is array 
                        if (!File.Exists(paths[i])) response = Translator.get_translate("no_file", static_variables.language);
                        // to telegram bot send
                        var response1 = telegram_processing.telegramSend_data(paths[i], send_type, stream_name);

                        response = response + "   ||   " + response1;
                    }
                    return response;
                }
                // if no
                else
                {
                    // if the paths is array 
                    if (!File.Exists(path)) return Translator.get_translate("no_file", static_variables.language);
                    // to telegram bot send
                    response = telegram_processing.telegramSend_data(path, send_type, stream_name).Result;

                    return response;
                }
            }
            catch (Exception ex)
            {
                string error = $"Error get_data: {ex.Message}";
                dp._logWrite(error);
                return error;
            }
        }
        private async Task<string> set_data(string file_id, string save_path, string data_name, string extension)
        {
            string st = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string get_file_url = $"https://api.telegram.org/bot{telegram_processing.telegram_botToken}/getFile?file_id={file_id}";
                    HttpResponseMessage response = await client.GetAsync(get_file_url);
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    // JSON-Answer parsen
                    JsonDocument doc = JsonDocument.Parse(jsonResponse);
                    string file_path = null;
                    string no_file = Translator.get_translate("no_file", static_variables.language); ;
                    if (doc.RootElement.TryGetProperty("result", out var result) && result.TryGetProperty("file_path", out var filePathProperty))
                    {
                        file_path = filePathProperty.GetString();
                    }
                    else return no_file;
                    // get extension
                    extension = Path.GetExtension(file_path) ?? extension;
                    // Photo with file_path download
                    string file_url = $"https://api.telegram.org/file/bot{telegram_processing.telegram_botToken}/{file_path}";
                    byte[] file_bytes = await client.GetByteArrayAsync(file_url);
                    // data save
                    save_path = dp.file_path_editing(save_path, data_name, extension);
                    await Task.Run(() => File.WriteAllBytes(save_path, file_bytes));
                    st = $"{Translator.get_translate("Error_set_download", static_variables.language)} {save_path}";
                    dp._logWrite(st);
                    return st;
                }
            }
            catch (Exception ex)
            {
                st = $"Error Download_file: {ex.Message}";
                dp._logWrite(st);
                return st;
            }
        }
        private void Execute_interne_command(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c " + command;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            try
            {
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                dp._logWrite(output);
                dp._logWrite(error);
            }
            catch (Exception ex)
            {
                dp._logWrite($"Error deleting command execute {ex.Message}");
            }
        }
    }
}   

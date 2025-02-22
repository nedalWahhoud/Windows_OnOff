using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Win32.TaskScheduler;

namespace Windows_OnOff
{
    internal class data_processing
    {
        public class user_checker_class
        {
            public string Command { get; set; }
            public string file_id { get; set; }
            public bool Result { get; set; }
        }
        public data_processing()
        {

        }
        public bool IsInternet_available()
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    var reply = ping.Send("8.8.8.8", 5000);
                    return reply != null && reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }
        public void Get_path_windowsService()
        {
            try
            {
                string query = $"SELECT PathName FROM Win32_Service WHERE Name = '{static_variables.Service_name}'";

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                using (ManagementObjectCollection results = searcher.Get())
                {
                    foreach (ManagementObject service in results)
                    {
                        static_variables.Service_full_path = service["PathName"]?.ToString();
                        static_variables.Service_folder_path = Path.GetDirectoryName(static_variables.Service_full_path);
                    }
                }
            }
            catch (Exception ex)
            {
                _logWrite($"Error retrieving service path: {ex.Message}");
            }
        }
        public void service_checker()
        {
            // bat
            // script in bat write
            string serviceChecker_ScriptPath = static_variables.Service_folder_path + "\\service_checker.bat";
            if (!File.Exists(serviceChecker_ScriptPath))
            {
                string skript = "set \"ServiceName=Windows_OnOff\"\r\nsc query \"%ServiceName%\" | find /i \"RUNNING\" >nul\r\nif %errorlevel%==0 (\r\n     echo Der Dienst \"%ServiceName%\" läuft bereits.\r\n) else (\r\n    \r\n    net start \"%ServiceName%\"\r\n    if %errorlevel%==0 (\r\n        echo Der Dienst \"%ServiceName%\" wurde erfolgreich gestartet.\r\n    ) else (\r\n          echo Fehler: Der Dienst \"%ServiceName%\" konnte nicht gestartet werden.\r\n    )\r\n)";
                try
                {
                    File.WriteAllText(serviceChecker_ScriptPath, skript);
                    //
                    _logWrite("Service Checker Skript was written successfully.");

                }
                catch (Exception ex)
                {
                    _logWrite("Error when writing .bat-script: " + ex.Message);
                }
            }
            // vbs
            // write vbs to run bat script without opening cmd-window
            string vbs_path = static_variables.Service_folder_path + "\\run_service_checker.vbs";
            if (!File.Exists(vbs_path))
            {
                string skript = $"Set objShell = CreateObject(\"WScript.Shell\")\r\nobjShell.Run \"{serviceChecker_ScriptPath}\", 0, True";
                try
                {
                    File.WriteAllText(vbs_path, skript);
                    //
                    _logWrite("Run Service (.vbs) was written successfully.");
                }
                catch (Exception ex)
                {
                    _logWrite("Error when writing .vbs-script: " + ex.Message);
                }
            }

            // add to TaskScheduler
            add_skript_TaskScheduler(vbs_path, "service_checker_windows_OnOff");
        }
        private void add_skript_TaskScheduler(string script_path, string task_name)
        {
            try
            {
                using (Microsoft.Win32.TaskScheduler.TaskService task_service = new Microsoft.Win32.TaskScheduler.TaskService())
                {
                    Microsoft.Win32.TaskScheduler.Task existingTask = task_service.FindTask(task_name, false);
                    if (existingTask != null)
                    {
                        _logWrite($"Taks '{task_name}' already exists.");
                        // delete Task to new craete
                        task_service.RootFolder.DeleteTask(task_name);
                        _logWrite($"Taks '{task_name}' deleted.");
                    }

                    // create new task 
                    Microsoft.Win32.TaskScheduler.TaskDefinition task_definition = task_service.NewTask();
                    task_definition.RegistrationInfo.Description = "Monitors and starts a Windows service.";
                    task_definition.Principal.LogonType = TaskLogonType.ServiceAccount; //Task is carried out interactively
                    task_definition.Principal.RunLevel = TaskRunLevel.Highest;

                    // Add triggers a BootTrigger to run when Windows starts
                    task_definition.Triggers.Add(new BootTrigger());
                    // Add triggers (e.g. run every hour)
                    task_definition.Triggers.Add(new DailyTrigger { StartBoundary = DateTime.Now, Repetition = new RepetitionPattern(TimeSpan.FromHours(1), TimeSpan.Zero) });

                    // Set a time limit if the script takes more than 1 hour (in case of errors or hangs)
                    task_definition.Settings.ExecutionTimeLimit = TimeSpan.FromHours(1);
                    // Action: Run PowerShell script
                    task_definition.Actions.Add(new ExecAction(
                        script_path, // Programm
                        null, // arguments
                        null // working directory (optional)
                    ));
                    // Register task
                    task_service.RootFolder.RegisterTaskDefinition(task_name, task_definition);

                    _logWrite($"Taks '{task_name}' was created successfully.");
                }
            }
            catch (Exception ex)
            {
                _logWrite("TaskScheduler_Error: " + ex.Message);
            }
        }
        public void get_userName()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    static_variables.user_name = queryObj["UserName"]?.ToString();
                    _logWrite($"User Name is: {static_variables.user_name}");
                    static_variables.user_name = static_variables.user_name + "^ ";
                }
            }
            catch (Exception ex)
            {
                static_variables.user_name = $"Error: {ex.Message}";
                _logWrite($"Error while get_UserName: {ex.Message}");
            }
        }
        public user_checker_class check_user(string message)
        {
            user_checker_class uc = new user_checker_class();

            string unser_error = Translator.get_translate("userName_error", static_variables.language);
            // 
            if (string.IsNullOrEmpty(message) || !message.Contains("^"))
            {
                uc.Command = unser_error;
                uc.Result = false;
                return uc;
            }
            // if Space is in the message before or after ^ then ignore
            message = Regex.Replace(message, @"(^\^ | \^$| \^ )", "^");
            // 
            string t_user_name = (message.Substring(0, message.IndexOf('^') + 1).ToLower()).Trim();
            // if all 
            if (t_user_name == "^")
                message = (message.Substring(message.IndexOf("^") + 1).ToLower()).Trim();
            else
            {

                t_user_name = (message.Substring(0, message.IndexOf('^')).ToLower()).Trim();
                t_user_name = (t_user_name.Substring(t_user_name.IndexOf('^') + 1).ToLower()).Trim();
                string user_name = (static_variables.user_name.Substring(0, static_variables.user_name.IndexOf("^")).ToLower()).Trim();
                // if user is not the same return -1, the command is not executed
                if (!(t_user_name == user_name))
                    message = "-1";
                else
                {
                    message = message = (message.Substring(message.IndexOf('^') + 1).ToLower()).Trim();
                }
            }

            _logWrite($"Check User result: {message}");

            uc.Command = message;
            uc.Result = true;
            return uc;
        }
        public string file_path_editing(string rest_command, string file_name, string extension)
        {
            if (is_valid_path(rest_command, extension))
                return rest_command;

            int counter = 0;

            string file_path = Path.Combine(static_variables.Service_folder_path, $"{file_name}{counter.ToString()}{extension}");

            if (File.Exists(file_path))
            {
                string directory = Path.GetDirectoryName(file_path);
                string[] files = Directory.GetFiles(Path.GetDirectoryName(file_path), $"{file_name}*.*");

                foreach (string file in files)
                {
                    counter++;
                    file_path = Path.Combine(static_variables.Service_folder_path, $"{file_name}{counter.ToString()}{extension}");
                    if (!File.Exists(file_path))
                        break;
                }
                return file_path;
            }
            else
                return file_path;
        }
        private bool is_valid_path(string path, string extension)
        {
            try
            {
                // check if path is valid
                bool is_valid_path = System.IO.Path.IsPathRooted(path) && !path.Any(Path.GetInvalidPathChars().Contains);
                // check if file name is valid
                string file_name = Path.GetFileName(path);
                bool is_valid_name = !string.IsNullOrEmpty(file_name) && !file_name.Any(Path.GetInvalidFileNameChars().Contains);
                // check if file extension is valid
                string extension1 = Path.GetExtension(path);
                bool is_valid_extension = !string.IsNullOrEmpty(extension1);
                // 
                if (extension1 != extension)
                    is_valid_extension = false;

                return is_valid_path && is_valid_name && is_valid_extension;
            }
            catch (Exception ex)
            {
                _logWrite($"Error while checking path: {ex.Message}");
                return false;
            }
        }
        public void set_priority()
        {
            try
            {
                Process currentProcess = Process.GetCurrentProcess();
                if (currentProcess.PriorityClass != ProcessPriorityClass.RealTime)
                {
                    currentProcess.PriorityClass = ProcessPriorityClass.RealTime;
                    _logWrite("Priority was set");
                }
            }
            catch (Exception ex)
            {
                _logWrite("Erorr at priority: " + ex.Message);
            }
        }
        public int delay_time(int delay_time, int multiply_times,ref int multiply)
        {
            if (multiply >= multiply_times)
                multiply = 1;
            delay_time = delay_time * multiply;
            multiply++;

            return delay_time;
        }
        public void _logWrite(string st)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(static_variables.log_path))
                {
                    sw.NewLine = "\n";
                    sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy  HH:mm ") + st);
                }
            }
            catch (Exception ex)
            {
               /* using (StreamWriter sw = File.AppendText(static_variables.log_path))
                {
                    sw.NewLine = "\n";
                    sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy  HH:mm ") + "Error when Log writing " + ex.Message);
                }*/
            }
        }
        public void _logDelete()
        {
            try
            {
                if (File.Exists(static_variables.log_path)) { File.Delete(static_variables.log_path); }
            }
            catch(Exception ex)
            {
              /*  using (StreamWriter sw = File.AppendText(static_variables.log_path))
                {
                    sw.NewLine = "\n";
                    sw.WriteLine(DateTime.Now.ToString("dd.MM.yyyy  HH:mm ") + "Error when Log deleting " + ex.Message);
                }*/
            }
        }
    }
}

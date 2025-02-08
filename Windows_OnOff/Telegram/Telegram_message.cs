using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Windows_OnOff.Telegram
{
    public class Telegram_message
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("result")]
        public List<Result> Result { get; set; }
    }

    public class Result
    {
        [JsonPropertyName("update_id")]
        public long UpdateId { get; set; }

        [JsonPropertyName("message")]
        public Message Message { get; set; }
    }

    public class Message
    {
        [JsonPropertyName("message_id")]
        public long MessageId { get; set; }

        [JsonPropertyName("from")]
        public User From { get; set; }

        [JsonPropertyName("chat")]
        public Chat Chat { get; set; }

        [JsonPropertyName("date")]
        public long Date { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("photo")]
        public List<TelegramPhoto> Photo { get; set; }
        [JsonPropertyName("video")]
        public TelegramVideo Video { get; set; }
        [JsonPropertyName("document")]
        public TelegramDocument document { get; set; }

        [JsonPropertyName("caption")]
        public string caption { get; set; }
    }

    public class User
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("is_bot")]
        public bool IsBot { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("language_code")]
        public string LanguageCode { get; set; }
    }

    public class Chat
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
    public class TelegramPhoto
    {
        [JsonPropertyName("file_id")]
        public string FileId { get; set; }

        [JsonPropertyName("file_unique_id")]
        public string FileUniqueId { get; set; }

        [JsonPropertyName("file_size")]
        public int FileSize { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }
    }
    public class TelegramVideo
    {
        [JsonPropertyName("file_id")]
        public string FileId { get; set; }

        [JsonPropertyName("file_unique_id")]
        public string FileUniqueId { get; set; }

        [JsonPropertyName("file_size")]
        public int FileSize { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("file_name")]
        public string FileName { get; set; }

        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }
    }
    public class TelegramDocument
    {
        [JsonPropertyName("file_id")]
        public string FileId { get; set; }

        [JsonPropertyName("file_unique_id")]
        public string FileUniqueId { get; set; }

        [JsonPropertyName("file_name")]
        public string FileName { get; set; }

        [JsonPropertyName("mime_type")]
        public string MimeType { get; set; }

        [JsonPropertyName("file_size")]
        public int FileSize { get; set; }
    }
}

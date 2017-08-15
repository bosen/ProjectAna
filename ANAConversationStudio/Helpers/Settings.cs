﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace ANAConversationStudio.Helpers
{
    public class Settings
    {
        const string FILE_NAME = "Settings.json";

        public List<ChatServerConnection> SavedChatServerConnections { get; set; } = new List<ChatServerConnection>();
        public EditableSettings UpdateDetails { get; set; } = new EditableSettings();
        public static bool IsEncrypted()
        {
            try
            {
                if (!File.Exists(FILE_NAME)) return false;

                JsonConvert.DeserializeObject<Settings>(File.ReadAllText(FILE_NAME));
                //If the settings file gets parsed without decryption, it means, the password is not set yet.
                return false;
            }
            catch
            {
                return true;
            }
        }
        public static Settings Load(string password)
        {
            if (File.Exists(FILE_NAME))
                return JsonConvert.DeserializeObject<Settings>(Utilities.Decrypt(File.ReadAllText(FILE_NAME), password));
            return new Settings();
        }

        public void Save(string password)
        {
            File.WriteAllText(FILE_NAME, Utilities.Encrypt(JsonConvert.SerializeObject(this), password));
        }
    }

    public class ChatServerConnection
    {
        [PropertyOrder(1)]
        public string Name { get; set; }
        [PropertyOrder(2)]
        public string ServerUrl { get; set; }
        
        [PropertyOrder(3)]
        public string APIKey { get; set; }
        [PropertyOrder(4)]
        public string APISecret { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Name))
                return Name;
            if (!string.IsNullOrWhiteSpace(ServerUrl))
                return ServerUrl;
            return "New Chat Server Connection";
        }

        public bool IsValid()
        {
            return Utilities.ValidateStrings(ServerUrl, Name);
        }
    }

    public class EditableSettings
    {
        public string StudioUpdateUrl { get; set; }
    }

    public class AutoUpdateResponse
    {
        public string DownloadLink { get; set; }
        public Version Version { get; set; }
    }
}

using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpevoCore.Services.Token_Service
{
    public class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        private const string cookie = "";

        #endregion

        public static string CookieString
        {
            get => AppSettings.GetValueOrDefault(cookie, string.Empty);
            set => AppSettings.AddOrUpdateValue(cookie, value);
        }

        public static void ClearAll()
        {
            AppSettings.Clear();
        }
    }
}

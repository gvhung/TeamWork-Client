﻿namespace TeamWork
{
    public static class AppSettings
    {
        public const string ApiAddress = "https://teamworkbackend.azurewebsites.net/";
        public const string NotificationHubConnectionString = "ENTER_YOUR_NOTIFICATION_HUB_CONNECTION_STRING_HERE";
        public const string NotificationHubPath = "ENTER_YOUR_NOTIFICATION_HUB_PATH_HERE";
        public const string EmotionApiKey = "6057ad581e854f70a612700b95f13bff";

#if __ANDROID__
        public const string AndroidProjectNumber = "ENTER_YOUR_ANDROID_PROJECT_NUMBER_HERE";
        public const string AndroidPackageId = "ENTER_YOUR_ANDROID_PACKAGE_ID_HERE";
#endif
    }
}

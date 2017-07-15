namespace threshold.Apis.VirusTotal.Requests
{
    public static class RequestFactory
    {
        private static string ApiKey
        {
            get
            {
                return Properties.Settings.Default.VirusTotalApiKey;
            }
        }

        public static bool IsApiKeyValid
        {
            get
            {
                if (ApiKey.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static MultiHashRequest GetMultiHashRequest()
        {
            return new MultiHashRequest(ApiKey);
        }
    }
}

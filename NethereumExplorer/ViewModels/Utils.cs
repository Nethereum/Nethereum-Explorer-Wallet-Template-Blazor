using System;
using Nethereum.Util;

namespace NethereumExplorer.ViewModels
{
    public static class Utils
    {
        private static AddressUtil _addressUtil = new AddressUtil();

        public static bool IsValidUrl(string url)
        {
            return !string.IsNullOrEmpty(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }

        public static bool IsValidAddress(string address)
        {
            return !string.IsNullOrEmpty(address) && _addressUtil.IsValidAddressLength(address);
        }

        public static string TruncateEllipse(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
    }
}

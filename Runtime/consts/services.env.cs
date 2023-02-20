using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotemConsts
{
    public static class ServicesEnv
    {
        #region Auth

        public const string AuthServiceUrl = "https://auth.totem.gdn/";
        public const string HttpListenerUrl = "http://localhost:6700/auth/";
        public const string AuthHttpResponseFileName = "HttpResponse";
        public const string HttpResultParameterName = "token";
        public const string TokenPlayerPrefsName = "lastTotemUserToken";
        public const string TokenComandLineArgName = "token";

        #endregion

        #region LegacyServices

        public const string AssetLegacyServicesUrl = "https://api.totem.gdn/asset-legacy";

        #endregion

        #region SimpleAPI

        public const string SimpleAPIItemsUrl = "https://simple-api.totem.gdn/default/items/";
        public const string SimpleAPIAvatarsUrl = "https://simple-api.totem.gdn/default/avatars/";

        #endregion

        #region Analytics

        public const string AnalyticsUrl = "https://api.mixpanel.com/track";
        public const int AnalyticsPublicKeyLength = 10;
        public const string AnalyticsToken = "08d75082c984e41c67886a71b58e0875";

        #endregion

        #region SmartContract

        public const string SmartContractUrl = "https://matic-mumbai.chainstacklabs.com";
        public const string SmartContractAvatars = "0x11dBDbF2e6D262c2fe7e73ace1A60c6862dC14dE";
        public const string SmartContractItems = "0xEc9C96eF9b90a950057EDbe40B42385f3b1cE78C";
        public const string SmartContractAvatarsFilterName = "totem-common-files/filters/totem-avatar";
        public const string SmartContractItemsFilterName = "totem-common-files/filters/totem-item";

        #endregion

        #region Debug

        public const string AssetsOverrideFilePath = "Totem/totem-assets-override.json";


        #endregion
    }
}

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
        public const string SmartContractAvatars = "0xEE7ff88E92F2207dBC19d89C1C9eD3F385513b35";
        public const string SmartContractItems = "0xfC5654489b23379ebE98BaF37ae7017130B45086";
        public const string SmartContractAvatarsFilterName = "totem-common-files/filters/totem-avatar";
        public const string SmartContractItemsFilterName = "totem-common-files/filters/totem-item";

        #endregion
    }
}

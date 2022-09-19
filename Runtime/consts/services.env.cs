using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotemConsts
{
    public static class ServicesEnv
    {
        #region Web3Auth

        public const string AccountGatewayUrl = "https://account.totem.gdn/";
        public const string HttpListenerUrl = "http://localhost:6700/auth/";
        public const string HttpResponseFileName = "HttpResponse";
        public const string HttpResultParametrName = "result";

        public const string Web3AuthClientId = "BDwa3_jZNrSf_QCV5PVnIsCwxiV0Yy4uAEBRs3lu1rNFK-QfYiHC5nWitVHoe3YRROjIKg3Bo-XR0KMyxDcxsm0";
        public const string Web3AuthRedirectUrl = "torusapp://com.torus.Web3AuthUnity/auth";
        public const string Web3AuthWhiteLabelName = "Web3Auth Totem";
        public const string Web3AuthWhiteLabeColor = "#123456";
        public const Web3Auth.Network Web3AuthNetwork = Web3Auth.Network.TESTNET;


        #endregion

        #region LegacyServices

        public const string LegacyServicesUrl = "https://legacy-api.totem.gdn/";

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
    }
}

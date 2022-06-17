using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace consts
{
    public static class ServicesEnv
    {
        #region AccountGateway

        public const string AccountGatewayUrl = "https://account.totem.gdn/";
        public const string HttpListenerUrl = "http://localhost:6700/auth/";
        public const string HttpResponseFileName = "HttpResponse";
        public const string HttpResultParametrName = "result";

        #endregion

        #region LegacyServices

        public const string LegacyServicesUrl = "https://legacy-api.totem.gdn/";

        #endregion

        #region SimpleAPI

        public const string SimpleAPIItemsUrl = "https://simple-api.totem.gdn/default/items/";
        public const string SimpleAPIAvatarsUrl = "https://simple-api.totem.gdn/default/items/";
        
        #endregion
    }
}

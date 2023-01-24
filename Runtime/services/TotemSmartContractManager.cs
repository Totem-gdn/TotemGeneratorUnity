using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using Nethereum.Unity.Rpc;
using Nethereum.Signer;
using TotemEntities;
using TotemEntities.DNA;
using TotemServices.DNA;
using TotemUtils;
using TotemConsts;

namespace TotemServices
{
    public class TotemSmartContractManager : MonoBehaviour
    {
        private Dictionary<object, BigInteger> assetIdTable;

        private void Awake()
        {
            assetIdTable = new Dictionary<object, BigInteger>();
        }

        public void GetAvatars<T>(TotemUser user, TotemDNAFilter filter, UnityAction<List<T>> onComplete) where T : new()
        {
            StartCoroutine(GetAvatarsCoroutine(user.PublicKey, filter, onComplete));
        }

        private IEnumerator GetAvatarsCoroutine<T>(string publicKey, TotemDNAFilter filter, UnityAction<List<T>> onCompelte) where T : new()
        {
            var avatarsList = new List<T>();

            var key = new EthECKey(Convert.HexToByteArray(publicKey), false);
            string address = key.GetPublicAddress();

            var balanceQuery = new QueryUnityRequest<BalanceOfFunction, BalanceOfOutputDTO>(ServicesEnv.SmartContractUrl, address);
            yield return balanceQuery.Query(new BalanceOfFunction() { Owner = address }, ServicesEnv.SmartContractAvatars);

            var ownedAvatarsCount = balanceQuery.Result.ReturnValue1;
            for (int i = 0; i < ownedAvatarsCount; i++)
            {
                var tokenIdRequest = new QueryUnityRequest<TokenOfOwnerByIndexFunction, TokenOfOwnerByIndexOutputDTO>(ServicesEnv.SmartContractUrl, address);
                yield return tokenIdRequest.Query(new TokenOfOwnerByIndexFunction() { Owner = address, Index = i }, ServicesEnv.SmartContractAvatars);

                var avatarRequest = new QueryUnityRequest<TokenURIFunction, TokenURIOutputDTO>(ServicesEnv.SmartContractUrl, address);
                var tokenId = tokenIdRequest.Result.ReturnValue1;
                yield return avatarRequest.Query(new TokenURIFunction() { TokenId = tokenIdRequest.Result.ReturnValue1 }, ServicesEnv.SmartContractAvatars);

                string binaryDna = Convert.HexStringToBinary(avatarRequest.Result.ReturnValue1);

                var avatar = filter.FilterDNA<T>(binaryDna);
                avatarsList.Add(avatar);
                AddAssetToIdTable(avatar, tokenId);
            }

            onCompelte?.Invoke(avatarsList);
        }


        public void GetAvatar<T>(TotemUser user, TotemDNAFilter filter, BigInteger assetId, UnityAction<T> onComplete) where T : new()
        {
            StartCoroutine(GetAvatarCoroutine(user.PublicKey, filter, assetId, onComplete));
        }

        private IEnumerator GetAvatarCoroutine<T>(string publicKey, TotemDNAFilter filter, BigInteger assetId, UnityAction<T> onCompelte) where T : new()
        {
            var key = new EthECKey(Convert.HexToByteArray(publicKey), false);
            string address = key.GetPublicAddress();

            var avatarRequest = new QueryUnityRequest<TokenURIFunction, TokenURIOutputDTO>(ServicesEnv.SmartContractUrl, address);
            var tokenId = assetId;
            yield return avatarRequest.Query(new TokenURIFunction() { TokenId = tokenId }, ServicesEnv.SmartContractAvatars);

            string binaryDna = Convert.HexStringToBinary(avatarRequest.Result.ReturnValue1);

            var avatar = filter.FilterDNA<T>(binaryDna);
            AddAssetToIdTable(avatar, tokenId);

            onCompelte?.Invoke(avatar);
        }



        public void GetItems<T>(TotemUser user, TotemDNAFilter filter, UnityAction<List<T>> onCompelte) where T : new()
        {
            StartCoroutine(GetItemsCoroutine<T>(user.PublicKey, filter, onCompelte));
        }

        private IEnumerator GetItemsCoroutine<T>(string publicKey, TotemDNAFilter filter, UnityAction<List<T>> onCompelte) where T : new()
        {
            var itemsList = new List<T>();

            var key = new EthECKey(Convert.HexToByteArray(publicKey), false);
            string address = key.GetPublicAddress();

            var balanceQuery = new QueryUnityRequest<BalanceOfFunction, BalanceOfOutputDTO>(ServicesEnv.SmartContractUrl, address);
            yield return balanceQuery.Query(new BalanceOfFunction() { Owner = address }, ServicesEnv.SmartContractItems);

            var ownedItemsCount = balanceQuery.Result.ReturnValue1;
            for (int i = 0; i < ownedItemsCount; i++)
            {
                var tokenIdRequest = new QueryUnityRequest<TokenOfOwnerByIndexFunction, TokenOfOwnerByIndexOutputDTO>(ServicesEnv.SmartContractUrl, address);
                yield return tokenIdRequest.Query(new TokenOfOwnerByIndexFunction() { Owner = address, Index = i }, ServicesEnv.SmartContractItems);

                var itemRequest = new QueryUnityRequest<TokenURIFunction, TokenURIOutputDTO>(ServicesEnv.SmartContractUrl, address);
                var tokenId = tokenIdRequest.Result.ReturnValue1;
                yield return itemRequest.Query(new TokenURIFunction() { TokenId = tokenId }, ServicesEnv.SmartContractItems);

                string binaryDna = Convert.HexStringToBinary(itemRequest.Result.ReturnValue1);
                var item = filter.FilterDNA<T>(binaryDna);
                itemsList.Add(item);
                AddAssetToIdTable(item, tokenId);
            }

            onCompelte.Invoke(itemsList);
        }

        public void GetItem<T>(TotemUser user, TotemDNAFilter filter, BigInteger assetId, UnityAction<T> onComplete) where T : new()
        {
            StartCoroutine(GetItemCoroutine(user.PublicKey, filter, assetId, onComplete));
        }

        private IEnumerator GetItemCoroutine<T>(string publicKey, TotemDNAFilter filter, BigInteger assetId, UnityAction<T> onCompelte) where T : new()
        {
            var key = new EthECKey(Convert.HexToByteArray(publicKey), false);
            string address = key.GetPublicAddress();

            var itemRequest = new QueryUnityRequest<TokenURIFunction, TokenURIOutputDTO>(ServicesEnv.SmartContractUrl, address);
            var tokenId = assetId;
            yield return itemRequest.Query(new TokenURIFunction() { TokenId = tokenId }, ServicesEnv.SmartContractItems);

            string binaryDna = Convert.HexStringToBinary(itemRequest.Result.ReturnValue1);

            var item = filter.FilterDNA<T>(binaryDna);
            AddAssetToIdTable(item, tokenId);

            onCompelte?.Invoke(item);
        }


        private void AddAssetToIdTable(object asset, BigInteger id)
        {
            assetIdTable.Add(asset, id);
        }

        public BigInteger GetAssetId(object asset)
        {
            if (asset == null)
            {
                return -1;
            }

            if (!assetIdTable.ContainsKey(asset))
                return -1;

            return assetIdTable[asset];
        }

    }
}

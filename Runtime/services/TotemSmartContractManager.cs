using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nethereum.Unity.Rpc;
using Nethereum.Signer;
using TotemEntities;
using TotemEntities.DNA;
using TotemServices.DNA;
using TotemUtils.DNA;
using TotemConsts;

namespace TotemServices
{
    public class TotemSmartContractManager : MonoBehaviour
    {
        private string privateKey = "26ce2d1e9f8dbcf570eafc08271aa7018e602e1b8611624db53a93782e554625";

        private void Start()
        {
            StartCoroutine(GetAvatarsCoroutine<TotemDNAAvatar>(privateKey, new TotemDNAFilter(Resources.Load<TextAsset>("avatar-filter").text)));
            StartCoroutine(GetItemsCoroutine<TotemDNAItem>(privateKey, new TotemDNAFilter(Resources.Load<TextAsset>("item-filter").text)));
        }


        public void GetAvatars<T>(TotemUser user, TotemDNAFilter filter) where T : new()
        {
            StartCoroutine(GetAvatarsCoroutine<T>(user.PrivateKey, filter));
        }
        private IEnumerator GetAvatarsCoroutine<T>(string privateKey, TotemDNAFilter filter) where T : new()
        {
            var key = new EthECKey(privateKey);
            string address = key.GetPublicAddress();

            var balanceQuery = new QueryUnityRequest<BalanceOfFunction, BalanceOfOutputDTO>(ServicesEnv.SmartContractUrl, address);
            yield return balanceQuery.Query(new BalanceOfFunction() { Owner = address }, ServicesEnv.SmartContractAvatars);

            var ownedAvatarsCount = balanceQuery.Result.ReturnValue1;
            for (int i = 0; i < ownedAvatarsCount; i++)
            {
                var tokenIdRequest = new QueryUnityRequest<TokenOfOwnerByIndexFunction, TokenOfOwnerByIndexOutputDTO>(ServicesEnv.SmartContractUrl, address);
                yield return tokenIdRequest.Query(new TokenOfOwnerByIndexFunction() { Owner = address, Index = i }, ServicesEnv.SmartContractAvatars);

                var avatarRequest = new QueryUnityRequest<TokenURIFunction, TokenURIOutputDTO>(ServicesEnv.SmartContractUrl, address);
                yield return avatarRequest.Query(new TokenURIFunction() { TokenId = tokenIdRequest.Result.ReturnValue1 }, ServicesEnv.SmartContractAvatars);

                string binaryDna = Convert.HexStringToBinary(avatarRequest.Result.ReturnValue1);
                var avatar = filter.FilterDNA<T>(binaryDna);
                Debug.Log(avatar.ToString());
            }

        }



        public void GetItems<T>(TotemUser user, TotemDNAFilter filter) where T : new()
        {
            StartCoroutine(GetItemsCoroutine<T>(user.PrivateKey, filter));
        }
        private IEnumerator GetItemsCoroutine<T>(string privateKey, TotemDNAFilter filter) where T : new()
        {
            var key = new EthECKey(privateKey);
            string address = key.GetPublicAddress();

            var balanceQuery = new QueryUnityRequest<BalanceOfFunction, BalanceOfOutputDTO>(ServicesEnv.SmartContractUrl, address);
            yield return balanceQuery.Query(new BalanceOfFunction() { Owner = address }, ServicesEnv.SmartContractItems);

            var ownedItemsCount = balanceQuery.Result.ReturnValue1;
            for (int i = 0; i < ownedItemsCount; i++)
            {
                var tokenIdRequest = new QueryUnityRequest<TokenOfOwnerByIndexFunction, TokenOfOwnerByIndexOutputDTO>(ServicesEnv.SmartContractUrl, address);
                yield return tokenIdRequest.Query(new TokenOfOwnerByIndexFunction() { Owner = address, Index = i }, ServicesEnv.SmartContractItems);

                var itemRequest = new QueryUnityRequest<TokenURIFunction, TokenURIOutputDTO>(ServicesEnv.SmartContractUrl, address);
                yield return itemRequest.Query(new TokenURIFunction() { TokenId = tokenIdRequest.Result.ReturnValue1 }, ServicesEnv.SmartContractItems);

                string binaryDna = Convert.HexStringToBinary(itemRequest.Result.ReturnValue1);
                var item = filter.FilterDNA<T>(binaryDna);
                Debug.Log(item.ToString());
            }

        }

    }
}

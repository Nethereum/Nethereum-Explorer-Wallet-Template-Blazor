﻿using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using NethereumExplorer.ViewModels;

namespace NethereumExplorer.Services
{
    public class Web3ProviderService: IWeb3ProviderService
    {
        public string CurrentUrl { get; set; } = "https://mainnet.infura.io/v3/ddd5ed15e8d443e295b696c0d07c8b02";

        //TODO: Simple chainId workaround, this should be the ChainId from the connection, when adding the url we should get the chainId using rpc and add it here.
        public string ChainId => CurrentUrl;

        public Web3 GetWeb3()
        {
            if (Utils.IsValidUrl(CurrentUrl))
            {
                return new Web3(CurrentUrl);
            }

            return null;
        }

        public Web3 GetWeb3(Account account)
        {
            if (Utils.IsValidUrl(CurrentUrl))
            {
                return new Web3(account, CurrentUrl);
            }

            return null;
        }
    }
}

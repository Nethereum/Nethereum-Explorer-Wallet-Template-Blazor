using System.Collections.Generic;

namespace NethereumBlazor.Services
{
    public class TokenAddressService
    {
        public List<TokenInfo> GetTokens(string chainId)
        {
            //hack / workaround chainId this needs to use the rpc getchainid
            if (chainId.ToLower() == "https://mainnet.infura.io/v3/7238211010344719ad14a89db874158c".ToLower())
            {
                return new List<TokenInfo>(
                    new []
                    {
                        new TokenInfo(){Address = "0x9f8f72aa9304c8b593d555f12ef6589cc3a579a2", Description = "Maker"},
                        new TokenInfo(){Address = "0x89d24a6b4ccb1b6faa2625fe562bdd9a23260359", Description = "Dai"},
                        new TokenInfo(){Address = "0x42d6622dece394b54999fbd73d108123806f6a18", Description = "Spank"},
                        new TokenInfo(){Address = "0x6810e776880c02933d47db1b9fc05908e5386b96", Description = "Gnosis"},
                        new TokenInfo(){Address = "0x1985365e9f78359a9B6AD760e32412f4a445E862", Description = "Augur"},
                         
                    }
                );
            }
            return new List<TokenInfo>();
        }
    }
}

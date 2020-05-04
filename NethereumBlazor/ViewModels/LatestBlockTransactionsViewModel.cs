using System;
using NethereumBlazor.Messages;
using NethereumBlazor.Services;
using ReactiveUI;

namespace NethereumBlazor.ViewModels
{
    public class LatestBlockTransactionsViewModel : BlockTransactionsViewModel
    {
        public LatestBlockTransactionsViewModel(IWeb3ProviderService web3ProviderService = null) : base(web3ProviderService)
        {
            MessageBus.Current.Listen<NewBlock>().Subscribe(x =>
                {
                    if (x.BlockNumber != BlockNumber)
                    {
                        BlockNumber = x.BlockNumber;
                    }
                }
           );
        }
    }
}

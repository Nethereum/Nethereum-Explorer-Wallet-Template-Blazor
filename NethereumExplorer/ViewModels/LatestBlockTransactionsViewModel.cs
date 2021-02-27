using System;
using NethereumExplorer.Messages;
using NethereumExplorer.Services;
using ReactiveUI;

namespace NethereumExplorer.ViewModels
{
    public class LatestBlockTransactionsViewModel : BlockTransactionsViewModel
    {
        public LatestBlockTransactionsViewModel(IWeb3ProviderService web3ProviderService):base(web3ProviderService)
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

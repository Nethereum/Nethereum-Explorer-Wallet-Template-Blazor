using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Nethereum.Hex.HexTypes;
using NethereumBlazor.Messages;
using NethereumBlazor.Services;
using ReactiveUI;

namespace NethereumBlazor.ViewModels
{
    public class BlockTransactionsViewModel : ReactiveObject, IDisposable
    {
        private readonly IWeb3ProviderService _web3ProviderService;
        private BigInteger? _blockNumber;
        private BlockViewModel _block;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private bool _disposed = false;
        private bool _loading;
        private bool _blockFound;

        public BlockTransactionsViewModel(IWeb3ProviderService web3ProviderService)
        {
            _web3ProviderService = web3ProviderService;
            _block = new BlockViewModel();
            MessageBus.Current.Listen<UrlChanged>().Subscribe(async x =>
                {
                    Loading = true;
                    try
                    {
                        await _semaphoreSlim.WaitAsync().ConfigureAwait(false);
                        BlockNumber = _blockNumber;
                    }
                    finally
                    {
                        Loading = false;
                        _semaphoreSlim.Release();
                    }
                }
            );

            this.WhenAnyValue(x => x.BlockNumber).Subscribe(async _ =>
                GetBlockTransactionsAsync());
        }

        public bool Loading
        {
            get { return _loading; }
            set { this.RaiseAndSetIfChanged(ref _loading, value); }
        }

        public bool BlockFound
        {
            get { return _blockFound; }
            set { this.RaiseAndSetIfChanged(ref _blockFound, value); }
        }

        public BigInteger? BlockNumber
        {
            get => _blockNumber;
            set => this.RaiseAndSetIfChanged(ref _blockNumber, value);
        }

        public BlockViewModel Block
        {
            get => _block;
            set => this.RaiseAndSetIfChanged(ref _block, value);
        }

        public SourceCache<TransactionViewModel, string> Transactions { get; set; } =
            new SourceCache<TransactionViewModel, string>(x => x.TransactionHash);


        public async Task GetBlockTransactionsAsync()
        {
            var transactionViewModels = new List<TransactionViewModel>();
            try
            {
                Loading = true;
                await _semaphoreSlim.WaitAsync().ConfigureAwait(false);

                var web3 = _web3ProviderService.GetWeb3();
                if (web3 != null && BlockNumber != null)
                {
                    var blockWithTransactions = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber
                        .SendRequestAsync(new HexBigInteger(BlockNumber.Value)).ConfigureAwait(false);

                    if (blockWithTransactions == null)
                    {
                        BlockFound = false;
                        Loading = false;
                    }
                    else
                    {
                        BlockFound = true;
                        Block = new BlockViewModel(blockWithTransactions);
                        foreach (var transaction in blockWithTransactions.Transactions)
                        {
                            var txnTransactionViewModel = new TransactionViewModel();
                            txnTransactionViewModel.Initialise(transaction);
                            transactionViewModels.Add(txnTransactionViewModel);
                        }
                    }
                }
            }
            finally
            {
                Transactions.Edit(innerList =>
                {
                    innerList.Clear();
                    innerList.AddOrUpdate(transactionViewModels);
                    Loading = false;
                    
                });

                _semaphoreSlim.Release();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _semaphoreSlim?.Dispose();
            }

            _disposed = true;
        }
    }
}

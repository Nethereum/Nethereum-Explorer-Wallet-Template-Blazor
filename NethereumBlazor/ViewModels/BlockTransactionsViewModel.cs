using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Nethereum.Hex.HexTypes;
using NethereumBlazor.Messages;
using NethereumBlazor.Services;
using ReactiveUI;
using Splat;

namespace NethereumBlazor.ViewModels
{
    public class BlockTransactionsViewModel : ReactiveObject, IDisposable
    {
        private readonly IWeb3ProviderService _web3ProviderService;
        private BigInteger? _blockNumber;
        private BlockViewModel _block = new BlockViewModel();
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private bool _disposed = false;
        private bool _loading;
        private bool _blockFound;

        public BlockTransactionsViewModel(IWeb3ProviderService web3ProviderService = null)
        {
            _web3ProviderService = web3ProviderService ?? Locator.Current.GetService<IWeb3ProviderService>();
            MessageBus.Current.Listen<UrlChanged>().Select(x => ReloadTransactions().ToObservable()).Concat().Subscribe();

            this.WhenAnyValue(x => x.BlockNumber).Select(x =>
                GetBlockTransactionsAsync().ToObservable()).Concat().Subscribe();
        }

        private async Task ReloadTransactions()
        {
            Loading = true;
            try
            {
                await _semaphoreSlim.WaitAsync().ConfigureAwait(false);
                Transactions.Clear();
                BlockNumber = _blockNumber;
            }
            finally
            {
                Loading = false;
                _semaphoreSlim.Release();
            }
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
            //Hack / Workaround prevent crashing on start up
            if (BlockNumber != null)
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
                catch
                {
                    //hacky graceful catch
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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using NethereumBlazor.Messages;
using ReactiveUI;

namespace NethereumBlazor.Services
{
    public class NewBlockProcessingService : ReactiveObject
    {
        private readonly IWeb3ProviderService _web3ProviderService;
        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(10000);
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private IDisposable _timer;
        private ulong _blockNumber;
        private bool _loading;

        public SourceCache<BlockWithTransactionHashes, string> Blocks { get; set; } =
            new SourceCache<BlockWithTransactionHashes, string>(t => t.Number.Value.ToString(CultureInfo.InvariantCulture));

        public bool Loading
        {
            get { return _loading; }
            set { this.RaiseAndSetIfChanged(ref _loading, value); }
        }

        public ulong BlockNumber
        {
            get => _blockNumber;
            set => this.RaiseAndSetIfChanged(ref _blockNumber, value);
        }

        public NewBlockProcessingService(IWeb3ProviderService web3ProviderService)
        {
            _web3ProviderService = web3ProviderService;
            MessageBus.Current.Listen<UrlChanged>().Subscribe(async x =>
                {
                    Loading = true;
                    try
                    {
                        await _semaphoreSlim.WaitAsync().ConfigureAwait(false);
                        Blocks.Clear();
                        CreateBlockWatcher();

                    }
                    finally
                    {
                        _semaphoreSlim.Release();
                    }
                }
            );

            CreateBlockWatcher();
        }

        public void CreateBlockWatcher()
        {
            //This deadlocks the ui when using Async,I assume is threading
            //_timer = Observable.Timer(TimeSpan.FromMilliseconds(500), _updateInterval, RxApp.MainThreadScheduler)
            //    .Select(_ => Observable.FromAsync(GetLatestBlocksAsync)).Subscribe();

            _timer = Observable.Timer(TimeSpan.FromMilliseconds(500), _updateInterval, RxApp.MainThreadScheduler)
                .Subscribe(async _ => await GetLatestBlocksAsync().ConfigureAwait(false));
        }

        public async Task GetLatestBlocksAsync()
        {
            try
            {
                await _semaphoreSlim.WaitAsync().ConfigureAwait(false);

                var web3 = _web3ProviderService.GetWeb3();
                if (web3 != null)
                {
                    var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync().ConfigureAwait(false);

                    var lastBlockNumber = blockNumber.Value - 10;

                    if (Blocks.Count > 0)
                    {
                        lastBlockNumber = Blocks.Items.OrderByDescending(x => x.Number.Value).First().Number;
                    }

                    if (lastBlockNumber < blockNumber.Value)
                    {
                        Loading = true;
                        var blocksToAdd = new List<BlockWithTransactionHashes>();
                        while (lastBlockNumber <= blockNumber.Value)
                        {
                            var block = await web3.Eth.Blocks.GetBlockWithTransactionsHashesByNumber
                                .SendRequestAsync(new HexBigInteger(lastBlockNumber)).ConfigureAwait(false);
                            blocksToAdd.Add(block);
                            lastBlockNumber = lastBlockNumber + 1;
                        }

                        BlockNumber = (ulong)blockNumber.Value;
                        MessageBus.Current.SendMessage(new NewBlock(blockNumber.Value));
                        Blocks.Edit(innerList =>
                            {
                                innerList.AddOrUpdate(blocksToAdd);
                                var blocksCount = Blocks.Count;

                                var keysToRemove = new List<string>();
                                if (blocksCount > 10)
                                {
                                    for (int i = 11; i <= blocksCount; i++)
                                    {
                                        keysToRemove.Add(
                                            (blockNumber.Value - i).ToString(CultureInfo.InvariantCulture));
                                    }
                                }

                                innerList.RemoveKeys(keysToRemove);
                            }
                        );

                    }
                }
            }
            finally
            {
                Loading = false;
                _semaphoreSlim.Release();
            }

        }
    }
}

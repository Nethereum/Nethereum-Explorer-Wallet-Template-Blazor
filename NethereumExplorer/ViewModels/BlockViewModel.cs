using System;
using System.Numerics;
using Nethereum.RPC.Eth.DTOs;
using ReactiveUI;

namespace NethereumExplorer.ViewModels
{
    public class BlockViewModel : ReactiveObject
    {
        private BigInteger _number;
        private string _author;
        private string _hash;
        private string _parentHash;
        private BigInteger _gasUsed;
        private BigInteger _difficulty;
        private int _transactionCount;
        private DateTime _time;

        public BlockViewModel(BlockWithTransactionHashes block)
        {
            Initialise(block);
            _transactionCount = block.TransactionHashes.Length;
        }

        public BlockViewModel(BlockWithTransactions block)
        {
            Initialise(block);
            _transactionCount = block.Transactions.Length;
        }


        public BlockViewModel()
        {
          
        }
        public void Initialise(Block block)
        {
            _difficulty = block.Difficulty.Value;
            _hash = block.BlockHash;
            _gasUsed =  block.GasUsed.Value;
            _parentHash = block.ParentHash;
            _number = block.Number.Value;
            _author = block.Miner;
            _time = UnixTimeStampToDateTime((int)block.Timestamp.Value);
        }

        public string Hash
        {
            get { return _hash; }
            set { this.RaiseAndSetIfChanged(ref _hash, value); }
        }

        public string ParentHash
        {
            get { return _parentHash; }
            set { this.RaiseAndSetIfChanged(ref _parentHash, value); }
        }

        public BigInteger Difficulty
        {
            get { return _difficulty; }
            set { this.RaiseAndSetIfChanged(ref _difficulty, value); }
        }

        public BigInteger GasUsed
        {
            get { return _gasUsed; }
            set { this.RaiseAndSetIfChanged(ref _gasUsed, value); }
        }

        public BigInteger Number
        {
            get { return _number; }
            set { this.RaiseAndSetIfChanged(ref _number, value); }
        }

        public string Author
        {
            get { return _author; }
            set { this.RaiseAndSetIfChanged(ref _author, value); }
        }

        public int TransactionCount
        {
            get { return _transactionCount; }
            set { this.RaiseAndSetIfChanged(ref _transactionCount, value); }
        }

        public DateTime Time
        {
            get { return _time; }
            set { this.RaiseAndSetIfChanged(ref _time, value); }
        }

        public DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).DateTime;
        }

    }
}

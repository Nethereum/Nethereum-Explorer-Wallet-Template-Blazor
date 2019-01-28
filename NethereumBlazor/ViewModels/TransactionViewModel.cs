using System.Numerics;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using ReactiveUI;

namespace NethereumBlazor.ViewModels
{
    public class TransactionViewModel : ReactiveObject
    {
        private decimal _amount;
        private ulong _index;
        private string _blockHash;
        private string _data;
        private string _from;
        private BigInteger? _gas;
        private BigInteger? _gasPrice;
        private ulong _nonce;
        private string _to;
        private ulong _blockNumber;
        private string _transactionHash;

        
        public ulong BlockNumber

        {
            get => _blockNumber;

            set => this.RaiseAndSetIfChanged(ref _blockNumber, value);
        }

        public string BlockHash

        {
            get => _blockHash;

            set => this.RaiseAndSetIfChanged(ref _blockHash, value);
        }

        public string TransactionHash

        {
            get => _transactionHash;

            set => this.RaiseAndSetIfChanged(ref _transactionHash, value);
        }


        public ulong Index

        {
            get => _index;

            set => this.RaiseAndSetIfChanged(ref _index, value);
        }

        public string From

        {
            get => _from;

            set => this.RaiseAndSetIfChanged(ref _from, value);
        }

        public string To

        {
            get => _to;

            set => this.RaiseAndSetIfChanged(ref _to, value);
        }

        public decimal Amount

        {
            get => _amount;

            set => this.RaiseAndSetIfChanged(ref _amount, value);
        }

        public BigInteger? Gas
        {
            get => _gas;

            set => this.RaiseAndSetIfChanged(ref _gas, value);
        }

        public string Data
        {
            get => _data;

            set => this.RaiseAndSetIfChanged(ref _data, value);
        }

        public BigInteger? GasPrice

        {
            get => _gasPrice;

            set => this.RaiseAndSetIfChanged(ref _gasPrice, value);
        }

        public ulong Nonce
        {
            get => _nonce;

            set => this.RaiseAndSetIfChanged(ref _nonce, value);
        }

        public void Initialise(Transaction transaction)

        {
            TransactionHash = transaction.TransactionHash;

            BlockNumber = (ulong)transaction.BlockNumber.Value;

            BlockHash = transaction.BlockHash;

            Nonce = (ulong)transaction.Nonce.Value;

            From = transaction.From;

            To = transaction.To;

            Gas = transaction.Gas;

            GasPrice = transaction.GasPrice;

            Data = transaction.Input;

            Index = (ulong)transaction.TransactionIndex.Value;

            if (transaction.Value != null)
            {
                Amount = Web3.Convert.FromWei(transaction.Value.Value);
            }
        }
    }
}

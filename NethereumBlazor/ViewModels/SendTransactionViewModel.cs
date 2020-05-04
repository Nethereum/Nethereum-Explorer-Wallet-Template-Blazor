using System.Reactive;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using NethereumBlazor.Services;
using ReactiveUI;

namespace NethereumBlazor.ViewModels
{
    public class SendTransactionViewModel : SendTransactionBaseViewModel
    {
        private ObservableAsPropertyHelper<string> _latestTransactionHash;

        public string LatestTransactionHash { get => _latestTransactionHash.Value; }

        public ReactiveCommand<Unit, Unit> RefreshBalanceCommand { get; }

        public ReactiveCommand<Unit, string> SendTransactionCommand { get; }

        public SendTransactionViewModel(IAccountsService accountsService = null) : base(accountsService)
        {
            RefreshBalanceCommand = ReactiveCommand.CreateFromTask(RefreshBalanceAsync, outputScheduler: RxApp.TaskpoolScheduler);
            SendTransactionCommand = ReactiveCommand.CreateFromTask(SendTransactionAsync, outputScheduler: RxApp.TaskpoolScheduler);

            _latestTransactionHash = SendTransactionCommand.ToProperty(this, nameof(LatestTransactionHash));
        }

        private async Task<string> SendTransactionAsync()
        {
            var transactionInput =
                new TransactionInput
                {
                    Value = new HexBigInteger(Web3.Convert.ToWei(AmountInEther)),
                    To = AddressTo,
                    From = Account
                };
            if (Gas != null)
                transactionInput.Gas = new HexBigInteger(Gas.Value);
            if (!string.IsNullOrEmpty(GasPrice))
            {
                var parsed = decimal.Parse(GasPrice);
                transactionInput.GasPrice = new HexBigInteger(Web3.Convert.ToWei(GasPrice, UnitConversion.EthUnit.Gwei));
            }

            if (Nonce != null)
                transactionInput.Nonce = new HexBigInteger(Nonce.Value);
            if (!string.IsNullOrEmpty(Data))
                transactionInput.Data = Data;

            return await AccountsService.SendTransactionAsync(transactionInput).ConfigureAwait(false);
        }
    }
}

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using Nethereum.Web3;
using NethereumBlazor.Messages;
using NethereumBlazor.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace NethereumBlazor.ViewModels
{
    public class SendErc20TransactionViewModel : SendTransactionBaseViewModel
    {
        private ObservableAsPropertyHelper<string> _latestTransactionHash;

        [Reactive]
        public string ContractAddress { get; set; }

        [Reactive]
        public string TransferTo { get; set; }

        [Reactive]
        public decimal TokenAmount { get; set; }

        [Reactive]
        public decimal TokenBalance { get; set; }

        [Reactive]
        public int DecimalPlaces { get; set; }

        public string LatestTransactionHash { get => _latestTransactionHash.Value; }

        public ReactiveCommand<Unit, Unit> RefreshBalanceCommand { get; }

        public ReactiveCommand<Unit, Unit> RefreshTokenBalanceCommand { get; }

        public ReactiveCommand<Unit, string> SendTokenCommand { get; }

        public SendErc20TransactionViewModel(IAccountsService accountsService = null) : base(accountsService)
        {
            DecimalPlaces = 18;

            this.WhenAnyValue(x => x.Account, x => x.ContractAddress, (x, y) => !string.IsNullOrEmpty(x) && !string.IsNullOrEmpty(y)).Select(_ =>
                  RefreshTokenBalanceAsync().ToObservable()).Concat().Subscribe();

            MessageBus.Current.Listen<UrlChanged>().Select(_ => RefreshTokenBalanceAsync().ToObservable()).Concat().Subscribe();

            RefreshBalanceCommand = ReactiveCommand.CreateFromTask(RefreshBalanceAsync, outputScheduler: RxApp.TaskpoolScheduler);
            RefreshTokenBalanceCommand = ReactiveCommand.CreateFromTask(RefreshTokenBalanceAsync, outputScheduler: RxApp.TaskpoolScheduler);
            SendTokenCommand = ReactiveCommand.CreateFromTask(SendTokenAsync, outputScheduler: RxApp.TaskpoolScheduler);

            _latestTransactionHash = SendTokenCommand.ToProperty(this, nameof(LatestTransactionHash));
        }

        private async Task RefreshTokenBalanceAsync()
        {
            if (!string.IsNullOrWhiteSpace(Account) && !string.IsNullOrWhiteSpace(ContractAddress))
            {
                TokenBalance = await AccountsService.GetAccountTokenBalanceAsync(Account, ContractAddress, DecimalPlaces);
            }
        }

        private async Task<string> SendTokenAsync()
        {
            var transferFunction =
                new TransferFunction()
                {
                    AmountToSend = new HexBigInteger(Web3.Convert.ToWei(AmountInEther)),
                    To = TransferTo,
                    FromAddress = Account,
                    Value = Web3.Convert.ToWei(TokenAmount, DecimalPlaces)
                };
            if (Gas != null)
                transferFunction.Gas = new HexBigInteger(Gas.Value);
            if (!string.IsNullOrEmpty(GasPrice))
            {
                var parsed = decimal.Parse(GasPrice);
                transferFunction.GasPrice = new HexBigInteger(Web3.Convert.ToWei(GasPrice, UnitConversion.EthUnit.Gwei));
            }

            if (Nonce != null)
                transferFunction.Nonce = new HexBigInteger(Nonce.Value);


            return await AccountsService.SendTransactionAsync(ContractAddress, transferFunction).ConfigureAwait(false);
        }
    }
}
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using Nethereum.Web3;
using NethereumExplorer.Messages;
using NethereumExplorer.Services;
using ReactiveUI;

namespace NethereumExplorer.ViewModels
{
    public class SendErc20TransactionViewModel : SendTransactionBaseViewModel
    {
        private string _contractAddress;

        public string ContractAddress
        {
            get => _contractAddress;
            set => this.RaiseAndSetIfChanged(ref _contractAddress, value);
        }

        private string _transferTo;

        public string TransferTo
        {
            get => _transferTo;
            set => this.RaiseAndSetIfChanged(ref _transferTo, value);
        }

        private decimal _tokenAmount;

        public decimal TokenAmount
        {
            get => _tokenAmount;
            set => this.RaiseAndSetIfChanged(ref _tokenAmount, value);
        }


        private decimal _tokenBalance;

        public decimal TokenBalance
        {
            get => _tokenBalance;
            set => this.RaiseAndSetIfChanged(ref _tokenBalance, value);
        }

        private int _decimalPlaces;

        public int DecimalPlaces
        {
            get => _decimalPlaces;
            set => this.RaiseAndSetIfChanged(ref _decimalPlaces, value);
        }

        protected ReactiveCommand<Unit, string> _executeTrasnactionCommand;
        //Does not work / threading issue
        public ReactiveCommand<Unit, string> ExecuteTransactionCommand => this._executeTrasnactionCommand;

        public SendErc20TransactionViewModel(IAccountsService accountsService) : base(accountsService)
        {
            _decimalPlaces = 18;

            this.WhenAnyValue(x => x.Account, x => x.ContractAddress, (x,y) => !string.IsNullOrEmpty(x) && !string.IsNullOrEmpty(y)).Select(_ =>
                 RefreshTokenBalanceAsync().ToObservable()).Concat().Subscribe();

            MessageBus.Current.Listen<UrlChanged>().Select( _=> RefreshTokenBalanceAsync().ToObservable()).Concat().Subscribe();
            /* Does not work threading
            var canExecuteTransaction = this.WhenAnyValue(
                x => x.AddressTo,
                x => x.Account,
                (addressTo, account) =>
                    Utils.IsValidAddress(addressTo) &&
                    Utils.IsValidAddress(account));


            this._executeTrasnactionCommand = ReactiveCommand.CreateFromTask(SendTokenAsync, canExecuteTransaction);
            */
        }

        public async Task RefreshTokenBalanceAsync()
        {
            if (!string.IsNullOrWhiteSpace(Account) && !string.IsNullOrWhiteSpace(ContractAddress))
            {
                TokenBalance = await AccountsService.GetAccountTokenBalanceAsync(Account, ContractAddress, DecimalPlaces);
            }
        }

        public async Task<string> SendTokenAsync()
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
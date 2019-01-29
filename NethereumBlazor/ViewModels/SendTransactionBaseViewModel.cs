using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using DynamicData;
using NethereumBlazor.Messages;
using NethereumBlazor.Model;
using NethereumBlazor.Services;
using ReactiveUI;

namespace NethereumBlazor.ViewModels
{
    public class SendTransactionBaseViewModel : ReactiveObject
    {
        protected IAccountsService AccountsService { get; }

        public SendTransactionBaseViewModel(IAccountsService accountsService)
        {
            AccountsService = accountsService;
            this.WhenAnyValue(x => x.Account, (x) => !string.IsNullOrEmpty(x)).Select(_=>RefreshBalanceAsync().ToObservable()).Concat().Subscribe();
            
            MessageBus.Current.Listen<UrlChanged>().Select(_ => RefreshBalanceAsync().ToObservable()).Concat().Subscribe();

            AccountsService.Accounts.Connect().Subscribe(x => { SelectFirstAccount(); });

            SelectFirstAccount();
        }

        private void SelectFirstAccount()
        {
            if (string.IsNullOrEmpty(Account) || !AccountsService.Accounts.Keys.Contains(Account.ToLowerInvariant()))
            {
                var firstAccount = AccountsService.Accounts.Items.FirstOrDefault();
                if (firstAccount != null)
                {
                    Account = firstAccount.Address;
                }
            }
        }

        public async Task RefreshBalanceAsync()
        {
            if (!string.IsNullOrWhiteSpace(Account))
            {
                EtherBalance = await AccountsService.GetAccountEtherBalanceAsync(Account);
            }
        }

        public SourceCache<AccountInfo, string> Accounts => AccountsService.Accounts;

        private string _account;

        public string Account
        {
            get => _account;
            set => this.RaiseAndSetIfChanged(ref _account, value);
        }

        private string _addressTo;

        public string AddressTo
        {
            get => _addressTo;
            set => this.RaiseAndSetIfChanged(ref _addressTo, value);
        }

        private decimal _amountInEther;

        public decimal AmountInEther
        {
            get => _amountInEther;
            set => this.RaiseAndSetIfChanged(ref _amountInEther, value);
        }

        private ulong? _gas;

        public ulong? Gas
        {
            get => _gas;
            set => this.RaiseAndSetIfChanged(ref _gas, value);
        }

        private ulong? _nonce;

        public ulong? Nonce
        {
            get => _nonce;
            set => this.RaiseAndSetIfChanged(ref _nonce, value);
        }

        private string _data;

        public string Data
        {
            get => _data;
            set => this.RaiseAndSetIfChanged(ref _data, value);
        }

        private string _gasPrice;

        public string GasPrice
        {
            get => _gasPrice;
            set => this.RaiseAndSetIfChanged(ref _gasPrice, value);
        }

        private decimal _etherBalance;

        public decimal EtherBalance
        {
            get => _etherBalance;
            set => this.RaiseAndSetIfChanged(ref _etherBalance, value);
        }
    }
}

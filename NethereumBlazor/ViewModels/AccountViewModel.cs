using DynamicData;
using NethereumBlazor.Model;
using NethereumBlazor.Services;
using ReactiveUI;
using Splat;
using System.Reactive;

namespace NethereumBlazor.ViewModels
{
    public class AccountsViewModel : ReactiveObject
    {
        private readonly IAccountsService _accountsService;

        public SourceCache<AccountInfo, string> Accounts => _accountsService.Accounts;

        public NewAccountPrivateKeyViewModel NewAccount { get; } = new NewAccountPrivateKeyViewModel();

        public ReactiveCommand<Unit, Unit> AddNewAccount { get; }

        public AccountsViewModel(IAccountsService accountsService = null)
        {
            _accountsService = accountsService ?? Locator.Current.GetService<IAccountsService>();

            InitNewAccount();

            AddNewAccount = ReactiveCommand.Create(() =>
            {
                if (NewAccount.ValidPrivateKey)
                {
                    var newAccountInfo = new AccountInfo()
                    {
                        Address = NewAccount.Address
                    };

                    _accountsService.AddAccount(newAccountInfo, NewAccount.PrivateKey);
                    NewAccount.Clear();
                }
            });
        }

        private void InitNewAccount()
        {
            NewAccount.Clear();
        }
    }
}

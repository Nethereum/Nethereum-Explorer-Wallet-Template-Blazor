using DynamicData;
using NethereumBlazor.Model;
using NethereumBlazor.Services;
using ReactiveUI;
using Splat;
using System.Reactive;

namespace NethereumBlazor.ViewModels
{
    public class AccountsViewModel : ReactiveObject, IEnableLogger
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
                this.Log().Info("Adding new account");

                if (NewAccount.ValidPrivateKey)
                {
                    var newAccountInfo = new AccountInfo()
                    {
                        Address = NewAccount.Address
                    };

                    _accountsService.AddAccount(newAccountInfo, NewAccount.PrivateKey);
                    NewAccount.Clear();
                }

                this.Log().Info("Failed add new account");
            });
        }

        private void InitNewAccount()
        {
            NewAccount.Clear();
        }
    }
}

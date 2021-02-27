using DynamicData;
using NethereumExplorer.Model;
using NethereumExplorer.Services;
using ReactiveUI;

namespace NethereumExplorer.ViewModels
{
    public class AccountsViewModel : ReactiveObject
    {
        private readonly IAccountsService _accountsService;
        private NewAccountPrivateKeyViewModel _newAccountPrivateKeyViewModel;

        public SourceCache<AccountInfo, string> Accounts => _accountsService.Accounts;

        public NewAccountPrivateKeyViewModel NewAccount
        {
            get => _newAccountPrivateKeyViewModel;
        }

        public AccountsViewModel(IAccountsService accountsService, NewAccountPrivateKeyViewModel newAccountPrivateKeyViewModel)
        {
            _accountsService = accountsService;
            _newAccountPrivateKeyViewModel = newAccountPrivateKeyViewModel;
            InitNewAccount();
        }

        private void InitNewAccount()
        {
            NewAccount.Clear();
        }

        public void AddNewAccount()
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
        }
    }
}

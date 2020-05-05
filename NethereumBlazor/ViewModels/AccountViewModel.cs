using DynamicData;
using Nethereum.Web3.Accounts;
using NethereumBlazor.Model;
using NethereumBlazor.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using Splat;
using System.Reactive;

namespace NethereumBlazor.ViewModels
{
    public class AccountsViewModel : ReactiveObject, IEnableLogger, IValidatableViewModel
    {
        private readonly IAccountsService _accountsService;

        [Reactive]
        public string PrivateKey { get; set; }

        [Reactive]
        public string Address { get; set; }

        public ValidationContext ValidationContext { get; } = new ValidationContext();

        public SourceCache<AccountInfo, string> Accounts => _accountsService.Accounts;

        public ReactiveCommand<Unit, Unit> AddNewAccountCommand { get; }

        public ReactiveCommand<string, Unit> LoadAccountCommand { get; }

        public AccountsViewModel(IAccountsService accountsService = null)
        {
            _accountsService = accountsService ?? Locator.Current.GetService<IAccountsService>();

            Clear();

            this.ValidationRule(
                 viewModel => viewModel.PrivateKey,
                 privateKey => !(string.IsNullOrWhiteSpace(privateKey) || privateKey.Length != 64),
                 "You must specify a valid private key");

            LoadAccountCommand = ReactiveCommand.Create<string, Unit>(LoadAccount, this.IsValid());
            AddNewAccountCommand = ReactiveCommand.Create(AddAccount, this.IsValid());

            this.WhenAnyValue(viewModel => viewModel.PrivateKey).InvokeCommand(LoadAccountCommand);
        }

        private void AddAccount()
        {
            this.Log().Info("Adding new account");

            var newAccountInfo = new AccountInfo()
            {
                Address = Address
            };

            _accountsService.AddAccount(newAccountInfo, PrivateKey);

            Clear();

            this.Log().Info("Added new account");
        }

        private Unit LoadAccount(string privateKey)
        {
            try
            {
                this.Log().Info("Loading account");

                var account = new Account(PrivateKey);
                Address = account.Address;
            }
            catch
            {
                this.Log().Info("Failed loading account");
                Address = string.Empty;
            }

            return Unit.Default;
        }

        private void Clear()
        {
            PrivateKey = string.Empty;
            Address = string.Empty;
        }
    }
}

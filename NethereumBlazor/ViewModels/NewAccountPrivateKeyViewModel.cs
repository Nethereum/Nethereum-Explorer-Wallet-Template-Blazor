using System;
using Nethereum.Web3.Accounts;
using ReactiveUI;

namespace NethereumBlazor.ViewModels
{
    public class NewAccountPrivateKeyViewModel : ReactiveObject
    {
        private string _privateKey;
        private string _address;
        private bool _validPrivateKey;
        
        public string PrivateKey
        {
            get => _privateKey;
            set => this.RaiseAndSetIfChanged(ref _privateKey, value);
        }

        public string Address
        {
            get => _address;
            set => this.RaiseAndSetIfChanged(ref _address, value);
        }

        public bool ValidPrivateKey
        {
            get => _validPrivateKey;
            set => this.RaiseAndSetIfChanged(ref _validPrivateKey, value);
        }

        public void Clear()
        {
            PrivateKey = string.Empty;
            Address = string.Empty;
            ValidPrivateKey = false;

        }

        public void LoadAccount()
        {
            if (string.IsNullOrEmpty(PrivateKey) || PrivateKey.Length < 64)
            {
                Address = string.Empty;
                ValidPrivateKey = false;
            }
            else
            {
                try
                {
                    var account = new Account(PrivateKey);
                    Address = account.Address;
                    ValidPrivateKey = true;
                }
                catch
                {
                    ValidPrivateKey = false;
                    Address = string.Empty;
                }
            }
        }

        public NewAccountPrivateKeyViewModel()
        {
            Clear();
            this.WhenAnyValue(x => x.PrivateKey).Subscribe(x => LoadAccount());
        }
    }
}
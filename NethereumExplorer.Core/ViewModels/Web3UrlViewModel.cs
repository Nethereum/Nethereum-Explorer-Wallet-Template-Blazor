using System;
using NethereumExplorer.Messages;
using NethereumExplorer.Services;
using ReactiveUI;

namespace NethereumExplorer.ViewModels
{
    public class Web3UrlViewModel : ReactiveObject
    {
        private IWeb3ProviderService _web3ProviderService;
        private string _url;

        public Web3UrlViewModel(IWeb3ProviderService web3ProviderService)
        {
            _web3ProviderService = web3ProviderService;
            _url = _web3ProviderService.CurrentUrl;

            this.WhenAnyValue(x => x.Url, Utils.IsValidUrl).Subscribe(_ =>
                MessageBus.Current.SendMessage(
                    new UrlChanged(_url)));
        }

        public string Url
        {
            get => _url;
            set
            {
                _web3ProviderService.CurrentUrl = value;
                this.RaiseAndSetIfChanged(ref _url, value);
            }
        }
    }
}

using System;
using Blazor.FlexGrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.MobileBlazorBindings;
using NethereumExplorer.Services;
using NethereumExplorer.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NethereumExplorer
{
    public class App : Application
    {
        public App(string[] args = null, IFileProvider fileProvider = null)
        {
            var hostBuilder = MobileBlazorBindingsHost.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    // Adds web-specific services such as NavigationManager
                    services.AddBlazorHybrid();

                    // Register app-specific services
                    //services.AddSingleton<CounterState>();

		    var web3ServiceProvider = new Web3ProviderService();
                    var accountsService = new AccountsService(web3ServiceProvider);
                    var newBlockProcessingService = new NewBlockProcessingService(web3ServiceProvider);
                    var toastsViewModel = new ToastsViewModel();
                    var blocksViewModel = new BlocksViewModel(newBlockProcessingService);
                    var latestBlockTransactionsViewModel = new LatestBlockTransactionsViewModel(web3ServiceProvider);
                    var newAccountPrivateKeyViewModel = new NewAccountPrivateKeyViewModel();
                    var accountsViewModel = new AccountsViewModel(accountsService, newAccountPrivateKeyViewModel);
                    var accountsTransactionMonitoringService = new AccountsTransactionMonitoringService(accountsService, web3ServiceProvider);

                    services.AddSingleton<IWeb3ProviderService, Web3ProviderService>((x) => web3ServiceProvider);
                    services.AddSingleton<IAccountsService, AccountsService>((x) => accountsService);
                    services.AddSingleton<NewBlockProcessingService>(newBlockProcessingService);
                    services.AddSingleton<ToastsViewModel>(toastsViewModel);
                    services.AddSingleton<BlocksViewModel>(blocksViewModel);
                    services.AddSingleton<LatestBlockTransactionsViewModel>(latestBlockTransactionsViewModel);
                    services.AddTransient<BlockTransactionsViewModel>();
                    services.AddSingleton<AccountsViewModel>(accountsViewModel);
                    services.AddSingleton<NewAccountPrivateKeyViewModel>(newAccountPrivateKeyViewModel);
                    services.AddSingleton<SendTransactionViewModel>();
                    services.AddSingleton<SendErc20TransactionViewModel>();
                    services.AddSingleton(accountsTransactionMonitoringService);
                    services.AddSingleton<TransactionWithReceiptViewModel>();
                    services.AddSingleton<Web3UrlViewModel>();

                    services.AddFlexGrid(cfg =>
                    {
                        cfg.ApplyConfiguration(new TransactionsViewModelGridConfiguration());
                    });
                })
                .UseWebRoot("wwwroot");

            if (fileProvider != null)
            {
                hostBuilder.UseStaticFiles(fileProvider);
            }
            else
            {
                hostBuilder.UseStaticFiles();
            }
            var host = hostBuilder.Build();

            MainPage = new ContentPage();
            NavigationPage.SetHasNavigationBar(MainPage, false);
            host.AddComponent<Main>(parent: MainPage);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

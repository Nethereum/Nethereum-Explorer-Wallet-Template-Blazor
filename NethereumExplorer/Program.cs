using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blazor.FlexGrid;
using NethereumExplorer.Services;
using NethereumExplorer.ViewModels;

namespace NethereumExplorer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
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

            services.AddFlexGrid(cfg =>
            {
                cfg.ApplyConfiguration(new TransactionsViewModelGridConfiguration());
            });

            services.AddSingleton<Web3UrlViewModel>();
        }
    }
}

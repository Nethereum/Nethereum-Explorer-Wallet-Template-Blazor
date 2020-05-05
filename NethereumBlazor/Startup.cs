using Blazor.FlexGrid;
using Microsoft.Extensions.DependencyInjection;
using NethereumBlazor.Services;
using NethereumBlazor.ViewModels;
using Splat;
using System;

namespace NethereumBlazor
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            var web3ServiceProvider = new Web3ProviderService();
            var accountsService = new AccountsService(web3ServiceProvider);
            var newBlockProcessingService = new NewBlockProcessingService(web3ServiceProvider);

            var accountsTransactionMonitoringService = new AccountsTransactionMonitoringService(accountsService, web3ServiceProvider);

            services.AddSingleton<IWeb3ProviderService, Web3ProviderService>((x) => web3ServiceProvider);
            services.AddSingleton<IAccountsService, AccountsService>((x) => accountsService);
            services.AddSingleton<NewBlockProcessingService>(newBlockProcessingService);
            services.AddSingleton(accountsTransactionMonitoringService);

            var toastsViewModel = new ToastsViewModel();
            var blocksViewModel = new BlocksViewModel(newBlockProcessingService);
            var latestBlockTransactionsViewModel = new LatestBlockTransactionsViewModel(web3ServiceProvider);

            services.AddSingleton<ToastsViewModel>(toastsViewModel);
            services.AddSingleton<BlocksViewModel>(blocksViewModel);
            services.AddSingleton<LatestBlockTransactionsViewModel>(latestBlockTransactionsViewModel);
            services.AddTransient<BlockTransactionsViewModel>();
            services.AddSingleton<SendTransactionViewModel>();
            services.AddSingleton<SendErc20TransactionViewModel>();
            services.AddSingleton<TransactionWithReceiptViewModel>();
            services.AddSingleton<Web3UrlViewModel>();

            services.AddFlexGrid(cfg =>
            {
                cfg.ApplyConfiguration(new TransactionsViewModelGridConfiguration());
            });
        }

        public static void ConfigureRxServices(IServiceProvider services)
        {
            var web3ServiceProvider = services.GetService<IWeb3ProviderService>();
            var accountsService = services.GetService<IAccountsService>();
            var newBlockProcessingService = services.GetService<NewBlockProcessingService>();

            Locator.CurrentMutable.Register(() => web3ServiceProvider, typeof(IWeb3ProviderService));
            Locator.CurrentMutable.Register(() => accountsService, typeof(IAccountsService));
            Locator.CurrentMutable.Register(() => newBlockProcessingService, typeof(NewBlockProcessingService));
            Locator.CurrentMutable.RegisterConstant(new ConsoleLogger(), typeof(ILogger));
        }
    }
}

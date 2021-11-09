using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using NethereumExplorer.Services;
using NethereumExplorer.ViewModels;
using Blazor.FlexGrid;

namespace NethereumExplorer.Maui
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
            
            var builder = MauiApp.CreateBuilder();
			builder
				.RegisterBlazorMauiWebView()
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			builder.Services.AddBlazorWebView();
 
            var services = builder.Services;
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

            return builder.Build();
        }
	}
}

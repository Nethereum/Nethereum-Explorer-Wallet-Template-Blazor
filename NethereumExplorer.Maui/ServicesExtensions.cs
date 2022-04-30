namespace NethereumExplorer.Maui;

internal static class ServicesExtensions
{
    public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
    {
        var services = builder.Services;

        services.AddMauiBlazorWebView();

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

        return builder;
    }
}

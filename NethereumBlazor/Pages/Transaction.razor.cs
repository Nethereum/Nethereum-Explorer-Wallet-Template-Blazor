using NethereumBlazor.ViewModels;
using ReactiveUI;
using System;

namespace NethereumBlazor.Pages
{
    public partial class Transaction
    {
        public Transaction()
        {
            ViewModel = new TransactionWithReceiptViewModel();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.WhenAny(v => v.TransactionHash, c => c.Value).Subscribe(hash =>
            {
                if (!string.IsNullOrWhiteSpace(hash))
                {
                    ViewModel.LoadTransactionCommand.Execute(hash);
                }
            });
        }
    }
}

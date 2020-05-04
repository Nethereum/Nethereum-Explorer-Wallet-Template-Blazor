using Blazor.FlexGrid.DataAdapters;
using NethereumBlazor.ViewModels;
using System.Numerics;
using System;
using System.Linq;
using ReactiveUI;

namespace NethereumBlazor.Pages
{
    public partial class Block
    {
        private CollectionTableDataAdapter<TransactionViewModel> dataAdapter;

        public int TransactionCount { get; set; }

        public Block()
        {
            ViewModel = new BlockTransactionsViewModel();
        }

        protected override void OnInitialized()
        {
            this.WhenAny(v => v.BlockNumber, c => c.Value).Subscribe(hash =>
            {
                if (BigInteger.TryParse(hash, out var result))
                {
                    ViewModel.BlockNumber = result;

                    ViewModel.Transactions.Connect()
                        .Subscribe(_ =>
                        {
                            TransactionCount = ViewModel.Transactions.Count;
                            dataAdapter = new CollectionTableDataAdapter<TransactionViewModel>(ViewModel.Transactions.Items.ToArray());

                            StateHasChanged();
                        });
                }
            });
        }
    }
}

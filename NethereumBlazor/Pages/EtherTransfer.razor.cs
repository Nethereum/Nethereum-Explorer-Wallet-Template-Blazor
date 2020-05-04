using NethereumBlazor.Components;
using NethereumBlazor.ViewModels;
using ReactiveUI;
using System;

namespace NethereumBlazor.Pages
{
    public partial class EtherTransfer
    {
        public RxButton RefreshBalanceButton { get; set; }

        public RxButton TransferEtherButton { get; set; }

        public EtherTransfer()
        {
            ViewModel = new SendTransactionViewModel();
        }

        protected override void OnAfterRender(bool isFirstRender)
        {
            base.OnAfterRender(isFirstRender);

            if (isFirstRender)
            {
                this.BindCommand(ViewModel, vm => vm.RefreshBalanceCommand, v => v.RefreshBalanceButton);
                this.BindCommand(ViewModel, vm => vm.SendTransactionCommand, v => v.TransferEtherButton);
            }
        }
    }
}

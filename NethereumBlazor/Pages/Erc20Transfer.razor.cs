using NethereumBlazor.Components;
using NethereumBlazor.ViewModels;
using ReactiveUI;
using System;

namespace NethereumBlazor.Pages
{
    public partial class Erc20Transfer
    {
        public RxButton RefreshBalanceButton { get; set; }

        public RxButton RefreshTokenBalanceButton { get; set; }

        public RxButton TransferTokenButton { get; set; }

        public Erc20Transfer()
        {
            ViewModel = new SendErc20TransactionViewModel();
        }

        protected override void OnAfterRender(bool isFirstRender)
        {
            base.OnAfterRender(isFirstRender);

            if (isFirstRender)
            {
                this.BindCommand(ViewModel, vm => vm.RefreshBalanceCommand, v => v.RefreshBalanceButton);
                this.BindCommand(ViewModel, vm => vm.RefreshTokenBalanceCommand, v => v.RefreshTokenBalanceButton);
            }
        }
    }
}

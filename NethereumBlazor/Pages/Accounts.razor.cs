using NethereumBlazor.Components;
using NethereumBlazor.ViewModels;
using ReactiveUI;

namespace NethereumBlazor.Pages
{
    public partial class Accounts
    {
        private RxButton AddAccountBtn { get; set; }

        private RxTextInput RxTextInput { get; set; }

        public Accounts()
        {
            ViewModel = new AccountsViewModel();
        }

        protected override void OnAfterRender(bool isFirstRender)
        {
            base.OnAfterRender(isFirstRender);

            if (isFirstRender)
            {
                this.BindCommand(ViewModel, vm => vm.AddNewAccount, v => v.AddAccountBtn);
                this.Bind(ViewModel, vm => vm.NewAccount.PrivateKey, v => v.RxTextInput.Text);
            }
        }
    }
}

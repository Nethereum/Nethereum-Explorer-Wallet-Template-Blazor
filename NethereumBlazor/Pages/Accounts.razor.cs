using NethereumBlazor.Components;
//using NethereumBlazor.Extensions;
using NethereumBlazor.ViewModels;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;

namespace NethereumBlazor.Pages
{
    public partial class Accounts
    {
        private RxButton AddAccountBtn { get; set; }

        private RxTextInput PrivateKey { get; set; }

        public Accounts()
        {
            ViewModel = new AccountsViewModel();
        }

        protected override void OnAfterRender(bool isFirstRender)
        {
            base.OnAfterRender(isFirstRender);

            if (isFirstRender)
            {
                this.BindCommand(ViewModel, vm => vm.AddNewAccountCommand, v => v.AddAccountBtn);
                this.Bind(ViewModel, vm => vm.PrivateKey, v => v.PrivateKey.Text);
                this.BindValidation(ViewModel, v => v.PrivateKey.Error);
            }
        }
    }
}

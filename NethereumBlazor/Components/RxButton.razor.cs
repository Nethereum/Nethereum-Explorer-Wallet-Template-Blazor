using Microsoft.AspNetCore.Components;
using System.Windows.Input;

namespace NethereumBlazor.Components
{
    public partial class RxButton
    {
        public ICommand Command { get; set; }

        public object CommandParameter { get; set; }

        private void OnClickHandler()
        {
            if (Command.CanExecute(null))
            {
                Command.Execute(null);
            }
        }
    }
}

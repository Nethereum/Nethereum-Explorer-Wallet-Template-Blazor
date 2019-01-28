using System;
using DynamicData;
using NethereumBlazor.Messages;
using ReactiveUI;

namespace NethereumBlazor.ViewModels
{
    public class ToastsViewModel : ReactiveObject
    {
        public SourceCache<ToastViewModel, string> Toasts = new SourceCache<ToastViewModel, string>(x => x.Key);

        public ToastsViewModel()
        {
            MessageBus.Current.Listen<AccountTransactionCompleted>().Subscribe(x =>
                {
                    var state = x.Failed.HasValue && x.Failed.Value ? "error" : "confirmed";

                    Toasts.AddOrUpdate(
                        new ToastViewModel()
                        {
                            Key = x.TransactionHash,
                            Title =  "Transaction " + state,
                            Message=  state + " for account: " + x.AccountAddress,
                            RelativeLink = "transaction/" + x.TransactionHash,
                            LinkMessage  = Utils.TruncateEllipse(x.TransactionHash, 20),
                            Received = DateTime.Now,
                            Failed = x.Failed.HasValue && x.Failed.Value ? true : false
                        });
                }
            );
        }

        public void RemoveToast(string key)
        {
            Toasts.Remove(key);
        }
    }
}

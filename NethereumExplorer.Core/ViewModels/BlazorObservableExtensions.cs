using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace NethereumExplorer.ViewModels
{
    public static class BlazorObservableExtensions
    {
        public static IDisposable SubscribeAndNotifyStateChanges<T>(this IObservable<T> source, Func<Action, Task> invokeAsync, Action statehasChanged)
        {
            return source.Select(x => InvokeAsyncStateHasChanged<T>(x, invokeAsync, statehasChanged).ToObservable()).Subscribe();
        }

        public static async Task<T> InvokeAsyncStateHasChanged<T>(T value, Func<Action, Task> invokeAsync, Action statehasChanged)
        {
            await invokeAsync(statehasChanged);
            return value;
        }
    }
}
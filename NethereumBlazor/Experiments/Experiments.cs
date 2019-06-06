using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;
using Nethereum.Web3.Accounts;
using Org.BouncyCastle.Cms;
using ReactiveUI;

namespace BlazorReactiveUI.Model
{
    /*
    public class HdWalletAccountInfo:AccountInfo
    {
        public int Index { get; set; }
    }
    */
    //public class StateHasChangedComponent: BlazorComponent, IStateHasChangedComponent{

    //    public void RaiseStateHasChanged()
    //    {
    //        StateHasChanged();
    //    }
    //}

    //public interface IStateHasChangedComponent
    //{
    //    void RaiseStateHasChanged();
    //}

    //public class ButtonReactiveCommand<TParam, TResult>:ReactiveObject
    //{ 
    //    public ButtonReactiveCommand(ReactiveCommand<TParam, TResult> reactiveCommand, IStateHasChangedComponent component)
    //    {
    //        ReactiveCommand = reactiveCommand;
    //        Component = component;
    //        reactiveCommand.CanExecute.Subscribe(x =>
    //        {
    //            CanExecute = x;
    //            Component.RaiseStateHasChanged();
    //        });


    //    }
    //    public TParam Param { get; set; }
    //    public Action OnClick;
    //    public ReactiveCommand<TParam, TResult> ReactiveCommand { get;  }
    //    public IStateHasChangedComponent Component { get; }
    //    public bool CanExecute { get; set; }
    //}

    //public static class ButtonReactiveCommandExtensions
    //{
    //    public static ButtonReactiveCommand<TParam, TResult> CreateButtonReactiveCommand<TParam, TResult>(
    //        this ReactiveCommand<TParam, TResult> command, IStateHasChangedComponent component)
    //    {
    //        return new ButtonReactiveCommand<TParam, TResult>(command, component);
    //    }
    //}
}

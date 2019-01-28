# Nethereum Blazor

The Nethereum Blazor application is a Wasm browser based light blockchain explorer and wallet.

The aim of this application is to provide another wallet / client reference for the final goal of providing a reusable common mvvm front end framework and libraries targetting all Dekstops (WinForms, Xamarin.Forms, Avalonia for Windows, Mac, Linux), Mobile (Android and iOS using Xamarin.Forms), Browser SPA (Blazor this example, Uno) and gaming / vr engines (Unity3d)


![Nethereum Blazor](Screenshots/NethereumBlazorDemo.gif "Nethereum Blazor")


## Blazor + ReactiveUI
ReactiveUI is not fully supported in Blazor, and the hope of this project is to provide an experimental context to enable all the ReactiveUI features in Blazor.

### What it works (Out of the box)
- MessageBus
- Binding
- Subscriptions
- Observable timers if using Subscribe with "async"

### What it does not work yet (Probably needs a Scheduler)
- ReactiveCommands
- Command validation / enable
- Observable using FromAsync / wait 
- ...

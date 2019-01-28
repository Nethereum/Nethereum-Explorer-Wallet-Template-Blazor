# Nethereum Blazor

The Nethereum Blazor application is a .Net Wasm SPA light blockchain explorer and simple wallet.

The aim of this application is to provide another wallet / client reference for the final goal of providing a reusable common mvvm front end framework and libraries targetting all Dekstops (WinForms, Xamarin.Forms, Avalonia for Windows, Mac, Linux), Mobile (Android and iOS using Xamarin.Forms), Browser SPA (Blazor this example, Uno) and gaming / vr engines (Unity3d)


![Nethereum Blazor](Screenshots/NethereumBlazorDemo.gif "Nethereum Blazor")


# More info:
* Blazor:  The .Net Html / razor wasm framework https://blazor.net/
* ReactiveUI: https://reactiveui.net/
* Blazor.FlexGrid: The grid component used in Block Page https://github.com/Mewriick/Blazor.FlexGrid
* Infura: Infura hosts the public Ethereum nodes preconfigured
* Testchains: If you need a test chain to run in your localhost 

## Blazor + ReactiveUI
One of the main goals is to eventually have full support of ReactiveUI as the common framework for all the Nethereum FrontEnd exxample and future solutions.
Avalonia Desktop (Windows, Linux, Mac): https://github.com/Nethereum/Nethereum.UI.Desktop, WindowsForms https://github.com/Nethereum/Nethereum.SimpleWindowsWallet
Xamarin.Forms Mobile and Desktop wallets:  https://github.com/Nethereum/Nethereum.UI.Wallet.Sample

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

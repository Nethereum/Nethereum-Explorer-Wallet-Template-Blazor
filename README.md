# Nethereum.BlazorReactiveUI

The aim of Nethereum Front Ends samples and final implementation is to reuse as much as possible all front end business logic and if possbile UI components accross the different platforms Mobile / Dekstop / Web (Xamarin.Forms, Uno, UWP, Avalonia WPF, Windows Forms... and Web == Blazor).

The intention of this PoC is to provide an MVVM / ReactiveUI implementation for Blazor, enable us to reuse the Wallet ViewModels already used in the Avalonia, WinForms samples. 

Once the PoC is done, all infrastructure components should be ported, pull to the proper projects (i.e ReactiveUI, etc)

Somethings to consider:

1. Blazor / WASM UI Scheduler 
2. MVVM (Command / Property Binding) Sample of MVVM (Not Reactive UI https://blog.jeremylikness.com/mvvm-support-in-blazor-dbc38060a4a0)
3. Messaging Sample of Blazor.RxMessaging https://github.com/bmsantos/RxBlazor, so this might work out of the box
4. Timers / Intervals using Js Interop (using the Scheduler?)
5. RXComponent including ViewModel, IViewFor, Activation etc.
...



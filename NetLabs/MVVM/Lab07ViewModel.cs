using FloxelLib.MVVM;
using NetLabs.Labs.Lab07;
using System.Diagnostics;
using System.Windows;

namespace NetLabs.MVVM;

public sealed partial class Lab07ViewModel : BaseViewModel
{
    public bool IsRunning => WebApp.IsRunning;

    [UpdateProperty]
    private string _buttonLabel = WebApp.IsRunning ? "Стоп" : "Старт";

    private static readonly Style offStyle = Application.Current.FindResource("ErrorOutlinedButton") as Style;
    private static readonly Style onStyle = Application.Current.FindResource("SuccessOutlinedButton") as Style;

    [UpdateProperty]
    private Style _buttonStyle = WebApp.IsRunning ? offStyle : onStyle;

    [RelayCommand]
    private void ToggleServer()
    {
        if (IsRunning)
        {
            WebApp.Stop();
            ButtonLabel = "Старт";
            ButtonStyle = onStyle;
        }
        else
        {
            WebApp.Start();
            ButtonLabel = "Стоп";
            ButtonStyle = offStyle;
        }

        OnPropertyChanged(nameof(IsRunning));
    }

    [RelayCommand]
    private void OpenBrowser()
    {
        if (IsRunning)
            Process.Start(new ProcessStartInfo { FileName = "https://asp.prevter.tk/", UseShellExecute = true });
    }
}

using System;
using FloxelLib.MVVM;
using NetLabs.Labs.Lab08;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using MessageBox = FloxelLib.Common.MessageBox;

namespace NetLabs.MVVM;

public sealed partial class Lab08ViewModel : BaseViewModel
{
    public ObservableCollection<Sportsman> Sportsmen { get; init; } = new();

    [UpdateProperty]
    private string _username, _password;
    
    [UpdateProperty]
    private bool _loading = false;

    private static bool IsLoggedIn => DatabaseController.IsConnected;
    
    [UpdateProperty]
    private Visibility _loginVisibility = DatabaseController.IsConnected ? Visibility.Collapsed : Visibility.Visible,
        _menuVisibility = DatabaseController.IsConnected ? Visibility.Visible : Visibility.Collapsed,
        _modalVisibility = Visibility.Collapsed;

    [UpdateProperty]
    private string _modalTitle = "Додати спортсмена";

    private int editedId = -1;
    private bool modalCreate = true;

    [UpdateProperty]
    private string _editedFirstName, _editedLastName, _editedMiddleName, _editedSportName, _editedAchievements, _editedPrizes;

    [UpdateProperty]
    private int _editedYear;

    [UpdateProperty]
    private bool _editedIsMale;

    [RelayCommand]
    private void Login()
    {
        try
        {
            DatabaseController.Connect(_username, _password);
            LoginVisibility = Visibility.Collapsed;
            MenuVisibility = Visibility.Visible;
            UpdateSportsmenAsync();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Помилка", MessageBoxImage.Error);
        }
    }
    
    private void UpdateSportsmenAsync()
    {
        Loading = true;
        Task.Factory.StartNew(() =>
        {
            Application.Current.Dispatcher.Invoke(() => Sportsmen.Clear());
            foreach (var sportsman in DatabaseController.GetSportsmen())
                Application.Current.Dispatcher.Invoke(() => Sportsmen.Add(sportsman));
            Loading = false;
        });
    }
    
    [RelayCommand]
    private void Logout()
    {
        DatabaseController.Disconnect();
        LoginVisibility = Visibility.Visible;
        MenuVisibility = Visibility.Collapsed;
        Sportsmen.Clear();
    }

    [RelayCommand]
    private void Refresh()
    {
        UpdateSportsmenAsync();
    }

    [RelayCommand]
    private void Create()
    {
        ModalTitle = "Додати спортсмена";
        ModalVisibility = Visibility.Visible;
        modalCreate = true;
        editedId = -1;

        EditedFirstName = "";
        EditedLastName = "";
        EditedMiddleName = "";
        EditedYear = 0;
        EditedIsMale = true;
        EditedSportName = "";
        EditedAchievements = "";
        EditedPrizes = "";
    }

    [RelayCommand]
    private void Edit(object? arg)
    {
        if (arg is not Sportsman sportsman)
            return;

        ModalTitle = "Редагувати спортсмена";
        ModalVisibility = Visibility.Visible;
        modalCreate = false;
        editedId = sportsman.Id;

        EditedFirstName = sportsman.FirstName;
        EditedLastName = sportsman.LastName;
        EditedMiddleName = sportsman.MiddleName;
        EditedYear = sportsman.Year;
        EditedIsMale = sportsman.IsMale;
        EditedSportName = sportsman.SportName;
        EditedAchievements = sportsman.Achievements;
        EditedPrizes = sportsman.Prizes;
    }

    [RelayCommand]
    private void Delete(object? arg)
    {
        if (arg is not Sportsman sportsman)
            return;

        if (MessageBox.Show($"Видалити спортсмена {sportsman.FullName}?", "Видалення", MessageBoxButton.YesNo, MessageBoxImage.Question) == FloxelLib.Common.MessageBoxResult.Yes)
        {
            try
            {
                DatabaseController.DeleteSportsman(sportsman);
                UpdateSportsmenAsync();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Помилка", MessageBoxImage.Error);
            }
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        ModalVisibility = Visibility.Collapsed;
    }

    [RelayCommand]
    private void CommitEdit()
    {
        try
        {
            var sportsman = new Sportsman(
                editedId, _editedFirstName, 
                _editedLastName, _editedMiddleName, 
                _editedYear, _editedIsMale, 
                _editedSportName, _editedAchievements, 
                _editedPrizes
            );

            if (modalCreate)
                DatabaseController.AddSportsman(sportsman);
            else
                DatabaseController.UpdateSportsman(sportsman);

            UpdateSportsmenAsync();
            ModalVisibility = Visibility.Collapsed;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Помилка", MessageBoxImage.Error);
        }
    }

    public Lab08ViewModel()
    {
        if (IsLoggedIn)
            UpdateSportsmenAsync();
    }
}

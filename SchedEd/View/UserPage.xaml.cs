namespace SchedEd.View;

using SchedEd.ViewModel;

public partial class UserPage : ContentPage
{
    public UserPage()
    {
        InitializeComponent();
        BindingContext = new UserViewModel();
    }

    private async void OnClickedHome(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}
namespace SchedEd.View;

using SchedEd.ViewModel;
public partial class Home : ContentPage
{
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Call GetClassesCommand to refresh the data
        var viewModel = BindingContext as HomeViewModel;
    }
    public Home()
    {
        InitializeComponent();
        BindingContext = new HomeViewModel();
    }

    private async void NavClassPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ClassPage");
    }

    private async void NavStudentsPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//StudentsPage");
    }

    private async void NavRecordsPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//RecordPage");
    }

    private void OnClassFilterChanged(object sender, EventArgs e)
    {
        if (BindingContext is StudentsPageViewModel viewModel)
        {
            viewModel?.ApplyFiltersCommand.Execute(null); // Call the ViewModel's filter logic
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is StudentsPageViewModel viewModel)
        {
            viewModel.SearchText = e.NewTextValue; // Update the search text in the ViewModel
            viewModel?.ApplyFiltersCommand.Execute(null);
        }
    }
}
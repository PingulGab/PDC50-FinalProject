using SchedEd.ViewModel;

namespace SchedEd.View.StudentsPageCollection;

public partial class StudentsPage : ContentPage
{
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Call GetClassesCommand to refresh the data
        var viewModel = BindingContext as StudentsPageViewModel;
        viewModel?.GetStudentsCommand.Execute(null);
    }
    public StudentsPage()
    {
        InitializeComponent();
        BindingContext = new StudentsPageViewModel();
    }

    private async void NavAddStudentPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//AddStudentPage");
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
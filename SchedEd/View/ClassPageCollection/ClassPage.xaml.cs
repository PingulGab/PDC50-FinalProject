namespace SchedEd.View.ClassPageCollection;

using SchedEd.ViewModel;
using System.Diagnostics;

public partial class ClassPage : ContentPage
{
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Call GetClassesCommand to refresh the data
        var viewModel = BindingContext as ClassPageViewModel;
        viewModel?.GetClassesCommand.Execute(null);
    }

    public ClassPage()
    {
        InitializeComponent();
        BindingContext = new ClassPageViewModel();
    }
    private async void NavAddClassPage(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//AddClassPage");
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is ClassPageViewModel viewModel)
        {
            viewModel.SearchText = e.NewTextValue; // Update the search text in the ViewModel
            viewModel?.ApplyFiltersCommand.Execute(null);
        }
    }

    //private async void NavEditClassPage(object sender, EventArgs e)
    //{
    //    var button = sender as Button;

    //    if (button.BindingContext is ClassPageViewModel classViewModel)
    //    {
    //        var classId = classViewModel.NewClassID; // Access the ClassId property
    //        await Shell.Current.GoToAsync($"//EditClassPage?classId={classId}");
    //        Debug.WriteLine(classId);
    //    }
    //    else
    //    {
    //        await Application.Current.MainPage.DisplayAlert("Error", "Invalid binding context for the button.", "OK");
    //    }
    //}
}
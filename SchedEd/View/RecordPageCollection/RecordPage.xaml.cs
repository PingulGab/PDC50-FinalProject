using SchedEd.ViewModel;

namespace SchedEd.View.RecordPageCollection;

public partial class RecordPage : ContentPage
{
    public RecordPage()
    {
        InitializeComponent();
        BindingContext = new RecordPageViewModel();
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is RecordPageViewModel viewModel)
        {
            viewModel.SearchText = e.NewTextValue; // Update the search text in the ViewModel
            viewModel?.ApplyFiltersCommand.Execute(null);
        }
    }
}
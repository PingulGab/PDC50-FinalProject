namespace SchedEd.View.ClassPageCollection;
using Microsoft.Maui.Controls;
using SchedEd.ViewModel;

public partial class AddClassPage : ContentPage
{
    public AddClassPage()
    {
        InitializeComponent();
        BindingContext = new AddClassPageViewModel();
    }
}
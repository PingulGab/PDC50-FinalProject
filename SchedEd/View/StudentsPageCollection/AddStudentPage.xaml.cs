using SchedEd.ViewModel;

namespace SchedEd.View.StudentsPageCollection;

public partial class AddStudentPage : ContentPage
{
    public AddStudentPage()
    {
        InitializeComponent();
        BindingContext = new AddStudentPageViewModel();
    }
}
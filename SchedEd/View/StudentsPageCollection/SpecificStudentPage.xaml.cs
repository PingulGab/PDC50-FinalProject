using SchedEd.ViewModel;
using System.Diagnostics;

namespace SchedEd.View.StudentsPageCollection;

[QueryProperty(nameof(studID), "studID")]
public partial class SpecificStudentPage : ContentPage
{
    public int studID { get; set; }
    private SpecificStudentPageViewModel _viewModel;
    public SpecificStudentPage()
    {
        InitializeComponent();
        BindingContext = _viewModel = new SpecificStudentPageViewModel();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args); // Call the base implementation

        Debug.WriteLine($"studID: {studID}"); // See if ClassId is correctly set

        if (studID > 0)
        {
            await _viewModel.LoadStudent(studID); // Load data if ClassId is valid
        }
        else
        {
            Debug.WriteLine("ClassId was not properly set.");
        }
    }
}
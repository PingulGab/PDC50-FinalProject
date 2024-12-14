using SchedEd.ViewModel;
using System.Diagnostics;

namespace SchedEd.View.ClassPageCollection;

[QueryProperty(nameof(classId), "classId")]
public partial class SpecificClassPage : ContentPage
{
    public int classId { get; set; }
    private SpecificClassViewModel _viewModel;
    public SpecificClassPage()
    {
        InitializeComponent();
        BindingContext = _viewModel = new SpecificClassViewModel();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args); // Call the base implementation

        Debug.WriteLine($"ClassId: {ClassId}"); // See if ClassId is correctly set

        if (classId > 0)
        {
            await _viewModel.LoadClassData(classId); // Load data if ClassId is valid
        }
        else
        {
            Debug.WriteLine("ClassId was not properly set.");
        }
    }
}
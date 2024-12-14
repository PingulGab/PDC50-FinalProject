using SchedEd.ViewModel;
using System.Diagnostics;

namespace SchedEd.View.ClassPageCollection;

// Bind the query property "classId" to the C# property "ClassId"
[QueryProperty(nameof(classId), "classId")]
public partial class EditClassPage : ContentPage
{
    public int classId { get; set; } // Property bound to Shell query parameter "classId"

    private EditClassPageViewModel _viewModel;

    public EditClassPage()
    {
        InitializeComponent();
        BindingContext = _viewModel = new EditClassPageViewModel();
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

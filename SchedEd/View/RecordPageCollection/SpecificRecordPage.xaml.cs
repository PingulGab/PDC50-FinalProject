using SchedEd.ViewModel;
using System.Diagnostics;

namespace SchedEd.View.RecordPageCollection;

[QueryProperty(nameof(classId), "classId")]
public partial class SpecificRecordPage : ContentPage
{
    public int classId { get; set; }

    private SpecificRecordPageViewModel _viewModel;
    public SpecificRecordPage()
    {
        InitializeComponent();
        BindingContext = _viewModel = new SpecificRecordPageViewModel();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        Debug.WriteLine($"ClassId: {ClassId}");

        if (classId > 0)
        {
            await _viewModel.LoadClassData(classId);
        }
        else
        {
            Debug.WriteLine("ClassId was not properly set.");
        }
    }
}
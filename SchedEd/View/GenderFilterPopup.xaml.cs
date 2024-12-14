using CommunityToolkit.Maui.Views;
using SchedEd.ViewModel;

namespace SchedEd.View
{
    public partial class GenderFilterPopup : Popup
    {
        private readonly StudentsPageViewModel _viewModel;

        public GenderFilterPopup(StudentsPageViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            Close(); // Close the popup
        }

        private void OnApplyClicked(object sender, EventArgs e)
        {
            var selectedGender = GenderPicker.SelectedItem?.ToString();
            _viewModel.ApplyGenderFilter(selectedGender);
            Close(); // Close the popup
        }
    }
}

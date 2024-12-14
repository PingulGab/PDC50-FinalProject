using CommunityToolkit.Maui.Views;
using SchedEd.ViewModel;

namespace SchedEd.View
{
    public partial class GenderFilterPopupHome : Popup
    {
        private readonly HomeViewModel _viewModel;

        public GenderFilterPopupHome(HomeViewModel viewModel)
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

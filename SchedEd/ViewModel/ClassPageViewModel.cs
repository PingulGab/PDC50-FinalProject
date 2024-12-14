using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SchedEd.Model;
using SchedEd.Services;
using SchedEd.View.ClassPageCollection;
using System.Diagnostics;

namespace SchedEd.ViewModel
{
    public class ClassPageViewModel : BindableObject
    {
        private readonly ClassService _classService;

        //Search
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        //Is Class Selected
        private bool _isClassSelected;
        public bool IsClassSelected
        {
            get => _isClassSelected;
            set
            {
                if (_isClassSelected != value)
                {
                    _isClassSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        //Selected Class
        private Class _selectedClass;
        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                if (_selectedClass != null)
                {
                    NewClassID = _selectedClass.ID;
                    IsClassSelected = true;
                }
                else
                {
                    IsClassSelected = false;
                }
                OnPropertyChanged();
            }
        }

        //Class Details
        private int _newClassID;
        public int NewClassID
        {
            get => _newClassID;
            set
            {
                _newClassID = value;
                OnPropertyChanged();
            }
        }


        public ObservableCollection<Class> Classes { get; set; }

        public ClassPageViewModel()
        {
            _classService = new ClassService();
            Classes = new ObservableCollection<Class>();

            GetClassesCommand = new Command(async () => await GetClasses());
            DeleteClassCommand = new Command(async () => await DeleteClass(), () => IsClassSelected);
            EditClassCommand = new Command<Class>(OnEditClass);
            OnDeleteClassCommand = new Command<Class>(OnDeleteClass);
            OnViewClassCommand = new Command<Class>(OnViewClass);
            ApplyFiltersCommand = new Command(ApplyFilters);


            GetClasses();
        }
        public ICommand ViewStudentCommand { get; }
        public ICommand OnDeleteClassCommand { get; }
        public ICommand ApplyFiltersCommand { get; }
        public ICommand OnViewClassCommand { get; }
        public ICommand GetClassesCommand { get; }
        public ICommand DeleteClassCommand { get; }
        public ICommand EditClassCommand { get; }

        private List<Class> _originalClasses = new List<Class>();
        private async Task GetClasses()
        {
            var classes = await _classService.GetClassesAsync();
            _originalClasses = classes;
            Classes.Clear();
            foreach (var class1 in classes)
            {
                class1.Image += $"?timestamp={DateTime.Now.Ticks}";
                Classes.Add(class1);
            }
        }

        private void ApplyFilters()
        {
            var filteredClass = _originalClasses; // Keep a backup of all students
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filteredClass = filteredClass
                    .Where(s => s.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            Classes.Clear();
            foreach (var class1 in filteredClass)
            {
                Classes.Add(class1);
            }
        }
        private async Task DeleteClass()
        {
            if (SelectedClass != null)
            {
                // Call DeleteClassAsync in ClassService
                var result = await _classService.DeleteClassAsync(SelectedClass.ID);

                if (result == "Success")
                {
                    // Remove from collection
                    Classes.Remove(SelectedClass);
                    SelectedClass = null;
                    await GetClasses();

                    await Application.Current.MainPage.DisplayAlert("Success", "Class deleted successfully.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", result, "OK");
                }
            }
        }

        private async void OnDeleteClass(Class class1)
        {
            if (class1 == null)
                return;

            bool isConfirmed = await Application.Current.MainPage.DisplayAlert(
                "Delete Student",
                $"Are you sure you want to delete {class1.Name}?",
                "Yes",
                "No"
            );

            if (!isConfirmed)
                return;

            try
            {
                // Call the service to delete the student
                var result = await _classService.DeleteClassAsync(class1.ID);

                if (result == "Success")
                {
                    // Remove the student from the UI collection
                    Classes.Remove(class1);
                    _originalClasses.Remove(class1);

                    await Application.Current.MainPage.DisplayAlert("Success", "Student deleted successfully.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", result, "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting student: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred while deleting the student.", "OK");
            }
        }

        private async void OnEditClass(Class selectedClass)
        {
            if (selectedClass != null)
            {
                // Navigate to EditClassPage and pass the ClassId

                await Shell.Current.GoToAsync($"//EditClassPage",
                new Dictionary<string, object>
                {
                    ["classId"] = selectedClass.ID,
                });
            }
        }

        private async void OnViewClass(Class selectedClass)
        {
            if (selectedClass != null)
            {
                // Navigate to EditClassPage and pass the ClassId

                await Shell.Current.GoToAsync($"//SpecificClassPage",
                new Dictionary<string, object>
                {
                    ["classId"] = selectedClass.ID,
                });
            }
        }


    }
}

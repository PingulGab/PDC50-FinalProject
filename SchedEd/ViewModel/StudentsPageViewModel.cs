using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SchedEd.View;
using SchedEd.Model;
using SchedEd.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Maui.Views;

namespace SchedEd.ViewModel
{
    public class StudentsPageViewModel : BindableObject
    {
        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<Class> Classes { get; set; }
        public ObservableCollection<string> GenderOptions { get; set; }
        private readonly StudentService _studentService;
        private readonly ClassService _classService;

        public string SelectedGender { get; set; } = "None";
        //Search Bar
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

        //Selected Class
        private Class _selectedClass;
        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged();
                ApplyFilters(); // Apply filters when class changes
            }
        }

        public StudentsPageViewModel()
        {
            _studentService = new StudentService();
            _classService = new ClassService();
            Classes = new ObservableCollection<Class>();
            Students = new ObservableCollection<Student>();

            GetClassesCommand = new Command(async () => await GetClasses());
            GetStudentsCommand = new Command(async () => await GetStudents());

            GenderOptions = new ObservableCollection<string> { "None", "Male", "Female" };
            ShowGenderFilterPopupCommand = new Command(ShowGenderFilterPopup);
            ClosePopupCommand = new Command(ClosePopup);
            ApplyFiltersCommand = new Command(ApplyFilters);

            ViewStudentCommand = new Command<Student>(OnViewStudent);
            EditStudentCommand = new Command<Student>(OnEditStudent);
            DeleteStudentCommand = new Command<Student>(OnDeleteStudent);

            GetStudents();
            GetClasses();
        }
        public ICommand GetClassesCommand { get; }
        public ICommand GetStudentsCommand { get; }
        public ICommand ShowGenderFilterPopupCommand { get; }
        public ICommand ClosePopupCommand { get; }
        public ICommand ApplyFiltersCommand { get; }
        public ICommand ViewStudentCommand { get; }
        public ICommand EditStudentCommand { get; }
        public ICommand DeleteStudentCommand { get; }

        private async Task GetClasses()
        {
            var classes = await _classService.GetClassesAsync();
            Classes.Clear();

            Classes.Add(new Class { ID = 0, Name = "None" });
            foreach (var class1 in classes)
            {
                class1.Image += $"?timestamp={DateTime.Now.Ticks}";
                Classes.Add(class1);
            }

            SelectedClass = Classes.FirstOrDefault(c => c.Name == "None"); // Default to "None"
        }

        private List<Student> _originalStudents = new List<Student>(); // Holds all students (unfiltered)

        // Update GetStudents to store the unfiltered data
        private async Task GetStudents()
        {
            var students = await _studentService.GetStudentsASync();
            _originalStudents = students; // Save original list
            Students.Clear();
            foreach (var student in students)
            {
                var matchedClass = Classes.FirstOrDefault(c => c.ID == student.ClassID);
                if (matchedClass != null)
                {
                    student.ClassName = matchedClass.Acronym; // ✅ Assign ClassName
                }

                student.Image += $"?timestamp={DateTime.Now.Ticks}";
                Students.Add(student);
            }
        }

        private void ApplyFilters()
        {
            var filteredStudents = _originalStudents; // Keep a backup of all students
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filteredStudents = filteredStudents
                    .Where(s => s.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (SelectedClass != null && SelectedClass.Name != "None")
            {
                filteredStudents = filteredStudents
                    .Where(s => s.ClassName == SelectedClass.Acronym)
                    .ToList();
            }

            if (!string.IsNullOrEmpty(SelectedGender) && SelectedGender != "None")
            {
                filteredStudents = filteredStudents
                    .Where(s => s.Gender == SelectedGender)
                    .ToList();
            }

            Students.Clear();
            foreach (var student in filteredStudents)
            {
                Students.Add(student);
            }
        }

        private GenderFilterPopup _popup;

        private void ShowGenderFilterPopup()
        {
            var currentPage = Application.Current?.MainPage;
            if (currentPage != null)
            {
                _popup = new GenderFilterPopup(this);
                Shell.Current.ShowPopup(_popup);
            }
        }

        private void ClosePopup()
        {
            _popup?.Close(); // Close the current popup instance
        }

        public void ApplyGenderFilter(string selectedGender)
        {
            if (selectedGender == "None" || string.IsNullOrEmpty(selectedGender))
            {
                Students.Clear();
                foreach (var student in _originalStudents)
                {
                    Students.Add(student);
                }
            }
            else
            {
                var filteredStudents = _originalStudents
                    .Where(student => student.Gender.Equals(selectedGender, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                Students.Clear();
                foreach (var student in filteredStudents)
                {
                    Students.Add(student);
                }
            }
        }

        private async void OnViewStudent(Student student)
        {
            // Navigate to View Student page or show details
            await Shell.Current.GoToAsync($"//SpecificStudentPage",
                new Dictionary<string, object>
                {
                    ["studID"] = student.ID,
                });
        }

        private async void OnEditStudent(Student student)
        {
            // Navigate to Edit Student page
            await Shell.Current.GoToAsync($"//EditStudentPage",
                new Dictionary<string, object>
                {
                    ["studID"] = student.ID,
                });
        }

        private async void OnDeleteStudent(Student student)
        {
            if (student == null)
                return;

            // Confirm deletion with the user
            bool isConfirmed = await Application.Current.MainPage.DisplayAlert(
                "Delete Student",
                $"Are you sure you want to delete {student.Name}?",
                "Yes",
                "No"
            );

            if (!isConfirmed)
                return;

            try
            {
                // Call the service to delete the student
                var result = await _studentService.DeleteStudentAsync(student.ID);

                if (result == "Success")
                {
                    // Remove the student from the UI collection
                    Students.Remove(student);
                    _originalStudents.Remove(student);

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


    }
}

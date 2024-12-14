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
    public class HomeViewModel : BindableObject
    {
        private readonly ClassService _classService;
        private readonly StudentService _studentService;

        public ObservableCollection<Class> Classes { get; set; }
        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<string> GenderOptions { get; set; }

        //Gender
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
        public HomeViewModel()
        {
            _classService = new ClassService();
            _studentService = new StudentService();
            Classes = new ObservableCollection<Class>();
            Students = new ObservableCollection<Student>();
            GetClassesForHomeCommand = new Command(async () => await GetClassesForHome());

            GenderOptions = new ObservableCollection<string> { "None", "Male", "Female" };
            ShowGenderFilterPopupCommand = new Command(ShowGenderFilterPopup);
            ApplyFiltersCommand = new Command(ApplyFilters);

            GetClassesForHome();

        }

        public ICommand GetClassesForHomeCommand { get; }
        public ICommand ShowGenderFilterPopupCommand { get; }
        public ICommand ApplyFiltersCommand { get; }

        private async Task GetClassesForHome()
        {
            var classes = await _classService.GetClassesForHomeAsync();
            Classes.Clear();
            foreach (var class1 in classes)
            {
                Classes.Add(class1);
            }

            GetStudents();
        }

        private List<Student> _originalStudents = new List<Student>();
        private async Task GetStudents()
        {
            var students = await _studentService.GetStudentsASync();
            _originalStudents = students; // Save original list
            Students.Clear();
            foreach (var student in students)
            {
                var matchedClass = Classes.FirstOrDefault(c => c.ID == student.ClassID);
                student.ClassName = matchedClass.Acronym;

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

        private GenderFilterPopupHome _popup;

        private void ShowGenderFilterPopup()
        {
            var currentPage = Application.Current?.MainPage;
            if (currentPage != null)
            {
                _popup = new GenderFilterPopupHome(this);
                Shell.Current.ShowPopup(_popup);
            }
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
    }
}

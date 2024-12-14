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
    public class RecordPageViewModel : BindableObject
    {
        public ObservableCollection<Class> Classes { get; set; }

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

        //Selected Class
        private Class _selectedClass;
        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged();
            }
        }

        public RecordPageViewModel()
        {
            _classService = new ClassService();
            Classes = new ObservableCollection<Class>();

            GetClassesCommand = new Command(async () => await GetClasses());
            OnSelectClassCommand = new Command<Class>(OnSelectClass);

            ApplyFiltersCommand = new Command(ApplyFilters);

            GetClasses();
        }

        public ICommand GetClassesCommand { get; }
        public ICommand OnSelectClassCommand { get; }
        public ICommand ApplyFiltersCommand { get; }


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

        private async void OnSelectClass(Class selectedClass)
        {
            await Shell.Current.GoToAsync($"//SpecificRecordPage",
                new Dictionary<string, object>
                {
                    ["classId"] = selectedClass.ID,
                });
        }
    }
}

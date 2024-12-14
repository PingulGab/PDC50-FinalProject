using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using SchedEd.Model;
using SchedEd.Services;

namespace SchedEd.ViewModel
{
    public class SpecificClassViewModel : INotifyPropertyChanged
    {
        private readonly ClassService _classService;
        private Stream _selectedImageStream;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int ClassId { get; set; }

        //Class ID
        private int _classID;
        public int ClassID
        {
            get => _classID;
            set
            {
                if (_classID != value)
                {
                    _classID = value;
                    OnPropertyChanged();
                }
            }
        }

        //Class Name
        private string _className;
        public string ClassName
        {
            get => _className;
            set
            {
                if (_className != value)
                {
                    _className = value;
                    OnPropertyChanged();
                }
            }
        }

        //Class Acronym
        private string _classAcronym;
        public string ClassAcronym
        {
            get => _classAcronym;
            set
            {
                if (_classAcronym != value)
                {
                    _classAcronym = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _imagePreviewPath;
        public string ImagePreviewPath
        {
            get => _imagePreviewPath;
            set
            {
                if (_imagePreviewPath != value)
                {
                    _imagePreviewPath = value;
                    OnPropertyChanged();
                }
            }
        }

        private TimeSpan _startTime;
        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private TimeSpan _endTime;
        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private Dictionary<string, bool> _days { get; set; }

        public Dictionary<string, bool> Days
        {
            get => _days;
            set
            {
                if (_days != value)
                {
                    _days = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand BackCommand { get; }

        public SpecificClassViewModel()
        {
            _classService = new ClassService();
            BackCommand = new Command(async () => await Back());
        }

        public async Task LoadClassData(int classId)
        {
            var classData = await _classService.GetClassByIdAsync(classId);

            if (classData != null)
            {
                ClassID = classData.ID;
                ClassName = classData.Name;
                ClassAcronym = classData.Acronym;
                ImagePreviewPath = classData.Image;

                try
                {
                    string daysJson = classData.Days?.ToString();
                    if (!string.IsNullOrWhiteSpace(daysJson))
                    {
                        Days = JsonSerializer.Deserialize<Dictionary<string, bool>>(daysJson);
                    }
                    else
                    {
                        Days = new Dictionary<string, bool> {
                            { "Monday", false },
                            { "Tuesday", false },
                            { "Wednesday", false },
                            { "Thursday", false },
                            { "Friday", false },
                            { "Saturday", false }
                        };
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
                    Days = new Dictionary<string, bool> {
                        { "Monday", false },
                        { "Tuesday", false },
                        { "Wednesday", false },
                        { "Thursday", false },
                        { "Friday", false },
                        { "Saturday", false }
                    };
                }

                StartTime = TimeSpan.Parse(classData.StartTime.Trim());
                EndTime = TimeSpan.Parse(classData.EndTime);
            }
        }

        private async Task Back()
        {
            await Shell.Current.GoToAsync("//ClassPage");
        }
    }
}

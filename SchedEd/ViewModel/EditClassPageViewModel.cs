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
    public class EditClassPageViewModel : INotifyPropertyChanged
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

        public ICommand CancelCommand { get; }
        public ICommand UpdateClassCommand { get; }
        public ICommand UploadImageCommand { get; } // NEW UPLOAD IMAGE COMMAND

        public EditClassPageViewModel()
        {
            _classService = new ClassService();
            CancelCommand = new Command(async () => await Cancel());
            UpdateClassCommand = new Command(async () => await UpdateClass());
            UploadImageCommand = new Command(async () => await UploadImage()); // NEW COMMAND
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

        private async Task UploadImage()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    _selectedImageStream = await result.OpenReadAsync();
                    ImagePreviewPath = result.FullPath;
                    OnPropertyChanged(nameof(ImagePreviewPath));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task UpdateClass()
        {
            try
            {
                string fileName = string.Empty;

                if (_selectedImageStream != null)
                {
                    string sanitizedClassName = ClassName.Replace(" ", "_");
                    string sanitizedClassAcronym = ClassAcronym.Replace(" ", "_");
                    fileName = $"e_{sanitizedClassName}_{sanitizedClassAcronym}_e.png".ToLowerInvariant();
                    string projectRootPath = @"C:\PDC05_SchedEd\SchedEd\SchedEd\Resources\Images";

                    Directory.CreateDirectory(projectRootPath);
                    string fullPath = Path.Combine(projectRootPath, fileName);

                    using (var fileStream = File.Create(fullPath))
                    {
                        await _selectedImageStream.CopyToAsync(fileStream);
                    }

                    ImagePreviewPath = $"{fileName}?timestamp={DateTime.Now.Ticks}";
                }

                var updatedClass = new Model.Class
                {
                    ID = ClassID,
                    Name = ClassName,
                    Acronym = ClassAcronym,
                    Image = ImagePreviewPath,
                    Days = JsonSerializer.Serialize(Days),
                    StartTime = StartTime.ToString(@"hh\:mm\:ss"),
                    EndTime = EndTime.ToString(@"hh\:mm\:ss")
                };

                var result = await _classService.UpdateClassAsync(updatedClass);

                if (result.Contains("Success"))
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Class updated successfully!", "OK");
                    await Shell.Current.GoToAsync("//ClassPage");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to update class.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("//ClassPage");
        }
    }
}

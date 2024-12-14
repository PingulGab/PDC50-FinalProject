using System;
using System.Collections.ObjectModel;
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
    public class EditStudentPageViewModel : INotifyPropertyChanged
    {
        private Stream _selectedImageStream;
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly StudentService _studentService;
        private readonly ClassService _classService;
        public int _studID;

        public ObservableCollection<Class> Classes { get; set; }

        // Properties
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

        private string _inputName;
        public string InputName
        {
            get => _inputName;
            set
            {
                if (_inputName != value)
                {
                    _inputName = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _inputStudentID;
        public string InputStudentID
        {
            get => _inputStudentID;
            set
            {
                if (_inputStudentID != value)
                {
                    _inputStudentID = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _inputGender;
        public string InputGender
        {
            get => _inputGender;
            set
            {
                if (_inputGender != value)
                {
                    _inputGender = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _inputContactNumber;
        public string InputContactNumber
        {
            get => _inputContactNumber;
            set
            {
                if (_inputContactNumber != value)
                {
                    _inputContactNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _inputClassID;
        public int InputClassID
        {
            get => _inputClassID;
            set
            {
                if (_inputClassID != value)
                {
                    _inputClassID = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _inputBirthdate;
        public string InputBirthdate
        {
            get => _inputBirthdate;
            set
            {
                if (_inputBirthdate != value)
                {
                    _inputBirthdate = value;
                    OnPropertyChanged();
                }
            }
        }

        private Class _selectedClass;
        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged();

                if (_selectedClass != null)
                {
                    InputClassID = _selectedClass.ID;
                }
            }
        }

        public ICommand UploadImageCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand CancelCommand { get; }

        public EditStudentPageViewModel()
        {
            _studentService = new StudentService();
            _classService = new ClassService();

            Classes = new ObservableCollection<Class>();

            UploadImageCommand = new Command(async () => await UploadImage());
            UpdateCommand = new Command(async () => await UpdateStudent());
            CancelCommand = new Command(async () => await Cancel());
        }

        public async Task LoadStudent(int studID)
        {
            var student = await _studentService.GetStudentByIDAsync(studID);

            if (student != null)
            {
                _studID = student.ID;
                ImagePreviewPath = student.Image;
                InputName = student.Name;
                InputStudentID = student.StudentID;
                InputGender = student.Gender;
                InputContactNumber = student.ContactNumber;
                InputClassID = student.ClassID;
                InputBirthdate = student.Birthdate;

                // Load class details
                var classes = await _classService.GetClassesAsync();
                Classes.Clear();
                foreach (var class1 in classes)
                {
                    class1.Image += $"?timestamp={DateTime.Now.Ticks}";
                    Classes.Add(class1);
                }

                SelectedClass = Classes.FirstOrDefault(c => c.ID == InputClassID);
            }
        }

        private async Task UpdateStudent()
        {
            try
            {
                if (_selectedImageStream == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please upload an image before submitting.", "OK");
                    return;
                }

                // Replace spaces with underscores in ClassName and ClassAcronym
                string sanitizedStudentName = InputName.Replace(" ", "_");
                string sanitizedStudentID = InputStudentID.Replace(" ", "_");

                // Generate file name using sanitized ClassName and ClassAcronym
                string fileName = $"a_{sanitizedStudentName}_{sanitizedStudentID}_a.png".ToLowerInvariant();

                // Specify the path where the image will be saved
                string projectRootPath = @"C:\PDC05_SchedEd\SchedEd\SchedEd\Resources\Images";

                // Ensure the directory exists
                Directory.CreateDirectory(projectRootPath);

                // Full path to save the image
                string fullPath = Path.Combine(projectRootPath, fileName);

                // Save the image to the specified location
                using (var fileStream = File.Create(fullPath))
                {
                    await _selectedImageStream.CopyToAsync(fileStream);
                }

                ImagePreviewPath = $"{fileName}?timestamp={DateTime.Now.Ticks}";

                // Create student object with updated data
                var updatedStudent = new Student
                {
                    ID = _studID,
                    Image = ImagePreviewPath,
                    StudentID = InputStudentID,
                    Name = InputName,
                    Gender = InputGender,
                    ContactNumber = InputContactNumber,
                    ClassID = InputClassID,
                    Birthdate = InputBirthdate
                };

                // Update student data via service
                var result = await _studentService.UpdateStudentAsync(updatedStudent);

                if (result.Contains("Success"))
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Student details updated successfully.", "OK");
                    await Shell.Current.GoToAsync("//StudentsPage");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", result, "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task UploadImage()
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

        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("//StudentsPage");
        }
    }
}

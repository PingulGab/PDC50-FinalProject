using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using SchedEd.Services;
using SchedEd.Model;

namespace SchedEd.ViewModel
{
    public class AddStudentPageViewModel : BindableObject
    {
        public ObservableCollection<Class> Classes { get; set; }
        private readonly ClassService _classService;
        private readonly StudentService _studentService;

        public string ImagePreviewPath { get; set; }
        private Stream _selectedImageStream;

        //Selected Class
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

        // Add Student - Properties
        public string InputName { get; set; }
        public string InputGender { get; set; }
        public string InputStudentID { get; set; }
        public string InputContactNumber { get; set; }
        public int InputClassID { get; set; }
        public string InputBirthdate { get; set; }
        public string InputElementaryEducation { get; set; }
        public string InputSecondaryEducation { get; set; }
        public string InputTertiaryEducation { get; set; }
        public AddStudentPageViewModel()
        {
            _classService = new ClassService();
            _studentService = new StudentService();
            Classes = new ObservableCollection<Class>();

            GetClassesCommand = new Command(async () => await GetClasses());
            UploadImageCommand = new Command(async () => await UploadImage());
            SubmitCommand = new Command(async () => await Submit());
            CancelCommand = new Command(async () => await Cancel());

            GetClasses();
        }

        public ICommand GetClassesCommand { get; }
        public ICommand UploadImageCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        private async Task Submit()
        {
            try
            {
                //Validate Input
                if (string.IsNullOrWhiteSpace(ImagePreviewPath) ||
                    string.IsNullOrWhiteSpace(InputName) ||
                    string.IsNullOrWhiteSpace(InputGender) ||
                    string.IsNullOrWhiteSpace(InputStudentID) ||
                    string.IsNullOrWhiteSpace(InputContactNumber) ||
                    (InputClassID == 0) ||
                    string.IsNullOrWhiteSpace(InputBirthdate) ||
                    string.IsNullOrWhiteSpace(InputElementaryEducation) ||
                    string.IsNullOrWhiteSpace(InputSecondaryEducation) ||
                    string.IsNullOrWhiteSpace(InputTertiaryEducation))
                {
                    // Create a list to store missing field names
                    List<string> missingFields = new List<string>();

                    // Check each input field and add its name to the list if it's missing
                    if (string.IsNullOrWhiteSpace(ImagePreviewPath))
                        missingFields.Add("Image");

                    if (string.IsNullOrWhiteSpace(InputName))
                        missingFields.Add("Name");

                    if (string.IsNullOrWhiteSpace(InputGender))
                        missingFields.Add("Gender");

                    if (string.IsNullOrWhiteSpace(InputStudentID))
                        missingFields.Add("Student ID");

                    if (string.IsNullOrWhiteSpace(InputContactNumber))
                        missingFields.Add("Contact Number");

                    if (InputClassID == 0)
                        missingFields.Add("Class");

                    if (string.IsNullOrWhiteSpace(InputBirthdate))
                        missingFields.Add("Birthdate");

                    if (string.IsNullOrWhiteSpace(InputElementaryEducation))
                        missingFields.Add("Elementary Education");

                    if (string.IsNullOrWhiteSpace(InputSecondaryEducation))
                        missingFields.Add("Secondary Education");

                    if (string.IsNullOrWhiteSpace(InputTertiaryEducation))
                        missingFields.Add("Tertiary Education");

                    // Check if there are any missing fields
                    if (missingFields.Count > 0)
                    {
                        // Create a message that lists all the missing fields
                        string missingFieldsMessage = "Please fill in the following fields before submitting:\n\n" +
                                                      string.Join("\n", missingFields);
                        await Application.Current.MainPage.DisplayAlert("Error", missingFieldsMessage, "OK");
                        return;
                    }

                }

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

                // Create class object with the new image file name
                var newStudent = new Student
                {
                    Image = $"{fileName}?timestamp={DateTime.Now.Ticks}",
                    Name = InputName,
                    Gender = InputGender,
                    StudentID = InputStudentID,
                    ContactNumber = InputContactNumber,
                    ClassID = InputClassID,
                    Birthdate = InputBirthdate,
                    ElementaryEducation = InputElementaryEducation,
                    SecondaryEducation = InputSecondaryEducation,
                    TertiaryEducation = InputTertiaryEducation
                };

                // Send data to the server
                var result = await _studentService.AddStudentAsync(newStudent);

                // Check server response
                if (!string.IsNullOrEmpty(result))
                {
                    // Navigate back to ClassPage
                    await Application.Current.MainPage.DisplayAlert("Success", "Student was added Successfully.", "OK");
                    await Shell.Current.GoToAsync("//StudentsPage");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to save the class.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task GetClasses()
        {
            var classes = await _classService.GetClassesAsync();
            Classes.Clear();
            foreach (var class1 in classes)
            {
                class1.Image += $"?timestamp={DateTime.Now.Ticks}";
                Classes.Add(class1);
            }
        }

        private async Task UploadImage()
        {
            try
            {
                // Pick an image
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

        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("//StudentsPage");
        }
    }
}

using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using SchedEd.Model;
using SchedEd.Services;

namespace SchedEd.ViewModel
{
    public class AddClassPageViewModel : BindableObject
    {
        private readonly ClassService _classService;

        // Properties
        public string ClassName { get; set; }
        public string ClassAcronym { get; set; }
        public string ImagePreviewPath { get; set; }
        public DaysSelection Days { get; set; } = new DaysSelection();
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        private Stream _selectedImageStream;

        // Commands
        public ICommand UploadImageCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        // Constructor
        public AddClassPageViewModel()
        {
            _classService = new ClassService();
            UploadImageCommand = new Command(async () => await UploadImage());
            SubmitCommand = new Command(async () => await Submit());
            CancelCommand = new Command(async () => await Cancel());
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

        private async Task Submit()
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(ClassName) || string.IsNullOrWhiteSpace(ClassAcronym))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please fill in all the fields before submitting.", "OK");
                    return;
                }

                if (_selectedImageStream == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please upload an image before submitting.", "OK");
                    return;
                }

                // Replace spaces with underscores in ClassName and ClassAcronym
                string sanitizedClassName = ClassName.Replace(" ", "_");
                string sanitizedClassAcronym = ClassAcronym.Replace(" ", "_");

                // Generate file name using sanitized ClassName and ClassAcronym
                string fileName = $"a_{sanitizedClassName}_{sanitizedClassAcronym}_a.png".ToLowerInvariant();

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

                // Convert Days to JSON
                string daysJson = Days.ToJson();
                Debug.WriteLine($"Serialized Days JSON: {daysJson}");

                // Convert StartTime and EndTime to format "HH:mm:ss"
                string formattedStartTime = StartTime.ToString(@"hh\:mm\:ss");
                string formattedEndTime = EndTime.ToString(@"hh\:mm\:ss");

                Debug.WriteLine($"StartTime: {formattedStartTime}, EndTime: {formattedEndTime}");

                // Create class object with the new image file name
                var newClass = new Class
                {
                    Name = ClassName,
                    Acronym = ClassAcronym,
                    // Append timestamp to force reload
                    Image = $"{fileName}?timestamp={DateTime.Now.Ticks}",
                    Days = daysJson,
                    StartTime = formattedStartTime, // Use formatted time
                    EndTime = formattedEndTime // Use formatted time
                };

                // Send data to the server
                var result = await _classService.AddClassAsync(newClass);

                // Check server response
                if (!string.IsNullOrEmpty(result))
                {
                    // Navigate back to ClassPage
                    await Shell.Current.GoToAsync("//ClassPage");
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
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("//ClassPage");
        }
    }
}

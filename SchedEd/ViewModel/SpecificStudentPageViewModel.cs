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
    public class SpecificStudentPageViewModel : INotifyPropertyChanged
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

        private string _inputElementaryEducation;
        public string InputElementaryEducation
        {
            get => _inputElementaryEducation;
            set
            {
                if (_inputElementaryEducation != value)
                {
                    _inputElementaryEducation = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _inputSecondaryEducation;
        public string InputSecondaryEducation
        {
            get => _inputSecondaryEducation;
            set
            {
                if (_inputSecondaryEducation != value)
                {
                    _inputSecondaryEducation = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _inputTertiaryEducation;
        public string InputTertiaryEducation
        {
            get => _inputTertiaryEducation;
            set
            {
                if (_inputTertiaryEducation != value)
                {
                    _inputTertiaryEducation = value;
                    OnPropertyChanged();
                }
            }
        }

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

        private string _attendanceSummary;
        public string AttendanceSummary
        {
            get => _attendanceSummary;
            set
            {
                if (_attendanceSummary != value)
                {
                    _attendanceSummary = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<StudentAttendanceRecord> _attendanceRecords;
        public ObservableCollection<StudentAttendanceRecord> AttendanceRecords
        {
            get => _attendanceRecords;
            set
            {
                if (_attendanceRecords != value)
                {
                    _attendanceRecords = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand UploadImageCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand GetClassesCommand { get; }

        public SpecificStudentPageViewModel()
        {
            _studentService = new StudentService();
            _classService = new ClassService();

            Classes = new ObservableCollection<Class>();

            GetClassesCommand = new Command(async () => await GetClasses());
            BackCommand = new Command(async () => await Back());

            GetClasses();
        }

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
        }

        public async Task LoadStudent(int studID)
        {
            var student = await _studentService.GetStudentByIDAsync(studID);

            if (student != null)
            {
                // Existing student details
                _studID = student.ID;
                ImagePreviewPath = student.Image;
                ClassName = student.ClassName;
                InputName = student.Name;
                InputStudentID = student.StudentID;
                InputGender = student.Gender;
                InputContactNumber = student.ContactNumber;
                InputClassID = student.ClassID;
                InputBirthdate = student.Birthdate;
                InputElementaryEducation = student.ElementaryEducation;
                InputSecondaryEducation = student.SecondaryEducation;
                InputTertiaryEducation = student.TertiaryEducation;

                // Load attendance records
                var attendance = await _studentService.GetAttendanceRecordsAsync(studID);


                if (attendance != null && attendance.Any())
                {
                    var presentCount = attendance.Count(a => a.Status == "Present");
                    var absentCount = attendance.Count(a => a.Status == "Absent");
                    var excusedCount = attendance.Count(a => a.Status == "Excused");

                    AttendanceSummary = $"{presentCount} Present, {absentCount} Absent, {excusedCount} Excused";
                    AttendanceRecords = new ObservableCollection<StudentAttendanceRecord>(attendance);
                }
                else
                {
                    AttendanceSummary = "No attendance records found.";
                    AttendanceRecords = new ObservableCollection<StudentAttendanceRecord>();
                }
            }
        }

        private async Task Back()
        {
            await Shell.Current.GoToAsync("//StudentsPage");
        }
    }
}

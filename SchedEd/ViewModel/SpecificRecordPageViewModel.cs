using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using SchedEd.Model;
using SchedEd.Services;

namespace SchedEd.ViewModel
{
    public class SpecificRecordPageViewModel : INotifyPropertyChanged
    {
        private readonly AttendanceService _attendanceService;
        private readonly StudentService _studentService;
        private readonly ClassService _classService;

        public ObservableCollection<Student> Students { get; set; }

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                if (_selectedStudent != value)
                {
                    _selectedStudent = value;
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

        private int _currentStudentIndex;

        public ICommand PresentCommand { get; }
        public ICommand AbsentCommand { get; }
        public ICommand ExcusedCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand CancelCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public SpecificRecordPageViewModel()
        {
            _attendanceService = new AttendanceService();
            _studentService = new StudentService();
            _classService = new ClassService();

            Students = new ObservableCollection<Student>();

            PresentCommand = new Command(async () => await MarkAttendance("Present"));
            AbsentCommand = new Command(async () => await MarkAttendance("Absent"));
            ExcusedCommand = new Command(async () => await MarkAttendance("Excused"));

            BackCommand = new Command(() => NavigateToPreviousStudent());
            NextCommand = new Command(() => NavigateToNextStudent());

            CancelCommand = new Command(() => CancelAsync());
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadClassData(int classId)
        {
            var classData = await _classService.GetClassByIdAsync(classId);

            if (classData != null)
            {
                ClassName = classData.Name;
                ClassAcronym = classData.Acronym;
            }

            var students = await _studentService.GetStudentByClassIDAsync(classId);
            Students.Clear();
            foreach (var student in students.OrderBy(s => s.Name))
            {
                Students.Add(student);
            }

            if (Students.Count > 0)
            {
                _currentStudentIndex = 0;
                SelectedStudent = Students[_currentStudentIndex];
            }
        }

        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("//RecordPage");
        }
        private async Task MarkAttendance(string status)
        {
            if (SelectedStudent == null)
                return;

            var todayDate = DateTime.Now.ToString("yyyy-MM-dd");

            // Check if attendance already exists for the student
            var existingRecord = await _attendanceService.GetAttendanceRecordAsync(SelectedStudent.ID, SelectedStudent.ClassID, todayDate);

            // Mark attendance by either updating or creating a record
            if (existingRecord != null)
            {
                await _attendanceService.RecordAttendanceAsync(SelectedStudent.ID, SelectedStudent.ClassID, status);
            }
            else
            {
                await _attendanceService.RecordAttendanceAsync(SelectedStudent.ID, SelectedStudent.ClassID, status);
            }

            // Navigate to the next student after marking attendance
            NavigateToNextStudent();
        }

        private void NavigateToNextStudent()
        {
            if (_currentStudentIndex < Students.Count - 1)
            {
                _currentStudentIndex++;
                SelectedStudent = Students[_currentStudentIndex];
            }
        }

        private void NavigateToPreviousStudent()
        {
            if (_currentStudentIndex > 0)
            {
                _currentStudentIndex--;
                SelectedStudent = Students[_currentStudentIndex];
            }
        }

        private int _classID;
        public int ClassID
        {
            get => _classID;
            set
            {
                _classID = value;
                OnPropertyChanged();
            }
        }
    }
}

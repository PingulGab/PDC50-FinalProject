using System.Text.Json;
using System.ComponentModel;

namespace SchedEd.ViewModel
{
    public class DaysSelection : BindableObject
    {
        private bool _monday;
        public bool Monday
        {
            get => _monday;
            set
            {
                if (_monday != value)
                {
                    _monday = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _tuesday;
        public bool Tuesday
        {
            get => _tuesday;
            set
            {
                if (_tuesday != value)
                {
                    _tuesday = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _wednesday;
        public bool Wednesday
        {
            get => _wednesday;
            set
            {
                if (_wednesday != value)
                {
                    _wednesday = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _thursday;
        public bool Thursday
        {
            get => _thursday;
            set
            {
                if (_thursday != value)
                {
                    _thursday = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _friday;
        public bool Friday
        {
            get => _friday;
            set
            {
                if (_friday != value)
                {
                    _friday = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _saturday;
        public bool Saturday
        {
            get => _saturday;
            set
            {
                if (_saturday != value)
                {
                    _saturday = value;
                    OnPropertyChanged();
                }
            }
        }

        // Method to convert the days to JSON string
        public string ToJson()
        {
            return JsonSerializer.Serialize(new
            {
                Monday,
                Tuesday,
                Wednesday,
                Thursday,
                Friday,
                Saturday
            });
        }
    }
}
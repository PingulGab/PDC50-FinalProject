using SchedEd.Model;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchedEd.Services
{
    public class AttendanceService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost/SchedEd/";

        public AttendanceService()
        {
            _httpClient = new HttpClient();
        }

        // Check if attendance record exists
        public async Task<Attendance> GetAttendanceRecordAsync(int studentId, int classId, string date)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}get_attendance.php?student_id={studentId}&class_id={classId}&date={date}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Attendance>(json);
            }

            return null;
        }

        // Record or update attendance
        public async Task RecordAttendanceAsync(int studentId, int classId, string status)
        {
            var attendanceData = new
            {
                StudentID = studentId,
                ClassID = classId,
                Date = DateTime.Now.ToString("yyyy-MM-dd"),
                Status = status
            };

            var json = JsonSerializer.Serialize(attendanceData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync($"{BaseUrl}record_attendance.php", content);
        }
    }
}

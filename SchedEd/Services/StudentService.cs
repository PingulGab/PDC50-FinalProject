using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using SchedEd.Model;

namespace SchedEd.Services
{
    public class StudentService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost/SchedEd/";
        public StudentService()
        {
            _httpClient = new HttpClient();
        }

        //GetFromJsonAsync - method call HTTP GET
        //PostAsJsonAsync - method to call HTTP POST
        //ReadAsStringAsync - method to read the current of HTTPContent

        //Get Student
        public async Task<List<Student>> GetStudentsASync()
        {
            Debug.WriteLine("Get Students from Services Method Reached");
            var response = await _httpClient.GetFromJsonAsync<List<Student>>($"{BaseUrl}students_get.php");
            return response ?? new List<Student>();
        }

        //Add Student
        public async Task<string> AddStudentAsync(Student student)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}students_add.php", student);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        //Delete Student
        public async Task<string> DeleteStudentAsync(int studID)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}students_delete.php", new { id = studID });
            var result = await response.Content.ReadAsStringAsync();

            try
            {
                // Decode the JSON response
                var jsonResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(result);

                // Check if the "message" field indicates success
                if (jsonResponse != null && jsonResponse.TryGetValue("message", out var message) && message == "Success")
                {
                    return "Success";
                }

                // Return the error message or a general error if unavailable
                return jsonResponse?.GetValueOrDefault("message") ?? "Error: Unknown error occurred.";
            }
            catch (JsonException)
            {
                // Handle unexpected response format
                return "Error: Invalid response from server.";
            }
        }

        // Fetch a specific class by its ID
        public async Task<Student> GetStudentByIDAsync(int studID)
        {
            var response = await _httpClient.GetFromJsonAsync<List<Student>>($"{BaseUrl}students_get_specific.php?id={studID}");
            return response?.FirstOrDefault();
        }

        // Fetch a specific class by its ClassID
        public async Task<List<Student>> GetStudentByClassIDAsync(int classID)
        {
            var response = await _httpClient.GetFromJsonAsync<List<Student>>($"{BaseUrl}students_get_byClass.php?id={classID}");
            return response ?? new List<Student>();
        }

        //Update Student
        public async Task<string> UpdateStudentAsync(Student student)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}students_update.php", student);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        //Attendance Record
        public async Task<List<StudentAttendanceRecord>> GetAttendanceRecordsAsync(int studentId)
        {
            var response = await _httpClient.GetStringAsync($"{BaseUrl}student_attendanceRecord.php?student_id={studentId}");

            // Handle possible deserialization errors
            try
            {
                return JsonSerializer.Deserialize<List<StudentAttendanceRecord>>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Deserialization error: {ex.Message}");
                return new List<StudentAttendanceRecord>();
            }
        }

    }
}

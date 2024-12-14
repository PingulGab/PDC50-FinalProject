using SchedEd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SchedEd.Services
{
    public class ClassService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost/SchedEd/";
        public ClassService()
        {
            _httpClient = new HttpClient();
        }

        //GetFromJsonAsync - method call HTTP GET
        //PostAsJsonAsync - method to call HTTP POST
        //ReadAsStringAsync - method to read the current of HTTPContent

        //Get Clases
        public async Task<List<Class>> GetClassesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Class>>($"{BaseUrl}class_get.php");
            return response ?? new List<Class>();
        }

        //Get Class for Home (Limit: 3)
        public async Task<List<Class>> GetClassesForHomeAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Class>>($"{BaseUrl}class_get_home.php");
            return response ?? new List<Class>();
        }

        //Add Class
        public async Task<string> AddClassAsync(Class class1)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}class_add.php", class1);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        //Delete Class
        public async Task<string> DeleteClassAsync(int classID)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}class_delete.php", new { id = classID });
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
        public async Task<Class> GetClassByIdAsync(int classId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<Class>>($"{BaseUrl}class_get_specific.php?id={classId}");
            return response?.FirstOrDefault();
        }

        //Update Class
        public async Task<string> UpdateClassAsync(Class class1)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}class_update.php", class1);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}

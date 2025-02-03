using Data.Models;
using System.Net;
using System.Net.Http;

namespace Blazor
{
    public class UserRegistrationRequest
    {
        public User User { get; set; }
        public string Password { get; set; }
    }
    public class UserLoginRequest
    {
        public string Phone { get; set; }
        public string Password { get; set; }
    }
    public class ApiServices
    {
        private readonly HttpClient _httpClient;
        private string basic = "http://localhost:9999/";
        public ApiServices()
        {
            _httpClient = new HttpClient();
        }
        public async Task<List<User>> GetListUsersAsync()
        {
            string s = $"{basic}api/users/GetListUsers";
            return await _httpClient.GetFromJsonAsync<List<User>>(s);
        }
        public async Task<List<Data.Models.Thread>> GetListThreadsByIdAsync(int id)
        {
            string s = $"{basic}api/threads/Threads/{id}";
            return await _httpClient.GetFromJsonAsync<List<Data.Models.Thread>>(s);
        }
        public async Task<List<Post>> GetListPostsByThreadIdAsync(int id)
        {
            string s = $"{basic}api/posts/thread/{id}";
            return await _httpClient.GetFromJsonAsync<List<Post>>(s);
        }
        public async Task<List<Category>> GetListCategoriesAsync()
        {
            string s = $"{basic}api/categories/GetListCategories";
            return await _httpClient.GetFromJsonAsync<List<Category>>(s);
        }
        public async Task<List<Animal>> GetListAnimalsAsync()
        {
            string s = $"{basic}api/animals/GetListAnimals";
            return await _httpClient.GetFromJsonAsync<List<Animal>>(s);
        }
        public async Task<List<string>> GetListBreed()
        {
            string s = $"{basic}api/animals/GetListBreed";
            return await _httpClient.GetFromJsonAsync<List<string>>(s);
        }
        public async Task<List<string>> GetListBreedByType(string type)
        {
            string s = $"{basic}api/animals/GetListBreedByType/{type}";
            return await _httpClient.GetFromJsonAsync<List<string>>(s);
        }
        public async Task<List<string>> GetListType()
        {
            string s = $"{basic}api/animals/GetListType";
            return await _httpClient.GetFromJsonAsync<List<string>>(s);
        }
        public async Task<User> GetUserAsync(int id)
        {
            string s = $"{basic}api/users/GetUser/{id}";
            return await _httpClient.GetFromJsonAsync<User>(s);
        }
        public async Task UserRegistration(User user, string password)
        {
            string s = $"{basic}api/users/Registration";
            var registrationRequest = new UserRegistrationRequest
            {
                User = user,
                Password = password
            };
            await _httpClient.PostAsJsonAsync<UserRegistrationRequest>(s, registrationRequest);

        }

        public async Task CreateAnimal(Animal animal)
        {
            string s = $"{basic}api/animals/NewAnimal";
            await _httpClient.PostAsJsonAsync<Animal>(s, animal);
        }
        public async Task CreatePost(Post post)
        {
            string s = $"{basic}api/posts/CreatePost";
            await _httpClient.PostAsJsonAsync<Post>(s, post);
        }
        public async Task CreateThread(Data.Models.Thread thread)
        {
            string s = $"{basic}api/threads/CreateThread";
            await _httpClient.PostAsJsonAsync<Data.Models.Thread>(s, thread);
        }
        public async Task<User> UserLogin(string phone, string password)
        {
            var loginRequest = new UserLoginRequest
            {
                Phone = phone,
                Password = password
            };
            string s = $"{basic}api/users/Login";
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(s, loginRequest);

            if (response.IsSuccessStatusCode)
            {
                User user = await response.Content.ReadFromJsonAsync<User>();
                return user;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Invalid phone or password");
            }
            else
            {
                throw new HttpRequestException($"Error during login: {response.StatusCode}");
            }
        }
        public async Task UpdateUser(User user)
        {
            string s = $"{basic}api/users/UpdateUser/{user.user_id}";
            await _httpClient.PutAsJsonAsync<User>(s, user);
        }
        public async Task UpdateUserPassword(User user, string password)
        {
            var registrationRequest = new UserRegistrationRequest
            {
                User = user,
                Password = password
            };
            string s = $"{basic}api/users/UpdateUserPassword/{user.user_id}";
            await _httpClient.PutAsJsonAsync<UserRegistrationRequest>(s, registrationRequest);
        }
    }

}

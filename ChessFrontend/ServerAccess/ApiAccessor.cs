using System.Net;
using System.Net.Http.Json;
using Shared.Model;
using Shared.Types;

namespace ChessFrontend.ServerAccess;

public class ApiAccessor
{
    private readonly HttpClient? _client;

    public ApiAccessor(HttpClient client)
    {
        _client = client;
    }

    // -------------------- Responses --------------------

    private async Task<ResponseModel> GetResponseModel(Func<HttpClient, Task<HttpResponseMessage>> request)
    {
        if (_client is null)
            return new ResponseModel()
            {
                Success = false,
                Message = "Client is null"
            };

        try
        {
            var response = await request(_client);
            if (!response.IsSuccessStatusCode)
            {
                return new ResponseModel()
                {
                    Success = false,
                    Message = "Request failed: " + await response.Content.ReadAsStringAsync()
                };
            }

            return new ResponseModel()
            {
                Success = true,
            };
        }
        catch (Exception e)
        {
            return new ResponseModel()
            {
                Success = false,
                Message = e.Message
            };
        }
    }

    private async Task<ResponseModel<T>> GetResponseModel<T>(Func<HttpClient, Task<HttpResponseMessage>> request)
        where T : class
    {
        if (_client is null)
            return new ResponseModel<T>()
            {
                Success = false,
                Message = "Client is null"
            };

        try
        {
            var response = await request(_client);
            if (!response.IsSuccessStatusCode)
            {
                return new ResponseModel<T>()
                {
                    Success = false,
                    Message = "Request failed: " + await response.Content.ReadAsStringAsync()
                };
            }

            return new ResponseModel<T>()
            {
                Success = true,
                Data = await response.Content.ReadFromJsonAsync<T>()
            };
        }
        catch (Exception e)
        {
            return new ResponseModel<T>()
            {
                Success = false,
                Message = e.Message
            };
        }
    }

    // -------------------- API --------------------

    public async Task<ResponseModel<string>> RegisterAsync(string username, string password)
    {
        var registerModel = new RegistrerModel(){ Username = username, Password = password };
        return await GetResponseModel<string>(client => client.PostAsJsonAsync("/register", registerModel));

    }

    public async Task<ResponseModel<string>> LoginAsync(string username, string password)
    {
        var loginModel = new LoginModel(){ Username = username, Password = password };
        return await GetResponseModel<string>(client => client.PostAsJsonAsync("/login", loginModel));
    }
}
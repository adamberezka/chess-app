using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ChessApp.Api;

namespace ChessAppClient.Communication;

public class RequestHandler
{
    private static HttpClient HttpClient = new ();
    public static string HostAddress;

    public static bool SetHostAddress(string address)
    {
        RequestHandler.HostAddress = address;
        if (!address.StartsWith("http://"))
            address = "http://" + address;
        Uri HostAddress;
        bool isUriValid = Uri.TryCreate(address, UriKind.Absolute, out HostAddress);
        if (isUriValid)
        {
            HttpClient.BaseAddress = HostAddress;
        }

        return isUriValid;
    }

    public static Boolean IsServerListening()
    {
        var getAsync = HttpClient.GetAsync(ChessAppApi.HEALTH);
        HealthCheckResponse healthCheckResponse = JsonSerializer.Deserialize<HealthCheckResponse>(getAsync.Result.Content.ReadAsStringAsync().Result);
        if (healthCheckResponse.status.Equals(HealthCheckResponse.STATUS_OK))
            return true;
        return false;
    }

    public static CreateUserResponse? Register(CreateUserRequest request)
    {
        var postRequest = HttpClient.PostAsJsonAsync(ChessAppApi.USER, request);
        if (postRequest.Result.StatusCode == HttpStatusCode.Conflict)
            return null;
        return JsonSerializer.Deserialize<CreateUserResponse>(postRequest.Result.Content.ReadAsStringAsync().Result, new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
    }

    public static LoginResponse? Login(LoginRequest request)
    {
        var postAsync = HttpClient.PostAsJsonAsync(ChessAppApi.USER_LOGIN, request);
        var httpResponseMessage = postAsync.Result;
        if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            return null;
        return JsonSerializer.Deserialize<LoginResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result, new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
    }
}
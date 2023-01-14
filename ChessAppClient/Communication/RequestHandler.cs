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

    public static bool SetHostAddress(string address)
    {
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

    public static Boolean Register(CreateUserRequest request)
    {
        var postRequest = HttpClient.PostAsJsonAsync(ChessAppApi.USER, request);
        return postRequest.Result.StatusCode != HttpStatusCode.Conflict;
    }

    public static LoginResponse? Login(LoginRequest request)
    {
        var postAsync = HttpClient.PostAsJsonAsync(ChessAppApi.USER_LOGIN, request);
        var httpResponseMessage = postAsync.Result;
        if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            return null;
        return JsonSerializer.Deserialize<LoginResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);
    }
}
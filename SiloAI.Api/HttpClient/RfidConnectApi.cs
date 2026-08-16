using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Logging;
using SiloAI.Application.Shared;
using SiloAI.Shared.JsonConverters;

namespace Silo.Api;

public partial class RfidConnectApi(IConfiguration Configuration
    , NavigationManager NavigationManager
    , ILogger<RfidConnectApi> Logger
    , ProtectedLocalStorage Storage)
    : HttpClientHandler
{
    private string baseUri;
    private const int bufferSize = 4096;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var storageResult = await Storage.GetAsync<string>("jwt");

            if (storageResult.Success)
            {
                request.Headers.Authorization = new("Bearer", storageResult.Value);
            }
            else
            {
                request.Headers.Remove("Authorization");
            }
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, ex.Message);

            request.Headers.Remove("Authorization");
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            if (response.Content.Headers.ContentType?.MediaType == "application/json")
            {
                var errorResult = await response.Content.ReadFromJsonAsync<ApiResponse<string>>(cancellationToken: cancellationToken);

                Logger.LogWarning($"Auth log error: {errorResult}");

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await Storage.DeleteAsync("token");

                    await Storage.DeleteAsync("username");

                    await Storage.DeleteAsync("jwt");

                    await Storage.DeleteAsync("signTime");

                    NavigationManager.NavigateTo("/account/login", true);
                }

                if (response.StatusCode == HttpStatusCode.Ambiguous)
                {
                    NavigationManager.NavigateTo("/settings/apisettings");
                }
            }
        }

        return response;
    }

    #region PostAsync
    public async Task<ApiResponse<T>> PostAsync<T>(string methodName, params KeyValuePair<string, object>[] data)
    {
        if (baseUri.HasNoValue())
        {
            SetUri();
        }

        var dataDict = new Dictionary<string, object>();
        foreach (var item in data)
        {
            dataDict.Add(item.Key, item.Value);
        }

        var finalDic = new Dictionary<string, object>();
        finalDic.Add("interface", "RestAPI");
        finalDic.Add("method", methodName);
        finalDic.Add("parameters", dataDict);

        JsonSerializerOptions option = new()
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new NullableDateTimeConverter()
            }
        };

        var passDataJsonString = JsonSerializer.Serialize(finalDic);
        var buffer = Encoding.UTF8.GetBytes(passDataJsonString);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpRequestMessage request = new(HttpMethod.Post, baseUri + "Wms/PostObject");

        request.Content = byteContent;

        var resultStream = await SendAsync(request, new CancellationToken());

        Stream httpStream = await resultStream.Content.ReadAsStreamAsync();

        var result = await JsonSerializer.DeserializeAsync<ApiResponse<T>>(httpStream, option);

        return result;
    }

    public async Task<byte[]> PostAsync(string uri, object data)
    {
        if (baseUri.HasNoValue())
        {
            SetUri();
        }

        JsonSerializerOptions option = new()
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new NullableDateTimeConverter()
            }
        };

        var passDataJsonString = JsonSerializer.Serialize(data);
        var buffer = Encoding.UTF8.GetBytes(passDataJsonString);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpRequestMessage request = new(HttpMethod.Post, baseUri + uri);

        request.Content = byteContent;

        var result = await SendAsync(request, new());

        if (result.IsSuccessStatusCode)
        {
            var bytes = await result.Content.ReadAsByteArrayAsync();
            return bytes;
        }

        return null;
    }

    public async Task<ApiResponse<T>> PostAsyncByUri<T>(string uri, string methodName, params KeyValuePair<string, object>[] data)
    {
        if (baseUri.HasNoValue())
        {
            SetUri();
        }

        var dataDict = new Dictionary<string, object>();
        foreach (var item in data)
        {
            dataDict.Add(item.Key, item.Value);
        }

        var finalDic = new Dictionary<string, object>();
        finalDic.Add("interface", "RestAPI");
        finalDic.Add("method", methodName);
        finalDic.Add("parameters", dataDict);

        JsonSerializerOptions option = new()
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new NullableDateTimeConverter()
            }
        };

        var passDataJsonString = JsonSerializer.Serialize(finalDic);
        var buffer = Encoding.UTF8.GetBytes(passDataJsonString);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpRequestMessage request = new(HttpMethod.Post, baseUri + uri);

        request.Content = byteContent;

        var resultStream = await SendAsync(request, new CancellationToken());

        Stream httpStream = await resultStream.Content.ReadAsStreamAsync();

        var result = await JsonSerializer.DeserializeAsync<ApiResponse<T>>(httpStream, option);

        return result;
    }

    public async Task<ApiResponse<T>> PostAsyncByUriAndContext<T>(string uri, string methodName, JsonSerializerContext context, params KeyValuePair<string, object>[] data)
    {
        if (baseUri.HasNoValue())
        {
            SetUri();
        }

        var dataDict = new Dictionary<string, object>();
        foreach (var item in data)
        {
            dataDict.Add(item.Key, item.Value);
        }

        var finalDic = new Dictionary<string, object>();
        finalDic.Add("interface", "RestAPI");
        finalDic.Add("method", methodName);
        finalDic.Add("parameters", dataDict);

        JsonSerializerOptions option = new()
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            TypeInfoResolver = context,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new NullableDateTimeConverter()
            }
        };

        var passDataJsonString = JsonSerializer.Serialize(finalDic);
        var buffer = Encoding.UTF8.GetBytes(passDataJsonString);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpRequestMessage request = new(HttpMethod.Post, baseUri + uri);

        request.Content = byteContent;

        var resultStream = await SendAsync(request, new CancellationToken());

        Stream httpStream = await resultStream.Content.ReadAsStreamAsync();

        var result = await JsonSerializer.DeserializeAsync<ApiResponse<T>>(httpStream, option);

        return result;
    }

    public async Task<ApiResponse<T>> PostAsyncByUri<T>(string uri, params KeyValuePair<string, object>[] data)
    {
        if (baseUri.HasNoValue())
        {
            SetUri();
        }

        var dataDict = new Dictionary<string, object>();

        foreach (var item in data)
        {
            dataDict.Add(item.Key, item.Value);
        }

        JsonSerializerOptions option = new()
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new NullableDateTimeConverter()
            }
        };

        var passDataJsonString = JsonSerializer.Serialize(dataDict);
        var buffer = Encoding.UTF8.GetBytes(passDataJsonString);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpRequestMessage request = new(HttpMethod.Post, baseUri + uri);

        request.Content = byteContent;

        var resultStream = await SendAsync(request, new CancellationToken());

        Stream httpStream = await resultStream.Content.ReadAsStreamAsync();

        using StreamReader sr = new StreamReader(httpStream);

        var result = await JsonSerializer.DeserializeAsync<ApiResponse<T>>(httpStream, option);

        return result;
    }

    public async Task<ApiResponse<T>> PostAsyncByContext<T>(string methodName, JsonSerializerContext context, params KeyValuePair<string, object>[] data)
    {
        if (baseUri.HasNoValue())
        {
            SetUri();
        }

        var dataDict = new Dictionary<string, object>();
        foreach (var item in data)
        {
            dataDict.Add(item.Key, item.Value);
        }

        JsonSerializerOptions option = new()
        {
            PropertyNameCaseInsensitive = true,
            TypeInfoResolver = context,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new NullableDateTimeConverter()
            }
        };

        var finalDic = new Dictionary<string, object>();
        finalDic.Add("interface", "RestAPI");
        finalDic.Add("method", methodName);
        finalDic.Add("parameters", dataDict);

        var passDataJsonString = JsonSerializer.Serialize(finalDic);
        var buffer = Encoding.UTF8.GetBytes(passDataJsonString);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpRequestMessage request = new(HttpMethod.Post, baseUri + "Wms/PostObject");

        request.Content = byteContent;

        var resultStream = await SendAsync(request, new CancellationToken());

        var result = (ApiResponse<T>)await resultStream.Content.ReadFromJsonAsync(typeof(ApiResponse<T>), option);

        return result;
    }

    public async Task<ApiResponse<T>> PostAsyncByContextAndOption<T>(string methodName
        , JsonSerializerContext context
        , JsonSerializerOptions options
        , params KeyValuePair<string, object>[] data)
    {
        if (baseUri.HasNoValue())
        {
            SetUri();
        }

        options.PropertyNameCaseInsensitive = true;

        options.TypeInfoResolver = context;

        var dataDict = new Dictionary<string, object>();
        foreach (var item in data)
        {
            dataDict.Add(item.Key, item.Value);
        }

        var finalDic = new Dictionary<string, object>();
        finalDic.Add("interface", "RestAPI");
        finalDic.Add("method", methodName);
        finalDic.Add("parameters", dataDict);

        var passDataJsonString = JsonSerializer.Serialize(finalDic);
        var buffer = Encoding.UTF8.GetBytes(passDataJsonString);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpRequestMessage request = new(HttpMethod.Post, baseUri + "Wms/PostObject");

        request.Content = byteContent;

        var resultStream = await SendAsync(request, new CancellationToken());

        var result = (ApiResponse<T>)await resultStream.Content.ReadFromJsonAsync(typeof(ApiResponse<T>), options);

        return result;
    }

    public async Task<ApiResponse<T>> PostAsyncByOption<T>(string methodName
        , JsonSerializerOptions options
        , params KeyValuePair<string, object>[] data)
    {
        if (baseUri.HasNoValue())
        {
            SetUri();
        }

        var dataDict = new Dictionary<string, object>();
        foreach (var item in data)
        {
            dataDict.Add(item.Key, item.Value);
        }

        var finalDic = new Dictionary<string, object>();
        finalDic.Add("interface", "RestAPI");
        finalDic.Add("method", methodName);
        finalDic.Add("parameters", dataDict);

        var passDataJsonString = JsonSerializer.Serialize(finalDic);
        var buffer = Encoding.UTF8.GetBytes(passDataJsonString);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpRequestMessage request = new(HttpMethod.Post, baseUri + "Wms/PostObject");

        request.Content = byteContent;

        var resultStream = await SendAsync(request, new CancellationToken());

        var result = (ApiResponse<T>)await resultStream.Content.ReadFromJsonAsync(typeof(ApiResponse<T>), options);

        return result;
    }

    public async Task<ApiResponse<T>> PostFileAsync<T>(string uri, string filePath, params KeyValuePair<string, string>[] headers)
    {
        if (baseUri.HasNoValue())
        {
            SetUri();
        }

        byte[] file = File.ReadAllBytes(filePath);

        var byteContent = new ByteArrayContent(file);

        using MultipartFormDataContent multipartContent = new();

        multipartContent.Add(byteContent, "file", filePath.Split("\\").Last());

        foreach (KeyValuePair<string, string> header in headers)
        {
            multipartContent.Headers.Add(header.Key, new List<string>() { header.Value });
        }

        HttpRequestMessage request = new(HttpMethod.Post, baseUri + "Wms/" + uri);

        request.Content = multipartContent;

        var resultStream = await SendAsync(request, new CancellationToken());

        using Stream httpStream = await resultStream.Content.ReadAsStreamAsync();

        var result = await JsonSerializer.DeserializeAsync<ApiResponse<T>>(httpStream);

        return result;
    }

    public async Task<ApiResponse<T>> PostMultipartContentAsync<T>(string uri
        , MultipartFormDataContent formDataContent)
    {
        if (baseUri.HasNoValue())
        {
            SetUri();
        }

        HttpRequestMessage request = new(HttpMethod.Post
            , baseUri + uri);

        request.Content = formDataContent;

        var resultStream = await SendAsync(request, new CancellationToken());

        Stream httpStream = await resultStream.Content.ReadAsStreamAsync();
        using StreamReader sr = new(httpStream);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var result = await JsonSerializer.DeserializeAsync<ApiResponse<T>>(httpStream, options);

        return result;
    }

    public async Task<ApiResponse<T>> SendAsyncObjectByUri<T>(HttpMethod method
    , string uri
    , object data = null
    , JsonSerializerContext context = null)
    {
        if (baseUri.HasNoValue())
        {
            SetUri();
        }

        JsonSerializerOptions option = new()
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new NullableDateTimeConverter()
            }
        };

        if (context is not null)
        {
            option.TypeInfoResolver = context;
        }

        var passDataJsonString = JsonSerializer.Serialize(data);
        var buffer = Encoding.UTF8.GetBytes(passDataJsonString);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpRequestMessage request = new(method, baseUri + uri);

        request.Content = byteContent;

        var resultStream = await SendAsync(request, new CancellationToken());

        Stream httpStream = await resultStream.Content.ReadAsStreamAsync();

        using StreamReader sr = new(httpStream);

        var result = await JsonSerializer.DeserializeAsync<ApiResponse<T>>(httpStream, option);

        return result;
    }

    #endregion

    private void SetUri()
    {
        var uri = Configuration.GetSection("RfidConnectApi").GetSection("Uri");

        if (uri.Value is null)
        {
            baseUri = $"http://{Configuration.GetSection("RfidConnectApi")["Ip"]}/RfidCore/v2/";
        }
        else
        {
            baseUri = $"http://{Configuration.GetSection("RfidConnectApi")["Ip"]}{uri.Value}";
        }
    }
}

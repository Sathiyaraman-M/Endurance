using Microsoft.JSInterop;
using Quark.Core.Interfaces.Services.Storage.Provider;

namespace Quark.Infrastructure.Services.Storage.Provider;

public class ServerStorageProvider : IStorageProvider
{
    //TODO - replace on implementation (added for tests)
    //private Dictionary<string, string> _storage = new();

    private readonly IJSRuntime _jSRuntime;
    private readonly IJSInProcessRuntime _jSInProcessRuntime;
    private readonly ICurrentUserService _currentUserService;

    public ServerStorageProvider(ICurrentUserService currentUserService, IJSRuntime jSRuntime)
    {
        _currentUserService = currentUserService;
        _jSRuntime = jSRuntime;
        _jSInProcessRuntime = jSRuntime as IJSInProcessRuntime;
    }

    public ValueTask ClearAsync()
        => _jSRuntime.InvokeVoidAsync("localStorage.clear");

    public ValueTask<string> GetItemAsync(string key)
    {
        ////TODO - replace on implementation (added for tests)--
        //if (_storage.ContainsKey(key))
        //    return ValueTask.FromResult(_storage[key]);

        //return ValueTask.FromResult(string.Empty);
        ////----------------------------------------------------

        //throw new NotImplementedException();
        return _jSRuntime.InvokeAsync<string>("localStorage.getItem", key);
    }

    public ValueTask<string> KeyAsync(int index)
        => _jSRuntime.InvokeAsync<string>("localStorage.key", index);

    public ValueTask<bool> ContainKeyAsync(string key)
        => _jSRuntime.InvokeAsync<bool>("localStorage.hasOwnProperty", key);

    public ValueTask<int> LengthAsync()
        => _jSRuntime.InvokeAsync<int>("eval", "localStorage.length");

    public ValueTask RemoveItemAsync(string key)
        => _jSRuntime.InvokeVoidAsync("localStorage.removeItem", key);

    public ValueTask SetItemAsync(string key, string data) =>
        ////TODO - replace on implementation (added for tests)--
        //if (_storage.ContainsKey(key))
        //{
        //    _storage[key] = data;
        //}
        //else
        //{
        //    _storage.Add(key, data);
        //}

        //return ValueTask.CompletedTask;
        ////----------------------------------------------------

        //throw new NotImplementedException();
        _jSRuntime.InvokeVoidAsync("localStorage.setItem", key, data);


    public void Clear()
    {
        CheckForInProcessRuntime();
        _jSInProcessRuntime.InvokeVoid("localStorage.clear");
        //throw new NotImplementedException();
    }

    public string GetItem(string key)
    {
        CheckForInProcessRuntime();
        return _jSInProcessRuntime.Invoke<string>("localStorage.getItem", key);
        //throw new NotImplementedException();
    }

    public string Key(int index)
    {
        CheckForInProcessRuntime();
        return _jSInProcessRuntime.Invoke<string>("localStorage.key", index);
        //throw new NotImplementedException();
    }

    public bool ContainKey(string key)
    {
        CheckForInProcessRuntime();
        return _jSInProcessRuntime.Invoke<bool>("localStorage.hasOwnProperty", key);
        //throw new NotImplementedException();
    }

    public int Length()
    {
        CheckForInProcessRuntime();
        return _jSInProcessRuntime.Invoke<int>("eval", "localStorage.length");
        //throw new NotImplementedException();
    }

    public void RemoveItem(string key)
    {
        CheckForInProcessRuntime();
        _jSInProcessRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        //throw new NotImplementedException();
    }

    public void SetItem(string key, string data)
    {
        CheckForInProcessRuntime();
        _jSInProcessRuntime.InvokeVoid("localStorage.setItem", key, data);
        //throw new NotImplementedException();
    }

    private void CheckForInProcessRuntime()
    {
        if (_jSInProcessRuntime == null)
            throw new InvalidOperationException("IJSInProcessRuntime not available");
        //throw new NotImplementedException();
    }
}

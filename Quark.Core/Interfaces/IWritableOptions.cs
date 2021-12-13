using Microsoft.Extensions.Options;

namespace Quark.Core.Interfaces;

public interface IWritableOptions<out T> : IOptions<T> where T : class, new()
{
    void Update(Action<T> applyChanges);
}
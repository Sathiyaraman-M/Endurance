using System.Diagnostics.CodeAnalysis;

namespace Quark.Core.Interfaces.Services.Storage;

[ExcludeFromCodeCoverage]
public class ChangingEventArgs : ChangedEventArgs
{
    public bool Cancel { get; set; }
}
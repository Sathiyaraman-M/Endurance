using Quark.Core.Requests;

namespace Quark.Core.Interfaces.Services;

public interface IUploadService
{
    string UploadAsync(UploadRequest model);
}
using System.ComponentModel;

namespace Quark.Core.Domain.Enums;

public enum UploadType
{
    [Description(@"Images\Assets")]
    Product,

    [Description(@"Images\ProfilePictures")]
    ProfilePicture,

    [Description(@"Documents")]
    Document
}
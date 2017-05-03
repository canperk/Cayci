using System;

namespace Basbakanlik.Strateji.Entities
{
    public class ApiRoute : Attribute
    {
        public string Path { get; set; }
        public MethodType Type { get; set; }
    }

    public enum MethodType
    {
        NotSet = 0,
        Post = 1,
        Get = 2,
        Put = 3,
        Delete = 4
    }
}

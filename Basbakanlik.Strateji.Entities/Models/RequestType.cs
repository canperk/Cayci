using System.Collections.Generic;

namespace Basbakanlik.Strateji.Entities.Models
{
    public class RequestType : ModelBase
    {
        public RequestType()
        {
            Options = new List<string>();
        }
        public string Name { get; set; }
        public string ColorClass { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public int ListOrder { get; set; }
        public List<string> Options{ get; set; }
    }
}

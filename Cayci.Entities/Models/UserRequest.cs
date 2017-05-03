using System.Collections.Generic;

namespace Cayci.Entities.Models
{
    public class UserRequest : ModelBase
    {
        public UserRequest()
        {
            Details = new List<RequestDetail>();
        }
        public bool Checked { get; set; }
        public bool Seen { get; set; }
        public string Notes { get; set; }
        public string UserId { get; set; }
        public string GroupId { get; set; }
        public List<RequestDetail> Details { get; set; }
    }

    public class RequestDetail
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}

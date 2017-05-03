namespace Basbakanlik.Strateji.Entities.Models
{
    public class User : ModelBase
    {
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        public bool IsOnDuty { get; set; }
    }
}

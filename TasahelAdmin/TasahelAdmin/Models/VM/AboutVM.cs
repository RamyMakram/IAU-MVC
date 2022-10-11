namespace TasahelAdmin.Models.VM
{
    public class AboutVM
    {
        public int Id { get; set; }
        public int DomainId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescAr { get; set; }
        public string DescEn { get; set; }
        public bool? Enabled { get; set; }
        public string domainname { get; set; }
        public int domainid { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasahelAdmin.Models.VM
{
    public class DomainCreateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain1 { get; set; }
        public string ConnectionString { get; set; }
        public bool Enabled { get; set; }
        public DateTime EndDate { get; set; }



        [NotMapped]
        public IFormFile NewReqICo { get; set; }
        public string Img_NewReqICo { get; set; }

        [NotMapped]
        public IFormFile FollowIco { get; set; }
        public string Img_FollowIco { get; set; }
        public bool EnableAcadmic { get; set; }



        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Address { get; set; }


        [NotMapped]
        public IFormFile Favicon { get; set; }
        public string Img_FavIco { get; set; }
        [NotMapped]
        public IFormFile Icon { get; set; }
        public string Img_Ico { get; set; }

        public string Title { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKeyword { get; set; }
        public string Maincolor { get; set; }
        public string Secondcolor { get; set; }
        public string Thirdcolor { get; set; }


        public string SMTPName { get; set; }
        public string Smtp { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MessageAppSid { get; set; }
        public string Sender { get; set; }
        public bool UseMessages { get; set; }

        public virtual ICollection<About> Abouts { get; set; }
        public virtual ICollection<SubDomain> SubDomains { get; set; }
    }
}

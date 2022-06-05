using IAUBackEnd.Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Threading;
using System.Net.Mail;
using IAUAdmin.DTO.Helper;
using System.Threading.Tasks;

namespace IAUBackEnd.Admin.Controllers
{
    public class SocketController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Send(string name)
        {
            MostafidDBEntities p = new MostafidDBEntities();
            var data_Mos = p.Request_Data.Include(q => q.Request_File).Include(q => q.RequestTransaction.Select(s => s.Units1)).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Request_Type).Where(q => q.RequestTransaction.Count() > 1);

            foreach (var data in data_Mos)
            {
                string td_data = "";
                var req_trans = data.RequestTransaction.Where(s => s.CommentDate.HasValue).OrderByDescending(q => q.CommentDate);
                foreach (var i in req_trans)
                {
                    td_data += $@"
                                <tr>
                                    <td><p>{i.Units1.Units_Name_AR}<p/> </br> <p>{i.Units1.Units_Name_EN}<p/> </td>
                                    <td>{i.Comment}</td>
                                    <td>{i.CommentDate?.ToString("dd-MM-yyyy HH:mm") ?? ""}</td>
                                <tr>";
                }
                string tableStyle = @"
                            <style>
                            </style>";
                string message = $@"
                                {tableStyle}
                                    <table dir='rtl' border='1' cellpadding='1' cellspacing='1' width='100%'>
                                        <thead>
                                            <tr>
                                                <th><p> اسم الفئة الإدارية </p><p> Unit Name</p></th>
                                                <th><p>التعليق </p><p> Comment</p></th>
                                                <th><p>التاريخ </p><p> Date</p></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {td_data}
                                        </tbody>
                                    <table>";

                _ = NotifyUser(data.Personel_Data.Mobile, name, $@"عزيزي المستفيد، تم الانتهاء من الطلب رقم '{data.Code_Generate}'." + (req_trans.Count() == 0 ? "" : message), $"Dear Mostafid, Request number '{data.Code_Generate}' has been completed");
            }
            return Ok();
        }
        [HttpGet]
        [Route("api/Socket/NotifyUser")]
        public async Task<IHttpActionResult> NotifyUser(string Mobile, string Email, string message_ar, string message_en)
        {
            if (WebApiApplication.Setting_UseMessage)
            {
                HttpClient h = new HttpClient();

                string url = $"http://basic.unifonic.com/wrapper/sendSMS.php?appsid=su7G9tOZc6U0kPVnoeiJGHUDMKe8tp&msg={message_ar}&to={Mobile}&sender=IAU-BSC&baseEncode=False&encoding=UCS2";
                h.BaseAddress = new Uri(url);

                var res = h.GetAsync("").Result.Content.ReadAsStringAsync().Result;
            }
            var message = $@"
					<p dir='ltr'>{message_en}</p>
					<p dir='rtl'>{message_ar}</p>
					";
            //SmtpClient smtpClient = new SmtpClient("mail.iau.edu.sa", 25);

            //smtpClient.Credentials = new System.Net.NetworkCredential("noreply.bsc@iau.edu.sa", "Bsc@33322");
            //// smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
            //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            ////smtpClient.EnableSsl = true;
            //MailMessage mail = new MailMessage();

            ////Setting From , To and CC
            //mail.From = new MailAddress("noreply.bsc@iau.edu.sa", "Mustafid");
            //mail.To.Add(new MailAddress(Email));
            //mail.Subject = "IAU Notify";
            //mail.Body = message;
            //mail.IsBodyHtml = true;
            //smtpClient.Send(mail);


            SmtpClient smtpClient = new SmtpClient("mail.iau-bsc.com", 25);

            smtpClient.Credentials = new System.Net.NetworkCredential("ramy@iau-bsc.com", "ENGGGGAAA1448847@");
            // smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress("ramy@iau-bsc.com", "Mustafid");
            mail.To.Add(new MailAddress(Email));
            mail.Subject = "IAU Notify";
            mail.Body = message;
            mail.IsBodyHtml = true;
            smtpClient.Send(mail);
            return Ok(new ResponseClass()
            {
                success = true
            });

        }
    }
}

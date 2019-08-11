using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Net.Mail;

namespace SenalesDelCaribe.Models
{
    public class CPhoto
    {
        public int IDPhoto { get; set; }

        public string Description { get; set; }
    }

    public class CContact
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string From { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string  Message { get; set; }
    }

    public class CEmail
    {
        public void Send(CContact contact)
        {
            MailMessage mail = new MailMessage(
                contact.From,
                ConfigurationManager.AppSettings[1],
                contact.Subject,
                contact.Message);

            new SmtpClient().Send(mail);
        }
    }
}
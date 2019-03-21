using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ted_Wallick.Pages
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        public ContactFormModel Contact { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Please send a message to me";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var mailbody = $@"This is a new contact request from your website:<br><br>First Name: {Contact.Name} <br>LastName: {Contact.LastName} <br>Email: {Contact.Email} <br>Message:<br> ""{Contact.Message}""";
            

            if (!ModelState.IsValid)
            {
                

                return Page();
            }

            //Send the message
            SendMail(mailbody);

            return RedirectToPage("Index");
        }

        private void SendMail(string mailbody)
        {
            using (var message = new MailMessage(Contact.Email, "twallick.home@gmail.com"))
            {
                message.IsBodyHtml = true;
                message.To.Add(new MailAddress("twallick.home@gmail.com"));
                message.From = new MailAddress(Contact.Email);
                message.Subject = "New E-Mail from my website";
                message.Body = mailbody;
                using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new System.Net.NetworkCredential("twallick.home@gmail.com", "mubrhtzbkqvpaneo");
                    smtpClient.Send(message);
                }
            }
        }

       
    }

    public class ContactFormModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }
    }

}

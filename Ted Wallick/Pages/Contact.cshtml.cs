using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.Extensions.Options;


namespace Ted_Wallick.Pages
{
    public class ContactModel : PageModel
    {
      
        [BindProperty]
        public ContactFormModel Contact { get; set; }

        public string Message { get; set; }
        public IOptions<TwilloSettings> _TwilioSettings { get; }

        //Constructor
        public ContactModel(IOptions<TwilloSettings> twilioSettings)
        {
            _TwilioSettings = twilioSettings;
        }

        public void OnGet()
        {
            Message = "Please send a message to me";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string mailbody = $@"This is a new contact request from your website:<br><br>First Name: {Contact.Name} <br>LastName: {Contact.LastName} <br>Email: {Contact.Email} <br>Message:<br> ""{Contact.Message}""";


            if (!ModelState.IsValid)
            {


                return Page();
            }

            //Send the message
            SendMail(mailbody);

            // Find your Account Sid and Token at twilio.com/console

            SendSMS();

            return RedirectToPage("Index");
        }

        private void SendSMS()
        {
           

            string accountSid =_TwilioSettings.Value.TWILIO_ACCOUNT_SID;
            string authToken = _TwilioSettings.Value.TWILIO_AUTH_TOKEN;


            TwilioClient.Init(accountSid, authToken);

            string body = "This is a new contact request from your website:\n\n";
            body += "First Name: " + Contact.Name + "\n";
            body += "LastName: " + Contact.LastName + "\n";
            body += "Email: " + Contact.Email + "\n";
            body += "\nMessage:\n" + Contact.Message;

            MessageResource message = MessageResource.Create(
                body: body,
                from: new Twilio.Types.PhoneNumber("+13343848339"),
                to: new Twilio.Types.PhoneNumber("+15029658925")
            );

            string rtn = message.Sid;
        }

        private void SendMail(string mailbody)
        {
            using (MailMessage message = new MailMessage(Contact.Email, "twallick.home@gmail.com"))
            {
                message.IsBodyHtml = true;
                message.To.Add(new MailAddress("twallick.home@gmail.com"));
                message.From = new MailAddress(Contact.Email);
                message.Subject = "New E-Mail from my website";
                message.Body = mailbody;
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
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

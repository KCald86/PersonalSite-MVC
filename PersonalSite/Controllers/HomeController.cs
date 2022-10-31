using Microsoft.AspNetCore.Mvc;
using PersonalSite.Models;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;

namespace PersonalSite.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Links()
        {
            return View();
        }

        public IActionResult Portfolio()
        {
            return View();
        }

        public IActionResult Recognition()
        {
            return View();
        }

        public IActionResult Resume()
        {
            return View();
        }

        public IActionResult Welcome()
        {
            return View();
        }

        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            string message = $"You have recieved a new email from your site's contact form!<br />" +
                $"Senter: {cvm.Name}<br />Email: {cvm.Email}<br />Subject: {cvm.Subject}<br />" +
                $"Message: {cvm.Message}";

            var mm = new MimeMessage();

            mm.From.Add(new MailboxAddress("Sender", _config.GetValue<string>("Credentials:Email:User")));

            mm.To.Add(new MailboxAddress("Personal", _config.GetValue<string>("Credentials:Email:User")));

            mm.Subject = cvm.Subject;

            mm.Body = new TextPart("HTML") {Text = message};

            mm.Priority = MessagePriority.Urgent;

            mm.ReplyTo.Add(new MailboxAddress("User", cvm.Email));

            using (var client = new SmtpClient())
            {
                client.Connect(_config.GetValue<string>("Credentials:Email:Client"));

                client.Authenticate(

                    _config.GetValue<string>("Credentials:Email:User"),

                    _config.GetValue<string>("Credentials:Email:Password")

                    );

                try
                {
                    //try to send the email
                    client.Send(mm);
                }
                catch (Exception ex)
                {
                    //if there is an issue we can store an error message in a ViewBag varialbe
                    //to be displayed in the View
                    ViewBag.ErrorMessage = $"There was an error processing your request. Please " +
                        $"try again later.<br />Error Message: {ex.StackTrace}";

                    //Return the user to the View with their form info intact
                    return View(cvm);
                }
            }


            return View("EmailConfirmation", cvm);
        }

    }
}
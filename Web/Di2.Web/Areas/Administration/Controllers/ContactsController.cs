namespace Di2.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Di2.Common;
    using Di2.Data.Common.Repositories;
    using Di2.Data.Models;
    using Di2.Services.Messaging;
    using Di2.Web.ViewModels.Contacts;
    using Microsoft.AspNetCore.Mvc;

    public class ContactsController : AdministrationController
    {
        private readonly IRepository<ContactForm> contactsRepository;
        private readonly IEmailSender emailSender;

        public ContactsController(IRepository<ContactForm> contactsRepository, IEmailSender emailSender)
        {
            this.contactsRepository = contactsRepository;
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactsInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var contactForm = new ContactForm
            {
                Name = input.Name,
                Email = input.Email,
                Subject = input.Subject,
                Content = input.SanitizedContent,
                CreatedOn = DateTime.UtcNow,
            };

            await this.contactsRepository.AddAsync(contactForm);
            await this.contactsRepository.SaveChangesAsync();

            await this.emailSender.SendEmailAsync(
                input.Email,
                input.Name,
                GlobalConstants.SystemEmail,
                input.Subject,
                input.SanitizedContent);

            return this.RedirectToAction("ThankYou");
        }

        public IActionResult ThankYou()
        {
            return this.View();
        }
    }
}

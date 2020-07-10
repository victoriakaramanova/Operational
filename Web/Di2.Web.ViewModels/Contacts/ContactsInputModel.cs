using Di2.Data.Models;
using Di2.Services.Mapping;
using Di2.Web.Infrastructure;
using Ganss.XSS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Di2.Web.ViewModels.Contacts
{
    public class ContactsInputModel : IMapTo<ContactForm>
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Моля въведете вашите имена")]
        [Display(Name = "Име")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Моля въведете вашият email адрес")]
        [EmailAddress(ErrorMessage = "Моля въведете валиден email адрес")]
        [Display(Name = "Имейл")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Моля въведете заглавие на съобщението")]
        [StringLength(100, ErrorMessage = "Темата трябва да е поне {2} и не повече от {1} символа.", MinimumLength = 5)]
        [Display(Name = "Тема")]
        public string Subject { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Моля въведете съдържание на съобщението")]
        [StringLength(10000, ErrorMessage = "Съобщението трябва да е поне {2} символа.", MinimumLength = 10)]
        [Display(Name = "Напишете съобщение")]
        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        // [GoogleReCaptchaValidation]
        // public string RecaptchaValue { get; set; }
    }
}

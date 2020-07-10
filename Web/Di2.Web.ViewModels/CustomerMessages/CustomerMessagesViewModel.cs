using Di2.Data.Models;
using Di2.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Di2.Web.ViewModels.CustomerMessages
{
    public class CustomerMessagesViewModel : IMapFrom<ContactForm>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}

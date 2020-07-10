using Di2.Data.Models;
using Di2.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Di2.Web.ViewModels.CustomerMessages
{
    public class CustomerMessagesComponentViewModel : IMapFrom<ContactForm>
    {
        public IEnumerable<CustomerMessagesViewModel> CustomerMessages { get; set; }
    }
}

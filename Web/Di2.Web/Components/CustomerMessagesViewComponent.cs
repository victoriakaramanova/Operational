namespace Di2.Web.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Di2.Data.Common.Repositories;
    using Di2.Data.Models;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.CustomerMessages;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent(Name ="CustomerMessages")]
    public class CustomerMessagesViewComponent : ViewComponent
    {
        private readonly IDeletableEntityRepository<ContactForm> contactsRepository;

        public CustomerMessagesViewComponent(IDeletableEntityRepository<ContactForm> contactsRepository)
        {
            this.contactsRepository = contactsRepository;
        }

        public IViewComponentResult Invoke()
        {
            var messages = this.contactsRepository.All()
                .OrderByDescending(x => x.CreatedOn).To<CustomerMessagesViewModel>().ToList();
            var viewModel = new CustomerMessagesComponentViewModel
            {
                CustomerMessages = messages,
            };

            return this.View(viewModel);
        }
    }
}

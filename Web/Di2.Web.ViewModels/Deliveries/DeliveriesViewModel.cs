namespace Di2.Web.ViewModels.Deliveries
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Di2.Data.Models;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.Deliveries;

    public class DeliveriesViewModel
    {
        public IEnumerable<DeliveryViewModel> Deliveries { get; set; }
    }
}

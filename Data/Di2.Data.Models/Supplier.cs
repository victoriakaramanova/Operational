namespace Di2.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Di2.Data.Common.Models;

    public class Supplier : BaseDeletableModel<int>
    {
        public Supplier()
        {
            this.PriceLists = new HashSet<PriceList>();
        }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<PriceList> PriceLists { get; }

    }
}

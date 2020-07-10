namespace Di2.Data.Models
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Di2.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.SubCategories = new HashSet<SubCategory>();
            this.Orders = new HashSet<Order>();
            //this.OrderSuppliers = new HashSet<OrderSupplier>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string NameEng { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }

        public virtual ICollection<Material> Materials { get; set; }

        public virtual ICollection<OrderSupplier> OrderSuppliers { get; set; }

        public ICollection<Delivery> Deliveries { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}

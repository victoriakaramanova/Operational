// ReSharper disable VirtualMemberCallInConstructor
namespace Di2.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Di2.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.SqlServer.Management.Smo;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Categories = new HashSet<Category>();
            this.SubCategories = new HashSet<SubCategory>();
            this.Materials = new HashSet<Material>();
            this.PriceLists = new HashSet<PriceList>();
            this.OrderSuppliers = new HashSet<OrderSupplier>();
            this.Deliveries = new HashSet<Delivery>();
            this.Orders = new HashSet<Order>();
        }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<Material> Materials { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }

        public virtual ICollection<PriceList> PriceLists { get; set; }

        public virtual ICollection<OrderSupplier> OrderSuppliers { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}

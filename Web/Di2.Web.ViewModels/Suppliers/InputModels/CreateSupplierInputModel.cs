namespace Di2.Web.ViewModels.Suppliers.InputModels
{
    using Di2.Data.Models;
    using Di2.Services.Mapping;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class CreateSupplierInputModel : IMapTo<Supplier>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}

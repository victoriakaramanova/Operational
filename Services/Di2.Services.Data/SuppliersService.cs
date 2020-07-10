namespace Di2.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Di2.Data.Common.Repositories;
    using Di2.Data.Models;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.Suppliers.ViewModels;
    using Microsoft.EntityFrameworkCore;

    public class SuppliersService : ISuppliersService
    {
        private readonly IDeletableEntityRepository<Supplier> supplierRepository;

        public SuppliersService(IDeletableEntityRepository<Supplier> supplierRepository)
        {
            this.supplierRepository = supplierRepository;
        }

        public async Task<int> AddAsync(string name, string address, string email, string phone, string userId)
        {
            var supplier = new Supplier
            {
                //Id = Guid.NewGuid().ToString(),
                Name = name,
                Address = address,
                Email = email,
                Phone = phone,
                UserId = userId,
            };

            await this.supplierRepository.AddAsync(supplier);
            await this.supplierRepository.SaveChangesAsync();
            return supplier.Id;
        }

        public IEnumerable<T> GetAllSuppliers<T>()
        {
            IQueryable<Supplier> query = this.supplierRepository
                    .All();
            return query.To<T>().ToList();
        }

        public SupplierViewModel GetById(int id)
        {
            var supplier = this.supplierRepository.All()
                .Where(x => x.Id == id)
                .To<SupplierViewModel>()
                .FirstOrDefault();
            return supplier;
        }

        public SupplierViewModel GetByName(string name)
        {
            var supplier = this.supplierRepository.All()
                .Where(x => x.Name == name)
                .To<SupplierViewModel>()
                .FirstOrDefault();
            return supplier;
        }

        public int GetCount()
        {
            return this.supplierRepository.All().Count();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var supplier = this.supplierRepository.All()
                    .FirstOrDefault(x => x.Id == id);
            this.supplierRepository.Delete(supplier);
            await this.supplierRepository.SaveChangesAsync();
            return supplier.Id;
        }
    }
}

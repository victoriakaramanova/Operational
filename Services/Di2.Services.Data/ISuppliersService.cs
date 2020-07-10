namespace Di2.Services.Data
{
    using Di2.Data.Models;
    using Di2.Web.ViewModels.Suppliers.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface ISuppliersService
    {
        Task<int> AddAsync(string name, string address, string email, string phone, string userId);

        IEnumerable<T> GetAllSuppliers<T>();

        SupplierViewModel GetById(int id);

        SupplierViewModel GetByName(string name);

        int GetCount();

        Task<int> DeleteAsync(int id);
    }
}

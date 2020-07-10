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
    using Di2.Web.ViewModels;
    using Di2.Web.ViewModels.Materials.ViewModels;
    using Di2.Web.ViewModels.PriceLists.InputModels;
    using Microsoft.EntityFrameworkCore;

    public class PriceListsService : IPriceListsService
    {
        private readonly IDeletableEntityRepository<PriceList> priceListsRepository;
        private readonly IDeletableEntityRepository<Material> materialsRepository;
        private readonly IDeletableEntityRepository<Supplier> suppliersRepository;

        public PriceListsService(
            IDeletableEntityRepository<PriceList> priceListsRepository,
            IDeletableEntityRepository<Material> materialsRepository,
            IDeletableEntityRepository<Supplier> suppliersRepository)
        {
            this.priceListsRepository = priceListsRepository;
            this.materialsRepository = materialsRepository;
            this.suppliersRepository = suppliersRepository;
        }

        public List<T> GetAllPriceLists<T>()
        {
            return this.priceListsRepository
            .All().OrderBy(x => x.Material.SubCategoryId)
            .ThenBy(x => x.Material.Name)
            .ThenBy(x => x.CheapRatio)
            .To<T>()
            .ToList();
        }

        public async Task CreateAsync(CreatePriceListInputModel input, string userId)
        {
            /*var mId = this.materialsRepository.All()
                .Where(x => x.Name == input.Material.Name)
                .Select(x => x.Id).FirstOrDefault();
            var sId = this.suppliersRepository.All()
                .Where(x => x.Name == input.Supplier.Name)
                .Select(x => x.Id).FirstOrDefault();*/
            var priceList = new PriceList
            {
                MaterialId = input.MaterialId,
                SupplierId = input.SupplierId,
                MinimumQuantityPerOrder = input.MinimumQuantityPerOrder,
                UnitPrice = input.UnitPrice,
                UserId = userId,
                CheapRatio = input.CheapRatio,
            };

            await this.priceListsRepository.AddAsync(priceList);
            await this.priceListsRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllMaterials<T>()
        {
            IQueryable<Material> query = this.materialsRepository
                .All();

            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetAllSuppliers<T>()
        {
            IQueryable<Supplier> query = this.suppliersRepository.All();
            return query.To<T>().ToList();
        }

        // WRONG!!!!!!!!!!!!!
        public T GetById<T>()
        {
            var priceList = this.priceListsRepository.All()
                //.LastOrDefault(x=>x.CreatedOn)
                .To<T>().LastOrDefault();
            return priceList;
        }

        public T GetByElements<T>(int materialId, int supplierId, double minQty, decimal unitPrice)
        {
            var priceList = this.priceListsRepository.All()
               .Where(x => x.MaterialId == materialId && x.SupplierId == supplierId && x.MinimumQuantityPerOrder == minQty && x.UnitPrice == unitPrice)
               .To<T>().FirstOrDefault();
            return priceList;
        }

        public int GetCount()
        {
            return this.priceListsRepository.All().Count();
        }

        public async Task DeleteAsync(int materialId, int supplierId, double mqo, decimal unitPrice)
        {
            var minQty = this.priceListsRepository.All()
                    .Where(x => x.Material.Id == materialId)
                    .FirstOrDefault(x => x.Supplier.Id == supplierId).MinimumQuantityPerOrder;
            var unPrice = this.priceListsRepository.All()
                    .Where(x => x.Material.Id == materialId)
                    .FirstOrDefault(x => x.Supplier.Id == supplierId).UnitPrice;
            var plToDelete = this.priceListsRepository.All()
                    .Where(x => x.Material.Id == materialId)
                    .FirstOrDefault(x => x.Supplier.Id == supplierId);
            if (plToDelete.MinimumQuantityPerOrder == minQty && plToDelete.UnitPrice == unitPrice)
            {
                this.priceListsRepository.Delete(plToDelete);
            }

            await this.priceListsRepository.SaveChangesAsync();
            //return subCategory.Id;
        }
    }
}

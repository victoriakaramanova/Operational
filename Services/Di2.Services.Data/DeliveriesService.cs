namespace Di2.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Di2.Common;
    using Di2.Data.Common.Repositories;
    using Di2.Data.Models;
    using Di2.Data.Models.Enums;
    using Di2.Services.Mapping;
    using Di2.Web.ViewModels.Categories.ViewModels;
    using Di2.Web.ViewModels.Deliveries;
    using Di2.Web.ViewModels.OrderSuppliers;
    using Di2.Web.ViewModels.PriceLists.InputModels;

    public class DeliveriesService : IDeliveriesService
    {
        private readonly IDeletableEntityRepository<Delivery> deliveriesRepository;
        private readonly IDeletableEntityRepository<OrderSupplier> orderSuppliersRepository;
        private readonly IDeletableEntityRepository<Material> materialsRepository;
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly IDeletableEntityRepository<SubCategory> subCategoriesRepository;

        public DeliveriesService(
            IDeletableEntityRepository<Delivery> deliveriesRepository,
            IDeletableEntityRepository<OrderSupplier> orderSuppliersRepository,
            IDeletableEntityRepository<Material> materialsRepository,
            IDeletableEntityRepository<Category> categoriesRepository,
            IDeletableEntityRepository<SubCategory> subCategoriesRepository)
        {
            this.deliveriesRepository = deliveriesRepository;
            this.orderSuppliersRepository = orderSuppliersRepository;
            this.materialsRepository = materialsRepository;
            this.categoriesRepository = categoriesRepository;
            this.subCategoriesRepository = subCategoriesRepository;
        }

        public async Task<int> Create(OrderSupplierViewModel input)
        {
            /*double stockQuantity = this.orderSuppliersRepository.All()
                .Where(x => x.Status == 0 && x.MaterialId == input.MaterialId)
                .Sum(x => x.Quantity);*/

            var materialCategoryId = this.materialsRepository.All()
                .Where(x => x.Id == input.MaterialId)
                .Select(x => x.CategoryId).FirstOrDefault();
            var material = this.materialsRepository.All()
                .Where(x => x.Id == input.MaterialId).FirstOrDefault();
            var delivery = new Delivery
            {
                OrderId = input.Id,
                MaterialId = input.MaterialId,
                Material = material,
                MaterialName = material.Name,
                Description = material.Description,
                ExtraInfo = material.ExtraInfo,
                Image = material.Image,
                CategoryId = materialCategoryId.Value,
                SubCategoryId = material.SubCategoryId.Value,
                RemainingQuantity = input.Quantity,
                Quantity = input.Quantity,
                SupplierUnitPrice = input.UnitPrice,
                Cost = input.TotalPrice,
                //QuantityOnStock = stockQuantity,
                UnitPrice = input.UnitPrice * (1 + (decimal)GlobalConstants.StandardMarkup),
            };

            await this.deliveriesRepository.AddAsync(delivery);
            await this.deliveriesRepository.SaveChangesAsync();

            return delivery.Id;
        }

        public IEnumerable<T> GetAllProducts<T>(int categoryId)
        {
            var categoryName = this.categoriesRepository.All()
                .FirstOrDefault(x => x.Id == categoryId).Name;
          //if(!categoryId.HasValue)
          //  {
          //      return null;
          //  }

            var query = from d in this.deliveriesRepository.All()
                        group d by new
                        {
                            d.MaterialId,
                            d.Material.Name,
                            d.Material.Description,
                            d.Material.ExtraInfo,
                            d.Material.Image,
                            d.CategoryId,
                            d.Material.SubCategoryId,
                        }
                        into m
                        select new CategoryProductsViewModel
                        {
                            MaterialId = m.Key.MaterialId,
                            MaterialName = m.Key.Name,
                            Description = m.Key.Description,
                            ExtraInfo = m.Key.ExtraInfo,
                            Image = m.Key.Image,
                            CategoryId = m.Key.CategoryId,
                            SubCategoryId = m.Key.SubCategoryId.Value,
                            SubCategoryName = this.subCategoriesRepository.All().FirstOrDefault(x => x.Id == m.Key.SubCategoryId).Name,
                            Quantity = m.Sum(x => x.Quantity),
                            //UnitPrice = m.Sum(x => x.UnitPrice) / (decimal)m.Sum(x => x.Quantity),
                            Cost = m.Sum(x => x.Cost),
                            AvgPrice = m.Sum(x => x.Cost) / (decimal)m.Sum(x=>x.Quantity),
                        };
            //.FirstOrDefault(x => x.CategoryId == categoryId)

            query = query.Where(x => x.CategoryId == categoryId);
            return query.To<T>().ToList();
        }


        public T GetByMaterialId<T>(int materialId)
        {
            var query = from d in this.deliveriesRepository.All()
                        group d by new
                        {
                            d.MaterialId,
                            d.Material.Name,
                            d.Material.Description,
                            d.Material.ExtraInfo,
                            d.Material.Image,
                            d.CategoryId,
                            d.Material.SubCategoryId,
                        }
                        into m
                        select new CategoryProductsViewModel //ProductViewModel
                        {
                            MaterialId = m.Key.MaterialId,
                            MaterialName = m.Key.Name,
                            Description = m.Key.Description,
                            ExtraInfo = m.Key.ExtraInfo,
                            Image = m.Key.Image,
                            CategoryId = m.Key.CategoryId,
                            SubCategoryId = m.Key.SubCategoryId.Value,
                            SubCategoryName = this.subCategoriesRepository.All().FirstOrDefault(x => x.Id == m.Key.SubCategoryId).Name,
                            Quantity = m.Sum(x => x.Quantity),
                            Cost = m.Sum(x => x.Cost),
                            AvgPrice = m.Sum(x => x.Cost) * (decimal)(1 + GlobalConstants.StandardMarkup) / (decimal)m.Sum(x => x.Quantity),
                        };
            query = query.Where(x => x.MaterialId == materialId);
            return query.To<T>().FirstOrDefault();
        }

        public bool GetDeliveredQuantityPerProduct(int materialId, double quantity)
        {
            var availableQuantity = this.deliveriesRepository.All()
                .Where(x => x.MaterialId == materialId)
                .Sum(x => x.Quantity);
            var enough = availableQuantity >= quantity ? true : false;
            return enough;
        }
    }
}

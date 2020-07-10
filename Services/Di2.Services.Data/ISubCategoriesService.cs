namespace Di2.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Di2.Data.Models;
    using Di2.Web.ViewModels.SubCategories.InputModels;
    using Di2.Web.ViewModels.SubCategories.ViewModels;

    public interface ISubCategoriesService
    {
        Task AddAsync(CreateSubCategoryInputModel input, string userId);

        IEnumerable<T> GetAllSubCategories<T>(int? count = null);

        IEnumerable<T> GetCategories<T>();

        T GetById<T>(int id);

        int GetCount();

        Task<int> DeleteAsync(int id);
    }
}

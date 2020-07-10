namespace Di2.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Di2.Web.ViewModels.Categories.InputModels;

    public interface ICategoriesService
    {
        Task AddAsync(CreateCategoryInputModel input, string userId);

        IEnumerable<T> GetAllCategories<T>();

        T GetByName<T>(string name);

        T GetByNameBg<T>(string name);

        T GetById<T>(int catId);
    }
}

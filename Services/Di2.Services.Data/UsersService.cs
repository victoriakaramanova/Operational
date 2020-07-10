namespace Di2.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Di2.Common;
    using Di2.Data.Common.Repositories;
    using Di2.Data.Models;
    using Di2.Services.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Di2.Web.ViewModels.Users;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.usersRepository = usersRepository;
            this.userManager = userManager;
        }

        public UserViewModel GetDetails(string id)
        {
            var query = this.usersRepository
                .All()
                .Where(x => x.Id == id);

            var viewModel = query.To<UserViewModel>().FirstOrDefault();
            viewModel.Role = this.userManager.GetRolesAsync(query.FirstOrDefault()).Result.FirstOrDefault() ?? GlobalConstants.UserRoleName;
            return viewModel;
        }
    }
}

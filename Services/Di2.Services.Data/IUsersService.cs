using Di2.Data.Models;
using Di2.Web.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Di2.Services.Data
{
    public interface IUsersService
    {
        UserViewModel GetDetails(string id);
    }
}

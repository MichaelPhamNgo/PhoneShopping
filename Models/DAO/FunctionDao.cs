using Models.EF;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class FunctionDao
    {
        public ShoppingDbContext db;
        public FunctionDao()
        {
            db = new ShoppingDbContext();
        }

        public List<FunctionModel> ListByRoleId(Guid RoleId)
        {
            return (from permission in db.Permissions
                    join function in db.Functions
                    on permission.FunctionId equals function.Id into grpjoin_per_func
                    from function in grpjoin_per_func.DefaultIfEmpty()
                    join role in db.Roles
                    on permission.RoleId equals role.Id into grpjoin_per_rol
                    from role in grpjoin_per_rol.DefaultIfEmpty()
                    select new FunctionModel
                    {
                        FunctionId = function.Id,
                        FunctionIconCss = function.IconCss,
                        FunctionName = function.Name,
                        FunctionParentId = function.ParentId,
                        FunctionSortOrder = function.SortOrder,
                        FunctionStatus = function.Status,
                        FunctionURL = function.URL,
                        RoleId = role.Id,
                        RoleName = role.Name,
                        RoleDescription = role.Description
                    }).Where(fun => fun.RoleId == RoleId).ToList();
        }
    }
}

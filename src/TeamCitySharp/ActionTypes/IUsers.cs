using System.Collections.Generic;
using TeamCitySharp.DomainEntities;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
    public interface IUsers : IUsersAsync
    {
        List<User> All();
        Users GetFields(string fields);
        User Details(string userName);
        List<Role> AllRolesByUserName(string userName);
        List<Group> AllGroupsByUserName(string userName);
        List<Group> AllUserGroups();
        List<User> AllUsersByUserGroup(string userGroupName);
        List<Role> AllUserRolesByUserGroup(string userGroupName);
        bool Create(string username, string name, string email, string password);
        bool AddPassword(string username, string password);
        bool IsAdministrator(string username);
    }

    public interface IUsersAsync
    {
        Task<List<User>> AllAsync();
        Task<User> DetailsAsync(string userName);
        Task<List<Role>> AllRolesByUserNameAsync(string userName);
        Task<List<Group>> AllGroupsByUserNameAsync(string userName);
        Task<List<Group>> AllUserGroupsAsync();
        Task<List<User>> AllUsersByUserGroupAsync(string userGroupName);
        Task<List<Role>> AllUserRolesByUserGroupAsync(string userGroupName);
        Task<bool> CreateAsync(string username, string name, string email, string password);
        Task<bool> AddPasswordAsync(string username, string password);
        Task<bool> IsAdministratorAsync(string username);
    }
}
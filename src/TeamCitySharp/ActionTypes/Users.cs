using System.Collections.Generic;
using System.Net;
using TeamCitySharp.Connection;
using TeamCitySharp.DomainEntities;
using System.Threading.Tasks;

namespace TeamCitySharp.ActionTypes
{
    public class Users : IUsers
    {
        private readonly ITeamCityCaller m_caller;
        private string m_fields;

        internal Users(ITeamCityCaller caller)
        {
            m_caller = caller;
        }

        public Users GetFields(string fields)
        {
            var newInstance = (Users)MemberwiseClone();
            newInstance.m_fields = fields;
            return newInstance;
        }

        public List<User> All()
        {
            var userWrapper = m_caller.Get<UserWrapper>("/users");

            return userWrapper.User;
        }

        public async Task<List<User>> AllAsync()
        {
            var userWrapper = await m_caller.GetAsync<UserWrapper>("/users");

            return userWrapper.User;
        }

        public List<Role> AllRolesByUserName(string userName)
        {
            var user =
                m_caller.GetFormat<User>("/users/username:{0}", userName);

            return user.Roles.Role;
        }

        public async Task<List<Role>> AllRolesByUserNameAsync(string userName)
        {
            var user = await m_caller.GetFormatAsync<User>("/users/username:{0}", userName);

            return user.Roles.Role;
        }

        public User Details(string userName)
        {
            var user = m_caller.GetFormat<User>("/users/username:{0}", userName);

            return user;
        }

        public async Task<User> DetailsAsync(string userName)
        {
            var user = await m_caller.GetFormatAsync<User>("/users/username:{0}", userName);

            return user;
        }

        public List<Group> AllGroupsByUserName(string userName)
        {
            var user =
                m_caller.GetFormat<User>("/users/username:{0}", userName);

            return user.Groups.Group;
        }

        public async Task<List<Group>> AllGroupsByUserNameAsync(string userName)
        {
            var user = await m_caller.GetFormatAsync<User>("/users/username:{0}", userName);

            return user.Groups.Group;
        }

        public List<Group> AllUserGroups()
        {
            var userGroupWrapper = m_caller.Get<UserGroupWrapper>("/userGroups");

            return userGroupWrapper.Group;
        }

        public async Task<List<Group>> AllUserGroupsAsync()
        {
            var userGroupWrapper = await m_caller.GetAsync<UserGroupWrapper>("/userGroups");

            return userGroupWrapper.Group;
        }

        public List<User> AllUsersByUserGroup(string userGroupName)
        {
            var group = m_caller.GetFormat<Group>("/userGroups/key:{0}", userGroupName);

            return group.Users.User;
        }

        public async Task<List<User>> AllUsersByUserGroupAsync(string userGroupName)
        {
            var group = await m_caller.GetFormatAsync<Group>("/userGroups/key:{0}", userGroupName);

            return group.Users.User;
        }

        public List<Role> AllUserRolesByUserGroup(string userGroupName)
        {
            var group = m_caller.GetFormat<Group>("/userGroups/key:{0}", userGroupName);

            return group.Roles.Role;
        }

        public async Task<List<Role>> AllUserRolesByUserGroupAsync(string userGroupName)
        {
            var group = await m_caller.GetFormatAsync<Group>("/userGroups/key:{0}", userGroupName);

            return group.Roles.Role;
        }

        public bool Create(string username, string name, string email, string password)
        {
            bool result = false;

            string data = $"<user name=\"{name}\" username=\"{username}\" email=\"{email}\" password=\"{password}\"/>";

            var createUserResponse = m_caller.Post(data, HttpContentTypes.ApplicationXml, "/users", string.Empty);

            // Workaround, Create POST request fails to deserialize password field. See http://youtrack.jetbrains.com/issue/TW-23200
            // Also this does not return an accurate representation of whether it has worked or not
            AddPassword(username, password);

            if (createUserResponse.StatusCode == HttpStatusCode.OK)
                result = true;

            return result;
        }

        public async Task<bool> CreateAsync(string username, string name, string email, string password)
        {
            bool result = false;

            string data = $"<user name=\"{name}\" username=\"{username}\" email=\"{email}\" password=\"{password}\"/>";

            var createUserResponse = await m_caller.PostAsync(data, HttpContentTypes.ApplicationXml, "/users", string.Empty);

            // Workaround, Create POST request fails to deserialize password field. See http://youtrack.jetbrains.com/issue/TW-23200
            // Also this does not return an accurate representation of whether it has worked or not
            await AddPasswordAsync(username, password);

            if (createUserResponse.StatusCode == HttpStatusCode.OK)
                result = true;

            return result;
        }

        public bool AddPassword(string username, string password)
        {
            bool result = false;

            var response = m_caller.Put(password, HttpContentTypes.TextPlain,
                $"/users/username:{username}/password", string.Empty);

            if (response.StatusCode == HttpStatusCode.OK)
                result = true;

            return result;
        }

        public async Task<bool> AddPasswordAsync(string username, string password)
        {
            bool result = false;

            var response = await m_caller.PutAsync(password, HttpContentTypes.TextPlain,
                $"/users/username:{username}/password", string.Empty);

            if (response.StatusCode == HttpStatusCode.OK)
                result = true;

            return result;
        }
        
        public bool IsAdministrator(string username)
        {
            var isAdministrator =
                m_caller.GetBoolean($"/users/username:{username}/roles/SYSTEM_ADMIN/g");
            return isAdministrator;
        }

        public async Task<bool> IsAdministratorAsync(string username)
        {
            var isAdministrator =
                await m_caller.GetBooleanAsync($"/users/username:{username}/roles/SYSTEM_ADMIN/g");
            return isAdministrator;
        }
    }
}
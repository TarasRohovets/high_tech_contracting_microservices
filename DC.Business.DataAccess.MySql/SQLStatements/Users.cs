using System;
using System.Reflection;
using DC.Core.Infrastructure.Resources;

namespace DC.Business.DataAccess.MySql.SQLStatements
{
    internal sealed class Users
    {
        private const string ResourceNamespace = "DC.Business.DataAccess.MySql.SQLStatements.Organization.Users";
        private static readonly Assembly ResourceAssembly = typeof(Users).Assembly;

        // SELECT
        internal static string GetUserByKey => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("GetUserByKey.sql");
        internal static string GetUserById => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("GetUserById.sql");
        internal static string GetUserByEmail => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("GetUserByEmail.sql");
        internal static string GetUserByEmailAndPasswords => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("GetUserByEmailAndPasswords.sql");
        internal static string SearchUsers => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("SearchUsers.sql");

        // INSERT
        internal static string InsertUser => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("InsertUser.sql");

        // UPDATE
        internal static string UpdateUser => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("UpdateUser.sql");
        internal static string UpdateUserProfilePhoto => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("UpdateUserProfilePhoto.sql");

        // DELETE
        
        internal static string DeleteUserProfilePhoto => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("DeleteUserProfilePhoto.sql");



    }
}

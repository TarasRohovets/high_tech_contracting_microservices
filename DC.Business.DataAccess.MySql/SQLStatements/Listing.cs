using System;
using System.Reflection;
using DC.Core.Infrastructure.Resources;
namespace DC.Business.DataAccess.MySql.SQLStatements
{
    internal sealed class Listing
    {
        private const string ResourceNamespace = "DC.Business.DataAccess.MySql.SQLStatements.Organization.Listing"; 
        private static readonly Assembly ResourceAssembly = typeof(Users).Assembly;

        // SELECT 
        internal static string GetOperationTypes => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("GetOperationTypes.sql");
        internal static string GetPropertyTypes => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("GetPropertyTypes.sql");
        internal static string GetPropertiesByUserBasic => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("GetPropertiesByUserBasic.sql");
        internal static string GetPropertyByUserId => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("GetPropertyByUserId.sql");
        internal static string GetPropertyById => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("GetPropertyById.sql");
        internal static string GetPropertyByEmail => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("GetPropertyByEmail.sql");
        internal static string SearchPropertiesForAdmin => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("SearchPropertiesForAdmin.sql");
        internal static string CountPropertiesForAdmin => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("CountPropertiesForAdmin.sql");
        
        // INSERT
        internal static string InsertProperty => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("InsertProperty.sql");

        // DELETE
        internal static string DeletePropertyByUserId => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("DeletePropertyByUserId.sql");

        // UPDATE
        internal static string ApprovePropertyForAdminService => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("ApprovePropertyForAdminService.sql");
        internal static string BlockPropertyByAdminService => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("BlockPropertyByAdminService.sql");
    }
}

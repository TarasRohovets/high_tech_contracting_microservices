using System;
using System.Reflection;
using DC.Core.Infrastructure.Resources;

namespace DC.Core.DataAccess.MySql.SQLStatements
{
    public class Common
    {
        private const string ResourceNamespace = "DC.Core.DataAccess.MySql.SQLStatements.Common";
        private static readonly Assembly ResourceAssembly = typeof(Common).Assembly;

        internal static string QueryPaginator => new ResourceManager(ResourceNamespace, ResourceAssembly).GetString("QueryPaginator.sql");
    }
}


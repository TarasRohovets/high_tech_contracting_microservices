using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Business.Application.Contracts.Dtos.Organization.Listing.Admin;
using DC.Business.Domain.Entities.Organization;
using DC.Business.Domain.Repositories.Organization;
using DC.Core.Contracts.Application.Pipeline.Dtos.Input;
using DC.Core.DataAccess.MySql;
using Microsoft.Extensions.Configuration;
using DC.Business.DataAccess.MySql.Extensions;

namespace DC.Business.DataAccess.MySql.Repositories.Organization
{
    public class ListingRepository : BusinessRepository, IListingRepository
    {
        public ListingRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<ulong> ListProperty(Property property)
        {
            using(IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                DynamicParameters propertyParameters = new DynamicParameters(property);

                SQLBuilder insertPropertyStmt = new SQLBuilder(SQLStatements.Listing.InsertProperty);
                ulong insertedId = (ulong)await connection.ExecuteScalarAsync(insertPropertyStmt.ToStatement(), propertyParameters).ConfigureAwait(false);
                return insertedId;
            }
        }

        public async Task<IEnumerable<Property>> GetPropertiesByUserBasicService(long userId)
        {
            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                SQLBuilder getStmt = new SQLBuilder(SQLStatements.Listing.GetPropertiesByUserBasic);
                var result = await connection.QueryAsync<Property>(getStmt.ToStatement(), new { userId = userId }).ConfigureAwait(false);
                return result?.ToList();
            }
        }

        public async Task<Property> GetPropertyById(long propertyId)
        {
            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                SQLBuilder getStmt = new SQLBuilder(SQLStatements.Listing.GetPropertyById);
                var result = await connection.QueryAsync<Property>(getStmt.ToStatement(), new { id = propertyId }).ConfigureAwait(false);
                return result?.SingleOrDefault();
            }
        }

        public async Task<Property> GetPropertyByEmail(string email)
        {
            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                SQLBuilder getStmt = new SQLBuilder(SQLStatements.Listing.GetPropertyByEmail);
                var result = await connection.QueryAsync<Property>(getStmt.ToStatement(), new { email = email }).ConfigureAwait(false);
                return result?.SingleOrDefault();
            }
        }

        public async Task<Property> GetPropertyByUser(PropertyByIdUserIdDto input)
        {
            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                SQLBuilder getStmt = new SQLBuilder(SQLStatements.Listing.GetPropertyByUserId);
                var result = await connection.QueryAsync<Property>(getStmt.ToStatement(), new { id = input.id, userId = input.userId }).ConfigureAwait(false);
                return result?.SingleOrDefault();
            }
        }

       public async Task DeletePorpertyByUser(PropertyByIdUserIdDto input)
        {
            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                SQLBuilder deleteStmt = new SQLBuilder(SQLStatements.Listing.DeletePropertyByUserId);
                await connection.ExecuteAsync(deleteStmt.ToStatement(), new { id = input.id, userId = input.userId }).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<OperationType>> GetOperationTypes()
        {
            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                SQLBuilder getOperationTypesStmt = new SQLBuilder(SQLStatements.Listing.GetOperationTypes);
                var result = await connection.QueryAsync<OperationType>(getOperationTypesStmt.ToStatement()).ConfigureAwait(false);
                return result?.ToList();
            }
        }

        public async Task<IEnumerable<PropertyType>> GetPropertyTypes()
        {
            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                SQLBuilder getPropertyTypesStmt = new SQLBuilder(SQLStatements.Listing.GetPropertyTypes);
                var result = await connection.QueryAsync<PropertyType>(getPropertyTypesStmt.ToStatement()).ConfigureAwait(false);
                return result?.ToList();
            }
        }

        #region Admin
        public async Task<IEnumerable<Property>> SearchPropertiesForAdmin(SearchPaginationRequestDto<SearchPropertyForAdminRequestDto> inputDto)
        {
            SQLBuilder query = new SQLBuilder(SQLStatements.Listing.SearchPropertiesForAdmin);

            if (string.IsNullOrEmpty(inputDto.OrderBy))
                query.AddOrderBy(nameof(Property.Id), inputDto.OrderDescending);
            else
                query.AddOrderBy(inputDto.OrderBy, inputDto.OrderDescending);

            IEnumerable<string> filterParameters = inputDto.RestrictionCriteria?.GetFilterParameters();
            query.KeepParameters(filterParameters);

            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                // var lookup = new Dictionary<long, User>();
                var limit = (inputDto.PageNumber - 1) * inputDto.RowsPerPage;

                var result = await connection.QueryAsync<Property>(query.ToPaginated(), new
                {
                    Type = inputDto.RestrictionCriteria.Type,
                    LimitPagination = limit,
                    RowsPerPage = inputDto.RowsPerPage,
                }).ConfigureAwait(false);
                return result.ToList();
            }
        }

        public async Task<int> CountPropertiesForAdmin(SearchPaginationRequestDto<SearchPropertyForAdminRequestDto> inputDto)
        {
            SQLBuilder query = new SQLBuilder(SQLStatements.Listing.CountPropertiesForAdmin);

            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                var result = await connection.QueryFirstAsync<int>(query.ToStatement(), new
                {
                    Type = inputDto.RestrictionCriteria.Type
                }).ConfigureAwait(false);
                return result;
            }
        }

        public async Task ApprovePropertyForAdminService(int mySqlId)
        {
            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                SQLBuilder statement = new SQLBuilder(SQLStatements.Listing.ApprovePropertyForAdminService);
                await connection.ExecuteAsync(statement.ToStatement(), new { Id = mySqlId }).ConfigureAwait(false);
            }
        }

        public async Task BlockPropertyByAdminService(int mySqlId)
        {
            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                SQLBuilder statement = new SQLBuilder(SQLStatements.Listing.BlockPropertyByAdminService);
                await connection.ExecuteAsync(statement.ToStatement(), new { Id = mySqlId }).ConfigureAwait(false);
            }
        }
        #endregion

    }
}

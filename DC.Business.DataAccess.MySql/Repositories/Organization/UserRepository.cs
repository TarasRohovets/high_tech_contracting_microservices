using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using DC.Business.Domain.Entities.Organization;
using DC.Business.Domain.Repositories.Organization;
using DC.Core.DataAccess.MySql;
using Microsoft.Extensions.Configuration;
using DC.Business.DataAccess.MySql.Extensions;
using System.Linq;
using DC.Core.Contracts.Application.Pipeline.Dtos.Input;
using DC.Business.Domain.ViewModel;

namespace DC.Business.DataAccess.MySql.Repositories.Organization
{
    public class UserRepository : BusinessRepository, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<User> GetUserByEmailAndPasswordsAsync(string email, List<string> hashedPasswords)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new System.ArgumentException(nameof(email));

            if (hashedPasswords == null)
                return null;

            SQLBuilder query = new SQLBuilder(SQLStatements.Users.GetUserByEmailAndPasswords);
            query.KeepParameters("Email", "HashedPasswords");

            User user = null;

            using(IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                user = await connection.QuerySingleOrDefaultAsync<User>(query.ToStatement(), new
                {
                   Email = email,
                   hashedPasswords = hashedPasswords
                }).ConfigureAwait(false);
            }

            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new System.ArgumentException(nameof(username));

            SQLBuilder query = new SQLBuilder(SQLStatements.Users.GetUserByKey);
            query.KeepParameters("UserName");

            User user = null;

            using( IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                user = await connection.QuerySingleOrDefaultAsync<User>(query.ToStatement(), new { username }).ConfigureAwait(false);
            }
            return user;
        }

        public async Task<User> GetUserByIdAsync(long userId)
        {
            SQLBuilder query = new SQLBuilder(SQLStatements.Users.GetUserById);
            query.KeepParameters("Id");

            User user = null;

            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                var result = await connection.QueryAsync<User>(query.ToStatement(), new { id = userId });
                user = result.SingleOrDefault();
                
            }

            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            SQLBuilder query = new SQLBuilder(SQLStatements.Users.GetUserByEmail);
            query.KeepParameters("Email");

            User user = null;

            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                var result =  await connection.QueryAsync<User>(query.ToStatement(), new { Email = email }).ConfigureAwait(false);
                user = result.SingleOrDefault();
            }

            return user;
        }

        public async Task<long> CreateUserAsync(User newUser, string hashedPassword)
        {
            long newUserId = 0;

            using(var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                // Insert user record
                DynamicParameters userParameters = new DynamicParameters(newUser);
                userParameters.Add(name: nameof(hashedPassword), value: hashedPassword, dbType: DbType.String, direction: ParameterDirection.Input);

                SQLBuilder insertUserStmt = new SQLBuilder(SQLStatements.Users.InsertUser);

                newUserId = await connection.ExecuteInsertGetIdAsync(insertUserStmt, userParameters).ConfigureAwait(false);

                // Insert Roles
                //if (newUser.Roles != default)
                //    foreach (Role item in newUser.Roles)
                //    {
                //        SQLBuilder insertRoleStmt = new SQLBuilder(SQLStatements.Users.InsertUserRoles);
                //        var parameters = new { roleId = item.Id, userId = newUserId };
                //        if ((await connection.ExecuteAsync(insertRoleStmt.ToStatement(), parameters).ConfigureAwait(false)) != 1)
                //            throw new UnexpectedResultFromSqlStatementException(nameof(SQLStatements.Users.InsertUserRoles));
                //    }

                transactionScope.Complete();
            }
            return newUserId;
        }

        public async Task UpdateUserProfilePhoto(long userId, string imageName, string imagePath)
        {
            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add(name: "Id", value: userId, dbType: DbType.Int64, direction: ParameterDirection.Input);
                parameters.Add(name: "ImageName", value: imageName, dbType: DbType.String, direction: ParameterDirection.Input);
                parameters.Add(name: "ImagePath", value: imagePath, dbType: DbType.String, direction: ParameterDirection.Input);

                SQLBuilder updateUserProfilePhotoStmt = new SQLBuilder(SQLStatements.Users.UpdateUserProfilePhoto);
                await connection.ExecuteAsync(updateUserProfilePhotoStmt.ToStatement(), parameters).ConfigureAwait(false);
            }
        }

        public async Task DeleteUserProfilePhoto(long userId)
        {
            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add(name: "Id", value: userId, dbType: DbType.Int64, direction: ParameterDirection.Input);

                SQLBuilder deleteUserProfilePhotoStmt = new SQLBuilder(SQLStatements.Users.DeleteUserProfilePhoto);
                await connection.ExecuteAsync(deleteUserProfilePhotoStmt.ToStatement(), parameters).ConfigureAwait(false);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            // using(var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using(IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                DynamicParameters userParamenters = new DynamicParameters(user);

                SQLBuilder updateUserStmt = new SQLBuilder(SQLStatements.Users.UpdateUser);
                await connection.ExecuteAsync(updateUserStmt.ToStatement(), userParamenters).ConfigureAwait(false);

                ////Update Jobs
                //if (user.Jobs != default)
                //{
                //    // Delete
                //    SQLBuilder queryJobs = new SQLBuilder(SQLStatements.Users.GetJobs);
                //    var getJobsFromDb = (await connection.QueryAsync<Job>(queryJobs.ToStatement(), new { UserId = user.Id }).ConfigureAwait(false)).AsList();
                //    foreach (Job job in getJobsFromDb)
                //    {
                //        if (!user.Jobs.Any(x => x.Id == job.Id))
                //        {
                //            SQLBuilder deleteJobById = new SQLBuilder(SQLStatements.Users.DeleteJobById);
                //            await connection.QueryAsync<Job>(deleteJobById.ToStatement(), new { Id = job.Id, UserId = job.UserId }).ConfigureAwait(false);
                //        }
                //    };

                //    foreach (Job job in user.Jobs)
                //    {
                //        if (!string.IsNullOrEmpty(job.Position)) // Dont the validation here but no ideias how to put it in business
                //        { 
                //            SQLBuilder queryJobsById = new SQLBuilder(SQLStatements.Users.GetJobsById);
                //            var getJobFromDb = await connection.QueryAsync<Job>(queryJobsById.ToStatement(), new { Id = job.Id, UserId = job.UserId }).ConfigureAwait(false);
                //            var jobFromDb = getJobFromDb.SingleOrDefault();

                //            DynamicParameters jobParameters = new DynamicParameters(job);
                //            jobParameters.Add(name: "UserId", value: user.Id, dbType: DbType.String, direction: ParameterDirection.Input);
                //            if (jobFromDb != null)
                //            {
                //                SQLBuilder updateUserJobsStmt = new SQLBuilder(SQLStatements.Users.UpdateJobs);
                //                await connection.ExecuteAsync(updateUserJobsStmt.ToStatement(), jobParameters).ConfigureAwait(false);
                //            } else {
                //                SQLBuilder insertUserJobsStmt = new SQLBuilder(SQLStatements.Users.InsertJobs);
                //                await connection.ExecuteAsync(insertUserJobsStmt.ToStatement(), jobParameters).ConfigureAwait(false);
                //            }
                //        }
                //    }

                //}
                //if(user.Jobs?.Count() == 0)
                //{
                //    SQLBuilder deleteUserJobsStmt = new SQLBuilder(SQLStatements.Users.DeleteJobs);
                //    await connection.ExecuteAsync(deleteUserJobsStmt.ToStatement()).ConfigureAwait(false);
                //}
                // transactionScope.Complete();
            }
        }

        public async Task<IEnumerable<User>> SearchUsers(SearchPaginationRequestDto<UserSearchInput> inputDto)
        {
            SQLBuilder query = new SQLBuilder(SQLStatements.Users.SearchUsers);

            if (string.IsNullOrEmpty(inputDto.OrderBy))
                query.AddOrderBy(nameof(User.Name), inputDto.OrderDescending);
            else
                query.AddOrderBy(inputDto.OrderBy, inputDto.OrderDescending);

            IEnumerable<string> filterParameters = inputDto.RestrictionCriteria?.GetFilterParameters();
            query.KeepParameters(filterParameters);

            using (IDbConnection connection = BusinessDatabase.OpenConnection())
            {
                var lookup = new Dictionary<long, User>();
                var limit = (inputDto.PageNumber - 1) * inputDto.RowsPerPage;

                var result = await connection.QueryAsync<User>(query.ToPaginated(), new {
                                 Email = inputDto.RestrictionCriteria.Email,
                                 Name = inputDto.RestrictionCriteria.Name,
                                 LimitPagination = limit,
                                 RowsPerPage = inputDto.RowsPerPage,
                    }).ConfigureAwait(false);
                return result.ToList();
            }
        }
    }
}

//var lookup = new Dictionary<int, Course>();
//conn.Query<Course, Location, Course>(@"
//    SELECT c.*, l.*
//    FROM Course c
//    INNER JOIN Location l ON c.LocationId = l.Id                    
//    , (c, l) => {
//        Course course;
//        if (!lookup.TryGetValue(c.Id, out course))
//            lookup.Add(c.Id, course = c);
//        if (course.Locations == null) 
//            course.Locations = new List<Location>();
//        course.Locations.Add(l); /* Add locations to course */
//        return course;
//     }).AsQueryable();
//var resultList = lookup.Values;

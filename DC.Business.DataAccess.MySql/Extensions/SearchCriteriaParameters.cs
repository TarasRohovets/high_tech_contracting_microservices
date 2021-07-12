using System;
using System.Collections.Generic;
using DC.Business.Application.Contracts.Dtos.Enums;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Business.Application.Contracts.Dtos.Organization.Listing.Admin;
using DC.Business.Domain.ViewModel;
using DC.Core.Contracts.Application.Pipeline.Dtos.Input;

namespace DC.Business.DataAccess.MySql.Extensions
{
    public static class SearchCriteriaParameters
    {

        /** USERS **/
        public static IEnumerable<string> GetFilterParameters(this UserSearchInput searchCriteria)
        {
            if (!string.IsNullOrWhiteSpace(searchCriteria.Email))
                yield return nameof(searchCriteria.Email);
            if (!string.IsNullOrWhiteSpace(searchCriteria.Name))
                yield return nameof(searchCriteria.Name);
            if (!string.IsNullOrWhiteSpace(searchCriteria.Username))
                yield return nameof(searchCriteria.Username);
        }

        public static IEnumerable<string> GetFilterParameters(this SearchPropertyForAdminRequestDto searchCriteria)
        {
            if (searchCriteria.Type != PropertyStateDto.None)
                yield return nameof(searchCriteria.Type);
        }


        public static object ConvertToDBTypes(this SearchPaginationRequestDto<UserSearchInput> searchCriteria)
        {
            return new
            {
                Email = searchCriteria.RestrictionCriteria.Email,
                Name = searchCriteria.RestrictionCriteria.Name,
                Username = searchCriteria.RestrictionCriteria.Username,

                PageNumber = searchCriteria.PageNumber,
                RowsPerPage = searchCriteria.RowsPerPage,
            };
        }
    }
}

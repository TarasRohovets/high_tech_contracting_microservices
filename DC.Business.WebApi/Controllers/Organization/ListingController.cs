using System;
using System.Threading;
using System.Threading.Tasks;
using DC.Business.Application.Contracts.Dtos.Organization.Listing;
using DC.Business.Application.Contracts.Dtos.Organization.Listing.Admin;
using DC.Business.Application.Contracts.Interfaces.Organization.Listing;
using DC.Business.Application.Contracts.Interfaces.Organization.Listing.Admin;
using DC.Core.Contracts.Application.Pipeline.Dtos.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DC.Business.WebApi.Controllers.Organization
{
    [Route("api/organization/[controller]")]
    // [Produces("application/json")]
    [ApiController]
    // [Authorize]
    public class ListingController : ControllerBase
    {
        private readonly IListSellHouseService _listSellHouseService;
        private readonly IGetOperationTypesService _getOperationTypesService;
        private readonly IGetPropertyTypesService _getPropertyTypesService;
        private readonly IGetPropertiesByUserBasicService _getPropertiesByUserBasicService;
        private readonly IGetPropertyByUserService _getPropertyByUserService;
        private readonly IDeletePropertyByUserService _deletePropertyByUserService;
        private readonly ISearchPropertiesService _searchPropertiesService;
        private readonly IGetPropertyByMySqlIdService _getPropertyByMySqlId;
        private readonly ISearchPropertiestForAdminService _searchPropertiestForAdminService;
        private readonly IApprovePropertyForAdminService _approvePropertyForAdminService;
        private readonly IBlockPropertyByAdminService _blockPropertyByAdminService;

        public ListingController(IListSellHouseService listSellHouseService,
            IGetOperationTypesService getOperationTypesService,
            IGetPropertyTypesService getPropertyTypesService,
            IGetPropertiesByUserBasicService getPropertiesByUserBasicService,
            IGetPropertyByUserService getPropertyByUserService,
            IDeletePropertyByUserService deletePropertyByUserService,
            ISearchPropertiesService searchPropertiesService,
            IGetPropertyByMySqlIdService getPropertyByMySqlIdService,
            ISearchPropertiestForAdminService searchPropertiestForAdminService,
            IApprovePropertyForAdminService approvePropertyForAdminService,
            IBlockPropertyByAdminService blockPropertyByAdminService)
        {
            _listSellHouseService = listSellHouseService ?? throw new ArgumentNullException(nameof(listSellHouseService));
            _getOperationTypesService = getOperationTypesService ?? throw new ArgumentNullException(nameof(getOperationTypesService));
            _getPropertyTypesService = getPropertyTypesService ?? throw new ArgumentNullException(nameof(getPropertyTypesService));
            _getPropertiesByUserBasicService = getPropertiesByUserBasicService ?? throw new ArgumentNullException(nameof(getPropertiesByUserBasicService));
            _getPropertyByUserService = getPropertyByUserService ?? throw new ArgumentNullException(nameof(getPropertyByUserService));
            _deletePropertyByUserService = deletePropertyByUserService ?? throw new ArgumentNullException(nameof(deletePropertyByUserService));
            _searchPropertiesService = searchPropertiesService ?? throw new ArgumentNullException(nameof(searchPropertiesService));
            _getPropertyByMySqlId = getPropertyByMySqlIdService ?? throw new ArgumentNullException(nameof(getPropertyByMySqlIdService));
            _searchPropertiestForAdminService = searchPropertiestForAdminService ?? throw new ArgumentNullException(nameof(searchPropertiestForAdminService));
            _approvePropertyForAdminService = approvePropertyForAdminService ?? throw new ArgumentNullException(nameof(approvePropertyForAdminService));
            _blockPropertyByAdminService = blockPropertyByAdminService ?? throw new ArgumentNullException(nameof(blockPropertyByAdminService));
        }

        [HttpPost("listSellHouse")]
        public async Task<IActionResult> ListSellHouse([FromBody] SellHouseDto sellHouseDto, CancellationToken cancellationToken = default)
        {
            var result = await _listSellHouseService.ExecuteServiceAsync(sellHouseDto);
            return Ok(result);
        }

        [HttpGet("getPropertiesByUserBasic/{userId}")]
        public async Task<IActionResult> GetPropertiesByUserBasic(long userId, CancellationToken cancellationToken = default)
        {
            var result = await _getPropertiesByUserBasicService.ExecuteServiceAsync(userId);
            return Ok(result);
        }

        [HttpGet("getPropertyByUser/{userId}/{propertyId}")]
        public async Task<IActionResult> GetPropertyByUser(long userId, long propertyId, CancellationToken cancellationToken = default)
        {
            var input = new PropertyByIdUserIdDto()
            {
                id = propertyId,
                userId = userId
            };
            var result = await _getPropertyByUserService.ExecuteServiceAsync(input);
            return Ok(result);
        }

        [HttpGet("getProperyByDbId/{dbId}")]
        public async Task<IActionResult> GetPropertyByMySqlId(int dbId, CancellationToken cancellationToken)
        {
            var result = _getPropertyByMySqlId.ExecuteServiceAsync(dbId);
            return Ok(result);
        }

        [HttpPost("searchProperties")]
        public async Task<IActionResult> SearchPropertiesAsync([FromBody] SearchCriteriaDto searchCriteria, CancellationToken cancellationToken = default)
        {
            var result = _searchPropertiesService.ExecuteServiceAsync(searchCriteria);
            return Ok(result);
        }

        [HttpPost("deactivatePropertyByUser/{userId}/{propertyId}")]
        public async Task<IActionResult> DeactivatePropertyByUser(long userId, long propertyId, CancellationToken cancellationToken = default)
        {
            // TODO
            return Ok();
        }

        [HttpDelete("deletePropertyByUser/{userId}/{propertyId}")]
        public async Task<IActionResult> DeletePropertyByUser(long userId, long propertyId, CancellationToken cancellationToken = default)
        {
            var input = new PropertyByIdUserIdDto()
            {
                id = propertyId,
                userId = userId
            };
            var result = _deletePropertyByUserService.ExecuteServiceAsync(input);
            return Ok(result);
        }

        [HttpGet("operationTypes")]
        public async Task<IActionResult> GetOperationTypes(CancellationToken cancellationToken = default)
        {
            var result = await _getOperationTypesService.ExecuteServiceAsync("", cancellationToken);
            return Ok(result);
        }

        [HttpGet("propertyTypes")]
        public async Task<IActionResult> GetPropertyTypes(CancellationToken cancellationToken = default)
        {
            var result = await _getPropertyTypesService.ExecuteServiceAsync("", cancellationToken);
            return Ok(result);
        }

        #region Admin

        [HttpPost("admin/search")]
        public async Task<IActionResult> SearchPropertiesForAdmin([FromBody] SearchPaginationRequestDto<SearchPropertyForAdminRequestDto> inputDto, CancellationToken cancellationToken = default)
        {
            var result = _searchPropertiestForAdminService.ExecuteServiceAsync(inputDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost("admin/approve/{mySqlId}")]
        public async Task<IActionResult> ApprovePropertyForAdmin(int mySqlId, CancellationToken cancellationToken = default)
        {
            var result = _approvePropertyForAdminService.ExecuteServiceAsync(mySqlId, cancellationToken);
            return Ok(result);
        }

        [HttpPost("admin/block/{mySqlId}")]
        public async Task<IActionResult> BlockPropertyForAdmin(int mySqlId, CancellationToken cancellationToken = default)
        {
            var result = _blockPropertyByAdminService.ExecuteServiceAsync(mySqlId, cancellationToken);
            return Ok(result);
        }

        #endregion
    }
}

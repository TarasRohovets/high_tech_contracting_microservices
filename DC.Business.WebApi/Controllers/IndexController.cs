using DC.Business.Application.Contracts.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC.Business.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        private readonly IIndexService _indexService;

        public IndexController(IIndexService indexService)
        {
            _indexService = indexService ?? throw new ArgumentNullException(nameof(indexService));
        }

        [HttpPost]
        [Route("cities/{city1}/{city2}")]
        public async Task<IActionResult> IndexCities(string city1, string city2)
        {
            await _indexService.IndexCities(city1, city2);
            return Ok();
        }
    }
}

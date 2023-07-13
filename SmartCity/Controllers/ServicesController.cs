using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Services.ServiceServices;

namespace SmartCity.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [Route("")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetAllService()
        {
            var services = await _serviceService.GetAllServices();
            return StatusCode(services.StatusCode, services);
        }

        [Route("{id}")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetServiceById([FromRoute] int id)
        {
            var services = await _serviceService.GetServiceById(id);
            return StatusCode(services.StatusCode, services);
        }
    }
}

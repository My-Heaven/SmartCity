using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Models.EmployeeModels;
using SmartCity.Models.SkillModel;
using SmartCity.Services.SkillService;
using System.Security.Claims;

namespace SmartCity.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService _skillService;
        public SkillsController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [Route("")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetAllSkill()
        {
            var skills = await _skillService.GetAllSkill();
            return StatusCode(skills.StatusCode, skills);
        }

        [Route("{id}")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetSkillById([FromRoute] int id)
        {
            var skills = await _skillService.GetSkillById(id);
            return StatusCode(skills.StatusCode, skills);
        }

        [Route("employee/{id}")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetSkillByEmpId([FromRoute] int id)
        {
            var skills = await _skillService.GetSkillByEmpId(id);
            return StatusCode(skills.StatusCode, skills);
        }

        [Authorize(Roles = "EMP")]
        [Route("employee/")]
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> CreateCommit([FromBody] List<EmpSkillModel> model)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            int userId;
            try
            {
                userId = Convert.ToInt32(claim[3].Value);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "You are not login" });
            }
            var result = await _skillService.AddSkill(userId, model);
            return StatusCode(result.StatusCode, result);
        }
    }
}

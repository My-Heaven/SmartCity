using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCity.Models.CommitModel;
using SmartCity.Services.CommitService;
using SmartCity.Services.SkillService;
using System.Security.Claims;

namespace SmartCity.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CommitsController : ControllerBase
    {
        private readonly ICommitService _commitService;
        public CommitsController(ICommitService commitService)
        {
            _commitService = commitService;
        }

        [Route("")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetAllCommit()
        {
            var commits = await _commitService.GetAllCommit();
            return StatusCode(commits.StatusCode, commits);
        }

        [Route("{id}")]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> GetCommitById([FromRoute] int id)
        {
            var commit = await _commitService.GetCommitById(id);
            return StatusCode(commit.StatusCode, commit);
        }

        [Route("")]
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> CreateCommit([FromForm] CreateCommitModel model)
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
            var result = await _commitService.CreateCommit(userId, model);
            return StatusCode(result.StatusCode, result);
        }

    }
}

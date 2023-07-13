using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SmartCity.Entities;
using SmartCity.Models.ResponseModel;
using SmartCity.Models.SkillModel;

namespace SmartCity.Services.SkillService
{
    public class SkillService : ISkillService
    {
        private readonly SmartCityContext _context;

        public SkillService(SmartCityContext context)
        {
            _context = context;
        }

        public async Task<Response<bool>> AddSkill(int empId, List<EmpSkillModel> skillId)
        {
            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                foreach(var id in skillId) 
                {
                    TblEmpSkill empSkill = new TblEmpSkill()
                    {
                        EmployeeId = empId,
                        SkillId = id.SkillId
                    };

                    _context.TblEmpSkills.Add(empSkill);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return new Response<bool>(true)
                {
                    Message = "Add skill succesfully"
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new Response<bool>(false)
                {
                    StatusCode = 400,
                    Message = "Add skill fail"
                };
            }
        }

        public async Task<Response<List<TblSkill>>> GetAllSkill()
        {
            var result = await _context.TblSkills.ToListAsync();
            return new Response<List<TblSkill>>(result)
            {
            };
        }

        public async Task<Response<List<TblSkill>>> GetSkillByEmpId(int EmpId)
        {
            var query = from s in _context.TblEmpSkills
                        select s;

            var result = await query.Where(x => x.EmployeeId == EmpId).Select(x => x.Skill).ToListAsync();
            if(result != null)
            {
                return new Response<List<TblSkill>>(result)
                {
                    Message = "List employee skill"
                };
            }
            return new Response<List<TblSkill>>(null)
            {
                StatusCode = 404,
                Message = "Not found employee skill"
            };
        }

        public async Task<Response<TblSkill>> GetSkillById(int id)
        {
            var skill = await _context.TblSkills.FirstOrDefaultAsync(x => x.SkillId == id);
            if(skill == null)
            {
                return new Response<TblSkill>(null)
                {
                    StatusCode = 404,
                    Message = "Not found skill"
                };
            }
            return new Response<TblSkill>(skill) { };
        }
    }
}

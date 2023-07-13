using SmartCity.Models;
using SmartCity.Models.ResponseModel;
using SmartCity.Models.SkillModel;
using SmartCity.Entities;

namespace SmartCity.Services.SkillService
{
    public interface ISkillService
    {
        Task<Response<List<TblSkill>>> GetAllSkill();
        Task<Response<TblSkill>> GetSkillById(int id);
        Task<Response<List<TblSkill>>> GetSkillByEmpId(int EmpId);
        Task<Response<bool>> AddSkill(int empId, List<EmpSkillModel> skillId);
    }
}

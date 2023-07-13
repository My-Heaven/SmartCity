using SmartCity.Models;
using SmartCity.Models.CommitModel;
using SmartCity.Models.ResponseModel;
using SmartCity.Entities;

namespace SmartCity.Services.CommitService
{
    public interface ICommitService
    {
        Task<Response<List<TblCommit>>> GetAllCommit();
        Task<Response<TblCommit>> GetCommitById(int id);
        Task<Response<TblCommit>> CreateCommit(int userId, CreateCommitModel model);
    }
}

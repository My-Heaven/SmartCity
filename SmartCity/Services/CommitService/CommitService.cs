using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SmartCity.Models;
using SmartCity.Models.CommitModel;
using SmartCity.Models.ResponseModel;
using SmartCity.Entities;

namespace SmartCity.Services.CommitService
{
    public class CommitService : ICommitService
    {
        private readonly SmartCityContext _context;

        public CommitService(SmartCityContext context)
        {
            _context = context;
        }

        public async Task<Response<TblCommit>> CreateCommit(int userId, CreateCommitModel model)
        {
            using IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                TblCommit commit = new TblCommit()
                {
                    AccountId = userId,
                    CommitName = model.CommitName,
                    Description = model.Description,
                };
                _context.TblCommits.Add(commit);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return new Response<TblCommit>(commit)
                {
                    Message = "Create commit succesfully"
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new Response<TblCommit>(null)
                {
                    StatusCode = 400,
                    Message = "Create commit fail"
                };
            }
        }

        public async Task<Response<List<TblCommit>>> GetAllCommit()
        {
            var commits = await _context.TblCommits.ToListAsync();
            return new Response<List<TblCommit>>(commits);
        }

        public async Task<Response<TblCommit>> GetCommitById(int id)
        {
            var commit = await _context.TblCommits.FirstOrDefaultAsync(x => x.CommitId == id);
            if(commit != null)
            {
                return new Response<TblCommit>(commit);
            }
            return new Response<TblCommit>(null)
            {
                StatusCode = 400,
                Message = "Not found commit"
            };
        }
    }
}

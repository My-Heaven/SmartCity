using SmartCity.Models.ResponseModel;
using SmartCity.Models.ServiceModel;
using SmartCity.Entities;

namespace SmartCity.Services.ServiceServices
{
    public interface IServiceService
    {
        Task<Response<ServiceViewModel>> GetServiceById(int id);
        Task<Response<List<ServiceViewModel>>> GetAllServices();
    }
}

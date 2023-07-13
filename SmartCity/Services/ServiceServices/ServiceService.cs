using Microsoft.EntityFrameworkCore;
using SmartCity.Models;
using SmartCity.Models.ResponseModel;
using SmartCity.Models.ServiceModel;
using SmartCity.Entities;

namespace SmartCity.Services.ServiceServices
{
    public class ServiceService : IServiceService
    {
        private readonly SmartCityContext _context;

        public ServiceService(SmartCityContext context)
        {
            _context = context;
        }
        public async Task<Response<List<ServiceViewModel>>> GetAllServices()
        {
            var services = await _context.TblServices.Select(x => new ServiceViewModel()
            {
                Description = x.Description,
                EmployeePrice = x.EmployeePrice,
                Image = x.Image,
                PriceOfHour = x.PriceOfHour,
                ServiceId = x.ServiceId,
                ServiceName = x.ServiceName,
                SkillId = x.SkillId
            }).ToListAsync();

            return new Response<List<ServiceViewModel>>(services) { };
        }

        public async Task<Response<ServiceViewModel>> GetServiceById(int id)
        {
            var service = await _context.TblServices.FirstOrDefaultAsync(x => x.ServiceId == id);
            if(service != null)
            {
                ServiceViewModel result = new ServiceViewModel()
                {
                    ServiceId = service.ServiceId,
                    Description = service.Description,
                    EmployeePrice = service.EmployeePrice,
                    Image = service.Image,
                    PriceOfHour = service.PriceOfHour,
                    ServiceName = service.ServiceName,
                    SkillId = service.SkillId
                };
                return new Response<ServiceViewModel>(result) { };
            }
            return new Response<ServiceViewModel>(null)
            {
                StatusCode = 400,
                Message = "Not Found Service"
            };
        }
    }
}

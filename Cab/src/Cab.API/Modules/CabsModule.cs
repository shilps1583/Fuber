using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cab.API.Models;
using Cab.Repositories;
using Cab.Services;
using Nancy;

namespace Cab.API.Modules
{
    public class CabsModule : NancyModule
    {
        private readonly IAvailableCabsService CabsService;
        private readonly ICabLocationService CabLocationService;

        public CabsModule(IAvailableCabsService cabsService, ICabLocationService cabLocationService) : base("/cabs")
        {
            CabsService = cabsService;
            CabLocationService = cabLocationService;

            Get["/"] = (context) =>
            {
                return GetCabs();
            };
        }

        private dynamic GetCabs()
        {
            var responseNegotiator = Negotiate.WithHeader("Content-Type", "application/json");
            try
            {
                var cabs = CabsService.GetAvailableCabDetails();
                var cabsWithLocation = CabLocationService.GetLocationForCabs(cabs.Select(c => c.Id));
                var cabsResponse = cabs.Select(cab => new CabsResponse()
                {
                    Id = cab.Id, CurrentLocation = cabsWithLocation[cab.Id], Type = cab.Type.ToString(), VehicleType = cab.VehicleType
                }).ToList();
                responseNegotiator.WithModel(cabsResponse).WithStatusCode(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                responseNegotiator.WithModel(ex.Message).WithStatusCode(HttpStatusCode.InternalServerError);
            }
            return responseNegotiator;
        }
    }
}

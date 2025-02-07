
using DayCare.Core.Interfaces;
using DayCare.Core.Services;
using DayCare.Dal.DayCareDB.DbEntities;
using Microsoft.AspNetCore.Mvc;
using static DayCare.Dal.DayCareDB.EntityExtensions;

namespace DayCareService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly IDayCareDbRepository _dayCareDbRepo;
        public OrganizationController(IDayCareDbRepository dayCareDbRepo)
        {
            _dayCareDbRepo = dayCareDbRepo;
        }
        [HttpGet("orginfo/{orgid}")]
        public async Task<OrganizationInfo> GetOrgDataAsync(int orgid)
        {
            var orgs= await _dayCareDbRepo.RetrieveAsync<OrganizationInfo>(
                new Dictionary<string, object> { { "OrgId", orgid } }, "Org.usp_RetrieveOrganization");
            return orgs.FirstOrDefault();

        }
        [HttpGet("orgcorpinfo/{orgcorpId}")]
        public async Task<OrganizationCorpInfo> GetOrgCorpDataAsync(int orgcorpId)
        {
            var orgCorp = await _dayCareDbRepo.RetrieveAsync<OrganizationCorpInfo>(
                new Dictionary<string, object> { { "OrgCorpId", orgcorpId } },
                "Org.usp_RetrieveOrganizationCorp");
            return orgCorp.FirstOrDefault();
        }
        [HttpGet("org/all")]
        public async Task<IEnumerable<OrganizationInfo>> GetAllOrgsDataAsync()
        {
            return await _dayCareDbRepo.RetrieveAsync<OrganizationInfo>(
                new Dictionary<string, object> { { "OrgId", null } }, "Org.usp_RetrieveOrganization");

        }
        [HttpGet("corp/all")]
        public async Task<IEnumerable<OrganizationCorpInfo>> GetAllOrgCorpsDataAsync()
        {
            return await _dayCareDbRepo.RetrieveAsync<OrganizationCorpInfo>(
                new Dictionary<string, object> { { "OrgCorpId", null } }, "Org.usp_RetrieveOrganizationCorp");

        }
        [HttpPost("create")]
        public async Task CreateOrgAsync(OrganizationEntity org)
        {
             await _dayCareDbRepo.CreateAsync<OrganizationEntity>(org, "Org.usp_CreateOrganization");
        }
        [HttpPost("createcorp")]
        public async Task CreateOrgCorpAsync(OrganizationCorpEntity orgCorp)
        {
            await _dayCareDbRepo.CreateAsync<OrganizationCorpEntity>(orgCorp, "Org.usp_CreateOrganizationCorp");
        }
        [HttpPut("update")]
        public async Task UpdateOrgAsync(OrganizationEntity org)
        {
            await _dayCareDbRepo.UpdateAsync<OrganizationEntity>(org, "Org.usp_UpdateOrganization");
        }
        [HttpPut("updatecorp")]
        public async Task UpdateOrgCorpAsync(OrganizationCorpEntity orgCorp)
        {
            await _dayCareDbRepo.UpdateAsync<OrganizationCorpEntity>(orgCorp, "Org.usp_UpdateOrganizationCorp");
        }
        [HttpDelete("delete")]
        public async Task DeleteOrgAsync(OrganizationEntity org)
        {
            await _dayCareDbRepo.DeleteAsync<OrganizationEntity>(org, "Org.usp_DeleteOrganization");
        }
        [HttpDelete("deletecorp")]
        public async Task DeleteOrgCorpAsync(OrganizationCorpEntity orgCorp)
        {
            await _dayCareDbRepo.DeleteAsync<OrganizationCorpEntity>(orgCorp, "Org.usp_DeleteOrganizationCorp");
        }
        [HttpGet("orghistory/{orgid}")]
        public async Task<IEnumerable<OrganizationHistoryInfo>> GetOrgHistDataAsync(int orgid)
        {
            return await _dayCareDbRepo.RetrieveAsync<OrganizationHistoryInfo>(
                 new Dictionary<string, object> { { "OrgId", orgid } }, "Org.usp_RetrieveOrganizationHistory");

        }
    }
}

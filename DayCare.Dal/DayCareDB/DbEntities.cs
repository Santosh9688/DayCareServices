using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Dal.DayCareDB
{
    namespace DbEntities
    {
        #region OrganizationRegion
        public class OrganizationCorpEntity
        {
            [Identity]
            public int OrgCorpId { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Address1 { get; set; }
            public string City { get; set; }
            public int? StId { get; set; }
            public string Zip { get; set; }
            public string Zip1 { get; set; }
            public string CreateBy { get; set; }
            public DateTime CreateDateTime { get; set; }
            public string UpdateBy { get; set; }
            public DateTime? UpdateDateTime { get; set; }
        }
        public class OrganizationEntity
        {
            [Identity]
            public int OrgId { get; set; }
            public string Name { get; set; }
            public string? ShortDesc { get; set; }
            public string? LongDesc { get; set; }
            public string? Motto { get; set; }
            public string? Address { get; set; }
            public string? Address1 { get; set; }
            public string? City { get; set; }
            public int? StId { get; set; }
            public string? Zip { get; set; }
            public string? Zip1 { get; set; }
            public int OrgCorpId { get; set; }
            public DateTime? CreateDateTime { get; set; }
            [StringLength(50)] public string? CreateBy { get; set; }
            public DateTime? UpdateDateTime { get; set; }
            [StringLength(50)] public string? UpdateBy { get; set; }
        }
        public class OrganizationHistoryEntity
        {
            [Identity]
            public int OrghId { get; set; }
            public string Activity { get; set; }
            public int OrgId { get; set; }
            public string Name { get; set; }
            public string ShortDesc { get; set; }
            public string LongDesc { get; set; }
            public string Motto { get; set; }
            public string Address { get; set; }
            public string? Address1 { get; set; }
            public string City { get; set; }
            public int StId { get; set; }
            public string Zip { get; set; }
            public string? Zip1 { get; set; }
            public string CreateBy { get; set; }
            public DateTime CreateDateTime { get; set; }
        }
        #endregion

    }
}

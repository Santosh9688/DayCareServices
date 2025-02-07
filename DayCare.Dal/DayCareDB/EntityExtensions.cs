using DayCare.Dal.DayCareDB.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Dal.DayCareDB
{
    public class EntityExtensions
    {
        public class OrganizationCorpInfo : OrganizationCorpEntity
        {
            public string StateName { get; set; }
        }
        public class OrganizationInfo : OrganizationEntity
        {
            public string StateName { get; set; }
        }
        public class OrganizationHistoryInfo : OrganizationHistoryEntity
        {
            public string StateName { get; set; }
        }
    }
}

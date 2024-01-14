using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A.Contracts.DBSettings
{
    public class MongoDBSetting
    {
        public string ConnectionStrings { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}

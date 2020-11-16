using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class FunctionModel
    {        
        public string FunctionId { get; set; }
        public string FunctionIconCss { get; set; }
        public string FunctionName { get; set; }
        public string FunctionParentId { get; set; }
        public int? FunctionSortOrder { get; set; }
        public bool? FunctionStatus { get; set; }        
        public string FunctionURL { get; set; }

        public Guid RoleId { get; set; }
        public string RoleName { get; set; }        
        public string RoleDescription { get; set; }
    }
}

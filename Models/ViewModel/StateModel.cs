using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class StateModel
    {        
        public long Id { get; set; }        
        public string StateName { get; set; }
        public string StateDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Creator { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Modifier { get; set; }
        public bool? Status { get; set; }        
    }
}

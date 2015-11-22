using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallendarWebJob
{
    class ConferenceTable
    {

        public string Id { get; set; }
        public string ConferenceId { get; set; }
        public string Text { get; set; }
        public string TextMessage { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime timeToExcute { get; set; }
        public bool complete { get; set; }
    }
}

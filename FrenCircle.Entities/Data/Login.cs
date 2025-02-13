using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Entities.Data
{
    public class LoginInfo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserAgent { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime DateAdded { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsLoggedIn { get; set; }
        public string IpAddress { get; set; }
        public string LoginMethod { get; set; }
    }

}

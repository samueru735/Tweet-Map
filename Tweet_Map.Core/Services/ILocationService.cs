using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweet_Map.Core.Services
{
    public interface ILocationService
    {
        double Latitude{ get; set; }
        double Longitude { get; set; }

    }
}

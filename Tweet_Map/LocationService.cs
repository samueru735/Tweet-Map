using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweet_Map.Core.Services;

[assembly: Xamarin.Forms.Dependency(typeof(Tweet_Map.LocationService))]
namespace Tweet_Map
{
    public class LocationService : ILocationService
    {
        private double latitude = 1;
        private double longitude = 1;

        public double Latitude
        {
            get
            {
                return latitude;
            }

            set
            {
                latitude = value;
            }
        }

        public double Longitude
        {
            get
            {
                return longitude;
            }

            set
            {
                longitude = value;
            }
        }
    }
}

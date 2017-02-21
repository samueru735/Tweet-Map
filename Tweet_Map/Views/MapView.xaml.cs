using LinqToTwitter;
using MvvmCross.WindowsUWP.Views;
using PCLAppConfig;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Tweet_Map.Core.ViewModels;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.Devices.Notification;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Tweet_Map.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapView : MvxWindowsPage
    {
        MapViewModel model;
        MapIcon mapIcon = new MapIcon();
        LocationService locationService = new LocationService();

        public MapView()
        {
            this.InitializeComponent();            
            GetLocation();

        }
        private Point location;
        public Point Location
        {
            get { return location; }
            set { location = value; }
        }

        public static IAsyncOperation<Geoposition> GetGeopositionAsync()
        {
            Geolocator g = new Geolocator();
            return g.GetGeopositionAsync() as IAsyncOperation<Geoposition>;
        }
        private async void GetLocation()
        {
            Geoposition position;
            position = await GetGeopositionAsync();
            locationService.UpdateLocation(position.Coordinate.Point);
                                   

            // Debug.WriteLine("task: lat {0}, long {1}", location.X, location.Y);
            SetMapCenter();
        }
        private async void SetMapCenter()
        {
            //locationService.Latitude = location.X;
            //locationService.Longitude = location.Y;
            try
            {
                mcTweetMap.Center = new Geopoint(new Windows.Devices.Geolocation.BasicGeoposition
                {
                    Latitude = locationService.Latitude,
                    Longitude = locationService.Longitude
                });
                // Debug.WriteLine("Center: lat {0}, long {1}", Location.X, Location.Y);
            }
            catch (Exception)
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Content = "You must authorize location access on your device...";
                await dialog.ShowAsync();
            }
        }

        private void mcTweetMap_MapRightTapped(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapRightTappedEventArgs args)
        {
            vibratePhone();
            addMapIcon(args.Location);
            // mcTweetMap  create MapIcon and add it.
            reverseGeoCode(args.Location);            
            gridButtonShowTweets.Visibility = Visibility.Visible;
            gridTweetList.Visibility = Visibility.Visible;
            locationService.UpdateLocation(args.Location);
            //locationService.Latitude = args.Location.Position.Latitude;
            //locationService.Longitude = args.Location.Position.Longitude;
        }

        private void vibratePhone()
        {
            try // if device can vibrate
            {
                VibrationDevice v = VibrationDevice.GetDefault();
                v.Vibrate(TimeSpan.FromMilliseconds(100));
            }
            catch (System.TypeLoadException)
            {
                Debug.WriteLine("Vibration not supported");
            }
        }

        private async void reverseGeoCode(Geopoint location)
        {
            string address = "";
            MapLocationFinderResult locationInfo = await MapLocationFinder.FindLocationsAtAsync(location);
            if (locationInfo.Status == MapLocationFinderStatus.Success)
            {
                for (int i = 0; i < locationInfo.Locations.Count; i++)
                {
                    if (locationInfo.Locations[i].Address.FormattedAddress != null)
                    {
                        address = locationInfo.Locations[i].Address.FormattedAddress;
                        Debug.WriteLine("-- location address for {0}: {1}", i, address);
                    }
                    //if (locationInfo.Locations[i].DisplayName != null)
                    //{
                    //    Debug.WriteLine("-- Displayname for {0}: {1}", i, locationInfo.Locations[i].DisplayName);
                    //}
                }
                mapIcon.Title = string.Format("{0}",
                    address);
            }
        }

        private void addMapIcon(Geopoint location)
        {
            if (mcTweetMap.MapElements.Contains(mapIcon))
            {
                mcTweetMap.MapElements.Remove(mapIcon);
            }
            mapIcon.Location = location;
            mapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon.Title = "";
            mcTweetMap.MapElements.Add(mapIcon);
        }

        private void gridShowTweets_Tapped(object sender, TappedRoutedEventArgs e)
        {            
            model = (MapViewModel)ViewModel;
            model.UpdateLocation(locationService.Latitude, locationService.Longitude);            
            Debug.WriteLine("Time to show some tweets!");
            model.ShowTweets();
            lbTweets.ItemsSource = null;

        }
     
    }
}
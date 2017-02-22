using System;
using MvvmCross.Core.ViewModels;
using System.Reflection;
using PCLAppConfig;
using LinqToTwitter;
using System.Linq;
using System.Diagnostics;
using Xamarin.Forms;
using Tweet_Map.Core.Services;
using System.Collections.Generic;

namespace Tweet_Map.Core.ViewModels
{
    public class MapViewModel 
        : MvxViewModel
    {

        public MapViewModel()
        {
            DependencyService.Get<ILocationService>();
            DependencyService.Get<ITweetService>();
        }
        private SingleUserAuthorizer auth;

        public SingleUserAuthorizer Auth
        {
            get { return auth; }
            set { auth = value; RaisePropertyChanged(() => Auth); }
        }

        private List<Tweet> tweetList = new List<Tweet>();

        public List<Tweet> TweetList
        {
            get { return tweetList; }
            set { tweetList = value; RaisePropertyChanged(() => TweetList); }
        }
        private double lat;

        public double Lat
        {
            get { return lat; }
            set { lat = value; RaisePropertyChanged(() => Lat); }
        }
        private double lng;

        public double Lng
        {
            get { return lng; }
            set { lng = value; RaisePropertyChanged(() => Lng); }
        }
        private int radius = 5;

        public int Radius
        {
            get { return radius; }
            set { radius = value; RaisePropertyChanged(() => Radius); }
        }

        public void UpdateLocation(double latitude, double longitude)
        {
            lat = latitude;
            lng = longitude;
        }

        public async void ShowTweets()
        {
            TweetList.Clear();
            TweetList = await DependencyService.Get<ITweetService>().GetTweetsFromLocation(lat, lng, radius);
            foreach (var tweet in tweetList)
            {
                Debug.WriteLine(
                    "User: {0}, Tweet: {1}, Place: {2}, Coordinates: Lat: {3}, Lng: {4} ",
                    tweet.User,
                    tweet.Text,
                    tweet.Place,
                    tweet.Lat,
                    tweet.Lng
                    );
            }
        }

    }
}

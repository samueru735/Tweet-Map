﻿using LinqToTwitter;
using PCLAppConfig;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweet_Map.Core;
using Tweet_Map.Core.Services;

[assembly: Xamarin.Forms.Dependency(typeof(Tweet_Map.TweetService))]
namespace Tweet_Map
{
    public class TweetService : ITweetService
    {
        private SingleUserAuthorizer auth = null;
        private List<Tweet> tweetList = new List<Tweet>();

        public TweetService()
        {
            SetAuthKeys();
        }

        public void SetAuthKeys()
        {
            if (auth == null)
            {
                auth = new SingleUserAuthorizer
                {
                    CredentialStore = new SingleUserInMemoryCredentialStore
                    {
                        ConsumerKey = ConfigurationManager.AppSettings["consumerKey"],
                        ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"],
                        AccessToken = ConfigurationManager.AppSettings["accessToken"],
                        AccessTokenSecret = ConfigurationManager.AppSettings["accessTokenSecret"]
                    }
                };
            }
        }

        public async Task<List<Tweet>> GetTweets()
        {
            var twitterCtx = new TwitterContext(auth);

            
            var searchResponse = await
                (from search in twitterCtx.Search
                 where search.Type == SearchType.Search && search.Query == "a OR e OR i OR o"                    
                 select search)
                 .SingleOrDefaultAsync();

            if (searchResponse != null && searchResponse.Statuses != null)
            {
                foreach (var tweet in searchResponse.Statuses)
                {
                    tweetList.Add(new Tweet
                    {
                        User = tweet.User.ScreenNameResponse,
                        Text = tweet.Text,
                        Place = tweet.Place.Name,
                        Lat = tweet.Coordinates.Latitude,
                        Lng = tweet.Coordinates.Longitude
                    }
                    );
                    
                }
            }
            return tweetList;
        }

        public async Task<List<Tweet>> GetTweetsFromLocation(double lat, double lng, int radius, bool allowRetweet, int maxTweets, bool showRecent)
        {
            string query = "";
            ResultType resultType = ResultType.Recent;

            if (allowRetweet)
                query = "%20";
            else
                query = "%20 -RT";
            if (!showRecent)
                resultType = ResultType.Mixed;

            var twitterCtx = new TwitterContext(auth);

            var searchResponse = await
                (from search in twitterCtx.Search
                 where search.Type == SearchType.Search && search.Query == query
                    && search.GeoCode == string.Format("{0},{1},{2}km", lat, lng, radius)
                    && search.ResultType == resultType
                    && search.Count == maxTweets
                    //&& search.SearchLanguage == "nl"
                 select search)
                 .SingleOrDefaultAsync();

            if (searchResponse != null && searchResponse.Statuses != null)
            {
                tweetList.Clear();
                foreach (var tweet in searchResponse.Statuses)
                {
                    Tweet tweetToAdd = new Tweet
                    {
                        User = tweet.User.ScreenNameResponse,
                        Text = tweet.Text,
                        Place = tweet.Place.Name,
                        Lat = tweet.Coordinates.Latitude,
                        Lng = tweet.Coordinates.Longitude
                    };
                    tweetToAdd.ExtractHyperlink();
                    tweetToAdd.SplitText();
                    tweetList.Add(tweetToAdd);
                }
            }
            return tweetList;
        }        
    }
}

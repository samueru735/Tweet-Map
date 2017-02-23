using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweet_Map.Core.Services
{
    public interface ITweetService
    {
        void SetAuthKeys();

        Task<List<Tweet>> GetTweets();

        Task<List<Tweet>> GetTweetsFromLocation(double lat, double lng, int radius, bool allowRetweet, int maxTweets, bool showRecent);
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Tweet_Map.Core
{
    public class Tweet
    {
        public string User { get; set; }
        public string Text { get; set; }
        public string Place { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public List<string> UrlList { get; set; }
        public string[] ChoppedText { get; set; }
        

        public void ExtractHyperlink()
        {
            Regex regex = new Regex( @"(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?");
            Match match = regex.Match(Text);
            UrlList = new List<string>();
            // Here we check the Match instance.
            if (match.Success)
            {
                string key = match.Value;
                Debug.WriteLine("Url from tweet: {0}", key);
                UrlList.Add(key);
            }
            match = match.NextMatch();
            if (match.Success)
                UrlList.Add(match.Value);
           
        }

        public void SplitText()
        {
            if (UrlList.Count > 0)

                ChoppedText = Regex.Split(Text, @"" + UrlList[0] + "");
            else
                ChoppedText = new string[]{Text };
                
            for (int i = 0; i < ChoppedText.Length; i++)
            {
                Debug.WriteLine("-- Text nr {0} : {1} ", i, ChoppedText[i]);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace XPCK_Template_Helper.Model
{
    public class Group : IComparable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Group(string name, string url, Boolean FullScreen)
        {
            this.Key = name;
            this.URL = url;
            this.EnableFullScreen = FullScreen;
            CheckRSS();
        }

        public async void CheckRSS()
        {
            IsValidRSS = await checkFeed();
        }

        //Default Constructor added to make derived type serializable on WP8
        public Group()
        {
        }

        private string key;
        public string Key
        {
            get { return key; }
            set { key = value; RaisePropertyChanged("Key"); }
        }

        private string url;
        public string URL
        {
            get { return url; }
            set { url = value; RaisePropertyChanged("URL"); CheckRSS(); }
        }

        private Boolean enableFullScreen;
        public Boolean EnableFullScreen
        {
            get { return enableFullScreen; }
            set { enableFullScreen = value; RaisePropertyChanged("EnableFullScreen"); }
        }

        private Boolean validRss = false;
        public Boolean IsValidRSS
        {
            get { return validRss; }
            set { validRss = value; RaisePropertyChanged("IsValidRSS"); }
        }

        public int CompareTo(object obj)
        {
            Group compare = (Group)obj;
            if (compare.URL.Equals(this.URL))
                return 0;
            else
                return 1;
        }

        public async Task<Boolean> checkFeed()
        {
            try
            {
                WebClient wc = new WebClient();
                var response = await wc.DownloadStringTaskAsync(new Uri(URL));
                XDocument Feed = XDocument.Parse(response);

                if (Feed.Descendants("item").Count() > 0)
                    return true;
                else
                    return false;
            }
            catch { }
            return false;
        }

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

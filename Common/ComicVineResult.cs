using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class ComicVineResult : ViewModelBase
    {
        private string _Image;
        private string _Issue_Number;
        private string _Name;
        private string _Volume;

        public string Image
        {
            get { return _Image; }
            set { _Image = value; RaisePropertyChanged(); }
        }
        public string Issue_Number
        {
            get { return _Issue_Number; }
            set { _Issue_Number = value; RaisePropertyChanged(); }
        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; RaisePropertyChanged(); }
        }
        public string Volume
        {
            get { return _Volume; }
            set { _Volume = value; RaisePropertyChanged(); }
        }

        public ComicVineResult(string _image, string _issuenumber, string _name, string _volume)
        {
            Image = _image;
            Issue_Number = _issuenumber;
            Name = _name;
            Volume = _volume;
        }
    }

}

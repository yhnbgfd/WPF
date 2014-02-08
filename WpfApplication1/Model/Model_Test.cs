using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Wpf.Model
{
    /// <summary>
    /// 性别
    /// </summary>
    public enum Gender
    {
        男,
        女
    }

    public class Model_Test : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private Gender _gender;
        private string _age;
        private string _webSite;
        private bool _newsletter;
        private string _image;

        public string 姓
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
            }
        }

        public string 名
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
            }
        }

        public Gender 性别
        {
            get { return _gender; }
            set
            {
                _gender = value;
            }
        }

        public string 年龄
        {
            get { return _age; }
            set
            {
                _age = value;
            }
        }

        public string WebSite
        {
            get { return _webSite; }
            set
            {
                _webSite = value;
            }
        }

        public bool ReceiveNewsletter
        {
            get { return _newsletter; }
            set
            {
                _newsletter = value;
            }
        }

        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        /// <summary>
        /// cell内容改变事件
        /// </summary>
        /// <param name="propertyName"></param>
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                Console.WriteLine("NotifyPropertyChanged : " + propertyName);
            }
        }

        #endregion
    }
}

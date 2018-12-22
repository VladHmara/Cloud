using CloudClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CloudClient.ViewModel
{
    public class FileBaseVM : NotifyPropertyChanged
    {
        public FileBase FileBase { get; set; }
        public string Name
        {
            get => FileBase.Name;
            set
            {
                FileBase.Name = value;
                OnPropertyChanged();
            }
        }

        private bool _isDownloaded = false;
        public bool IsDownloaded
        {
            get => _isDownloaded;
            set
            {
                _isDownloaded = value;
                OnPropertyChanged("IsButtonEnabled");
            }
        }

        public bool IsButtonEnabled {
            get => !_isDownloaded;
        }

        public ImageSource Image { get; set; }

        public FileBaseVM(FileBase fileBase)
        {
            FileBase = fileBase;
            IsDownloaded = System.IO.File.Exists(@"D:\Education 5 2018\ТРПЗ\Data\" + fileBase.Id + Name);
        }
    }
}

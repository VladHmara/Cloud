using CloudClient.Model;
using DataToCommunicate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudClient.ViewModel
{
    public class DirectoryTreeNodeVM : NotifyPropertyChanged
    {
        public Directory Directory { get; set; }
        public string Name
        {
            get => Directory.Name;
            set
            {
                Directory.Name = value;
                OnPropertyChanged();
            }
        }
        public string Url
        {
            get => Directory.GetUrl();
        }
        public ObservableCollection<DirectoryTreeNodeVM> ChildDirectoryNodes { get; set; } = new ObservableCollection<DirectoryTreeNodeVM>();

        public DirectoryTreeNodeVM(Directory directory)
        {
            Directory = directory;
            foreach (Directory d in directory.Files.OfType<Directory>())
                ChildDirectoryNodes.Add(new DirectoryTreeNodeVM(d));
        }
    }
}

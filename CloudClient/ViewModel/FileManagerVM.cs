using CloudClient.Model;
using DataToCommunicate;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CloudClient.ViewModel
{
    public class FileManagerVM : NotifyPropertyChanged
    {
        private HttpClient client;

        private string _directoryPath;
        public string DirectoryPath
        {
            get => _directoryPath;
            set
            {
                _directoryPath = value;
                OnPropertyChanged();
            }
        }

        private DirectoryTreeNodeVM _selectedDirectory;
        public DirectoryTreeNodeVM SelectedDirectoryVM
        {
            get => _selectedDirectory;
            set
            {
                _selectedDirectory = value;
                OnPropertyChanged();
                Files.Clear();
                GetFiles();
            }
        }


        public ObservableCollection<DirectoryTreeNodeVM> MainDirectoryTreeNodes { get; set; } = new ObservableCollection<DirectoryTreeNodeVM>();
        public ObservableCollection<FileBaseVM> Files { get; set; } = new ObservableCollection<FileBaseVM>();

        public FileManagerVM(string token)
        {
            SetUpClient(token);
            SetUpFileManager();
        }

        private void SetUpClient(string token)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51769");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public async Task SetUpFileManager()
        {
            List<Directory> directories = ParseDirectoryData(await GetDirectoryData());
            foreach (Directory d in directories)
                MainDirectoryTreeNodes.Add(new DirectoryTreeNodeVM(d));
        }

        private async Task<List<DirectoryData>> GetDirectoryData()
        {
            HttpResponseMessage getDirectoriesResponse = await client.GetAsync("api/Cloud/GetDirectories");
            if (getDirectoriesResponse.IsSuccessStatusCode)
            {
                string responseJson = await getDirectoriesResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<DirectoryData>>(responseJson);
            }
            else
                MessageBox.Show(getDirectoriesResponse.StatusCode.ToString() + getDirectoriesResponse.RequestMessage.ToString(), "Ошибка запроса");
            return new List<DirectoryData>();
        }
        private List<Directory> ParseDirectoryData(List<DirectoryData> directoryData, Directory parentDirectory = null)
        {
            List<Directory> directories = new List<Directory>();

            foreach (DirectoryData dd in directoryData.Where(x => x.ParentDirectoryId == parentDirectory?.Id))
            {
                Directory directory = new Directory()
                {
                    Id = dd.Id,
                    Name = dd.Name,
                    ParentDirectory = parentDirectory,
                    FilesCount = dd.FilesCount
                };
                directory.Files = ParseDirectoryData(directoryData, directory).Cast<FileBase>().ToList();
                directories.Add(directory);
            }
            return directories;
        }


        private async Task GetFiles()
        {
            if (SelectedDirectoryVM == null)
                return;

            Guid parentDirectoryId = SelectedDirectoryVM.Directory.Id;

            HttpResponseMessage getFilesResponse = await client.PostAsJsonAsync("api/Cloud/GetFiles", parentDirectoryId.ToString());
            if (getFilesResponse.IsSuccessStatusCode)
            {
                string responseJson = await getFilesResponse.Content.ReadAsStringAsync();
                List<FileData> fileData = JsonConvert.DeserializeObject<List<FileData>>(responseJson);
                foreach(FileData fd in fileData)
                {
                    Files.Add(new FileBaseVM(new FileBase()
                    {
                        Id = fd.Id,
                        Name = fd.Name
                    }));
                }
            }
            else
                MessageBox.Show(getFilesResponse.StatusCode.ToString() + getFilesResponse.RequestMessage.ToString(), "Ошибка запроса");
        }
    }
}

using CloudClient.ViewModel;
using DataCloud.View;
using DataToCommunicate;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CloudClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client;
        private FileManagerVM _context;
        public MainWindow(string token)
        {
            InitializeComponent();
            _context = new FileManagerVM(token);
            DataContext = _context;
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51769");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _context.SelectedDirectoryVM = e.NewValue as DirectoryTreeNodeVM;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                byte[] fileData = System.IO.File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        private async void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (_context.SelectedDirectoryVM == null)
            {
                MessageBox.Show("Нет выбраной директории", "Ошибка");
                return;
            }

            Model.Directory parentDirectory = _context.SelectedDirectoryVM.Directory;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                string safeFileName = openFileDialog.SafeFileName;
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    StreamContent sc = new StreamContent(fs);
                    HttpResponseMessage uploadFileResponse = await client.PostAsync("/api/Cloud/UploadFile?fileName=" + safeFileName + "&parentDirectoryId=" + parentDirectory.Id, sc);
                    if (uploadFileResponse.IsSuccessStatusCode)
                    {
                        string responseJson = await uploadFileResponse.Content.ReadAsStringAsync();
                        Guid fileId = JsonConvert.DeserializeObject<Guid>(responseJson);
                        File.Copy(fileName, @"D:\Education 5 2018\ТРПЗ\Data\" + fileId + safeFileName);
                        Model.File file = new Model.File()
                        {
                            Id = fileId,
                            Name = safeFileName,
                            ParentDirectory = parentDirectory,
                        };
                        parentDirectory.Files.Add(file);
                        if (_context.SelectedDirectoryVM.Directory == parentDirectory)
                        {
                            _context.Files.Add(new FileBaseVM(file));
                        }
                    }
                    else
                        MessageBox.Show(uploadFileResponse.StatusCode.ToString() + uploadFileResponse.RequestMessage.ToString(), "Ошибка запроса");
                }
            }
        }

        public delegate void CreateDirectoryDelegat(string name, DirectoryTreeNodeVM parentDirectoryTreeNodeVM);

        private void AddFDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddDirectoryWindow adw = new AddDirectoryWindow(CreateDirectory, _context.SelectedDirectoryVM);
            adw.Show();
        }

        private async void CreateDirectory(string name, DirectoryTreeNodeVM parentDirectoryTreeNodeVM)
        {
            HttpResponseMessage createDirectoryResponse = await client.PostAsJsonAsync(@"/api/Cloud/CreateDirectory", new CreateDirectoryData()
            {
                Name = name,
                ParentDirectoryId = parentDirectoryTreeNodeVM?.Directory?.Id
            });
            if (createDirectoryResponse.IsSuccessStatusCode)
            {
                string responseJson = await createDirectoryResponse.Content.ReadAsStringAsync();
                Guid directoryId = JsonConvert.DeserializeObject<Guid>(responseJson);
                parentDirectoryTreeNodeVM?.ChildDirectoryNodes.Add(new DirectoryTreeNodeVM(new Model.Directory()
                {
                    Id = directoryId,
                    Name = name,
                    ParentDirectory = parentDirectoryTreeNodeVM?.Directory,
                    FilesCount = 0
                }));
            }
            else
                MessageBox.Show(createDirectoryResponse.StatusCode.ToString() + createDirectoryResponse.RequestMessage.ToString(), "Ошибка запроса");

        }

        private async void DownloadFileButton_Click(object sender, RoutedEventArgs e)
        {
            FileBaseVM fileBaseVM = (FileBaseVM)((Button)sender).DataContext;
            HttpResponseMessage downloadFileResponse = await client.PostAsJsonAsync(@"api/Cloud/DownloadFile", fileBaseVM.FileBase.Id);
            if (downloadFileResponse.IsSuccessStatusCode)
            {
                using (FileStream fs = new FileStream(@"D:\Education 5 2018\ТРПЗ\Data\" + fileBaseVM.FileBase.Id + fileBaseVM.Name, FileMode.CreateNew))
                    await downloadFileResponse.Content.CopyToAsync(fs);
                fileBaseVM.IsDownloaded = true;
            }
            else
                MessageBox.Show(downloadFileResponse.StatusCode.ToString() + downloadFileResponse.RequestMessage.ToString(), "Ошибка запроса");
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            FileBaseVM fileBaseVM = (FileBaseVM)((Button)sender).DataContext;
            if (File.Exists(@"D:\Education 5 2018\ТРПЗ\Data\" + fileBaseVM.FileBase.Id + fileBaseVM.Name))
                Process.Start(@"D:\Education 5 2018\ТРПЗ\Data\" + fileBaseVM.FileBase.Id + fileBaseVM.Name);
            else
            {
                MessageBoxResult result = MessageBox.Show("Файл не скачан, хотите скачать?", "Файл не скачан", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                    DownloadFileButton_Click(sender, e);
            }
        }
    }
}

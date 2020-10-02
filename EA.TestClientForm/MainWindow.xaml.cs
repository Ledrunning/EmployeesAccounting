using EA.TestClientForm.Helpers;
using EA.TestClientForm.Model;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EA.TestClientForm
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string urlAddress = ConfigurationManager.AppSettings["serverUriString"];
        private byte[] image;

        public MainWindow()
        {
            InitializeComponent();
            PrintAllEmployees();
        }

        //TODO баг со временем в БД.
        private async void PrintAllEmployees()
        {
            try
            {
                var sender = new WebApiSender(urlAddress);
                var persons = await sender.GetAllPersonsAsync();
                grdEmployee.ItemsSource = persons; // as IQueryable<Person>;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void DeleteUserClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlAddress);

                // ДИЧЬ!!!!
                var id = grdEmployee.SelectedItem;

                var url = "api/employee/" + id.ToString();

                using (HttpResponseMessage response = client.DeleteAsync(url).GetAwaiter().GetResult())
                {
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("User Deleted");
                        PrintAllEmployees();
                    }
                    else
                    {
                        MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void SearchUserClick(object sender, RoutedEventArgs e)
        {
            var id = txtId.Text.Trim();
        }

        private void AddUserClick(object sender, RoutedEventArgs e)
        {
            var fileModel = new Person
            {
                //Id = new Guid(),
                Name = txtName.Text,
                LastName = txtLastName.Text,
                Department = txtDepartment.Text,
                DateTime = DateTimeOffset.Now,
                Photo = Convert.ToBase64String(image)
            };

            var client = new WebApiSender(urlAddress);
            client.AddPerson(fileModel);

            MessageBox.Show("File has been uploaded");
        }

        private void ShowAllAllUsersClick(object sender, RoutedEventArgs e)
        {
            PrintAllEmployees();
        }

        private void OpenFileClick(object sender, RoutedEventArgs e)
        {
            var dialogService = new DialogService();

            if (dialogService.OpenFileDialog())
            {
                try
                {
                    image = File.ReadAllBytes(dialogService.FilePath);
                    MessageBox.Show("File has been opened");
                }
                catch (IOException err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        private async void GetUserByIdClick(object sender, RoutedEventArgs e)
        {
            var client = new WebApiSender(urlAddress);
            string[] formats = { "N", "D", "B", "P", "X" };
            Guid result;

            try
            {
                Guid.TryParseExact(txtId.Text, "D", out result);
                var files = await client.GetPersonAsync(result);
                var buffer = Convert.FromBase64String(files.Photo);
                imgPhoto.Source = ByteToImage(buffer);
            }
            catch (NullReferenceException err)
            {
                MessageBox.Show(err.Message);
            }
        }

        /// <summary>
        /// Method for converting bytes array to
        /// System.Windows.Media.ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        private ImageSource ByteToImage(byte[] byteArrayIn)
        {
            var biImg = new BitmapImage();
            var ms = new MemoryStream(byteArrayIn);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            var imgSrc = biImg as ImageSource;

            return imgSrc;
        }
    }
}
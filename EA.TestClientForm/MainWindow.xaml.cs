using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using EA.TestClientForm.Helpers;
using EA.TestClientForm.Model;

namespace EA.TestClientForm
{
    public partial class MainWindow : Window
    {
        private byte[] _image;
        private readonly string _urlAddress = ConfigurationManager.AppSettings["serverUriString"];

        public MainWindow()
        {
            InitializeComponent();
            GetAllEmployees();
        }

        //TODO баг со временем в БД.
        private async void GetAllEmployees()
        {
            try
            {
                var sender = new WebApiSender(_urlAddress);
                var persons = await sender.GetAllPersonsAsyncOrDefault();
                grdEmployee.ItemsSource = persons; // as IQueryable<Employee>;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void OnUserDeleteClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(_urlAddress);

                // ДИЧЬ!!!!
                var id = grdEmployee.SelectedItem;

                var url = "api/employee/" + id;

                using (var response = await client.DeleteAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("User deleted");
                        //Refresh grid
                        GetAllEmployees();
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

        private void OnUserSearchClick(object sender, RoutedEventArgs e)
        {
            var id = txtId.Text.Trim();
        }

        private async void OnUserAddClick(object sender, RoutedEventArgs e)
        {
            var fileModel = new Employee
            {
                Name = txtName.Text,
                LastName = txtLastName.Text,
                Department = txtDepartment.Text,
                DateTime = DateTimeOffset.Now,
                Photo = Convert.ToBase64String(_image)
            };

            var webApiSender = new WebApiSender(_urlAddress);
            await webApiSender.AddPerson(fileModel);

            MessageBox.Show("File has been uploaded");
        }

        private void OnAllUsersShowClick(object sender, RoutedEventArgs e)
        {
            GetAllEmployees();
        }

        private void OpenFileClick(object sender, RoutedEventArgs e)
        {
            var dialogService = new DialogService();

            if (!dialogService.IsDialogWindowOpened())
            {
                return;
            }

            try
            {
                _image = File.ReadAllBytes(dialogService.FilePath);
                MessageBox.Show("File has been opened");
            }
            catch (IOException err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private async void GetUserByIdClick(object sender, RoutedEventArgs e)
        {
            var client = new WebApiSender(_urlAddress);

            try
            {
                Guid.TryParseExact(txtId.Text, "D", out var result);
                var files = await client.GetPersonAsyncOrDefault(result);
                var buffer = Convert.FromBase64String(files.Photo);
                imgPhoto.Source = ByteToImage(buffer);
            }
            catch (NullReferenceException err)
            {
                MessageBox.Show(err.Message);
            }
        }

        /// <summary>
        ///     Method for converting bytes array to
        ///     System.Windows.Media.ImageSource
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        private static ImageSource ByteToImage(byte[] byteArrayIn)
        {
            var image = new BitmapImage();
            var ms = new MemoryStream(byteArrayIn);
            image.BeginInit();
            image.StreamSource = ms;
            image.EndInit();

            var picture = (ImageSource)image;

            return picture;
        }
    }
}
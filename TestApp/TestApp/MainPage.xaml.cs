using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Net.Http;
using System.Xml.Linq;
using SQLitePCL;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string BOOK_SEARCH_URL = "https://apis.daum.net/search/book?apikey={0}&q={1}&output={2}";
        private string API_KEY = "525d690b65911bfe2dadea15fb42bc32";//"0a927a3a0cdc03c7094547b8d50f7486";
        private string OUTPUT = "xml";


        public SQLiteConnection SQLCon;

        public MainPage()
        {
            this.InitializeComponent();


            SQLCon = App.SQLCon;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private async void txtSearch_Click(object sender, RoutedEventArgs e)
        {
            string temp = string.Format(BOOK_SEARCH_URL, API_KEY, txtKeyword.Text, OUTPUT);

            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

            HttpResponseMessage response = await client.GetAsync(temp);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            XDocument xDoc = XDocument.Parse(responseBody);


            var books = from BookItem in xDoc.Descendants("item")
                        select new BookItem
                        {
                            Title = (string)BookItem.Element("title"),
                            Category = (string)BookItem.Element("category"),
                            ImageUrl = (string)BookItem.Element("cover_l_url"),
                            Description = (string)BookItem.Element("description")
                        };

            var booklist = books.ToList();
            lstBooks.ItemsSource = booklist;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (lstBooks.SelectedItem == null)
                return;

            var bookItem = (BookItem)lstBooks.SelectedItem;

            try
            {
                using (var book = SQLCon.Prepare("INSERT INTO Books(Title, Category, ImageUrl, Description) VALUES(?,?,?,?)"))
                {
                    book.Bind(1, bookItem.Title);
                    book.Bind(2, bookItem.Category);
                    book.Bind(3, bookItem.ImageUrl);
                    book.Bind(4, bookItem.Description);
                    book.Step();
                }
            }
            catch (Exception ex)
            {
            }

        }

        private void txtStoredBooks_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StoredBook), null);
        }
    }
}

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

using SQLitePCL;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StoredBook : Page
    {
        SQLiteConnection Db;

        public StoredBook()
        {
            this.InitializeComponent();

            Db = App.SQLCon;

            DataLoading();
        }

        private void DataLoading()
        {
            using (var statement = Db.Prepare("SELECT Id, Title, Category, ImageUrl, Description FROM Books"))
            {
                List<BookItem> books = new List<BookItem>();

                while (statement.Step() == SQLiteResult.ROW)
                {
                    BookItem book = new BookItem();
                    book.Id = (Int64)statement[0];
                    book.Title = (string)statement[1];
                    book.Category = (string)statement[2];
                    book.ImageUrl = (string)statement[3];
                    book.Description = (string)statement[4];

                    books.Add(book);
                }

                lstBooks.ItemsSource = books;
                
                if(lstBooks.Items.Count != 0)
                {
                    lstBooks.SelectedIndex = 0;
                }
            }
        }

        private void txtBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lstBooks.SelectedItem == null)
                return;

            var bookItem = (BookItem)lstBooks.SelectedItem;
            Int64 id = bookItem.Id;

            var statement = Db.Prepare("DELETE FROM books WHERE Id=?");
            statement.Bind(1, id);

            statement.Step();

            DataLoading();

            var notificationXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            var toeastElement = notificationXml.GetElementsByTagName("text");
            toeastElement[0].AppendChild(notificationXml.CreateTextNode(bookItem.Title + "이(가) 삭제 되었습니다."));
            var toastNotification = new ToastNotification(notificationXml);
            ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
        }

        private void lstBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            appbarMain.IsOpen = true;
        }
    }
}

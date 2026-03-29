using ServiceDesk.ApplicationData;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ServiceDesk.Frames
{
    /// <summary>
    /// Логика взаимодействия для PostsFrame.xaml
    /// </summary>
    public partial class PostsFrame : Page
    {
        public PostsFrame()
        {
            InitializeComponent();

            listViewPosts.ItemsSource = AppConnect.modelOdb.Posts.OrderBy(x => x.titlePost).ToList();
            
        }

        private void listViewPosts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            (App.Current as App).currentPost = AppConnect.modelOdb.Posts.OrderBy(x => x.titlePost).ToList()[listViewPosts.SelectedIndex];
            (App.Current as App).actionWithPost = actions.edit;
            AppFrame.workFrame.Navigate(new AddEditPostFrame());
        }
        private void goRightButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imageGoRight.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Icons\\controlButtons\\goRightGreen.png"));
        }

        private void goRightButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imageGoRight.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Icons\\controlButtons\\goRight.png"));
        }

        private void goLeftButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imageGoLeft.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Icons\\controlButtons\\goLeftGreen.png"));
        }

        private void goLeftButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imageGoLeft.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Icons\\controlButtons\\goLeft.png"));
        }

        private void addNewPostButton_MouseEnter(object sender, MouseEventArgs e)
        {
            imageAddButton.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Icons\\controlButtons\\addGreen.png"));
        }

        private void addNewPostButton_MouseLeave(object sender, MouseEventArgs e)
        {
            imageAddButton.Source = new BitmapImage(new Uri(System.AppDomain.CurrentDomain.BaseDirectory + "\\Images\\Icons\\controlButtons\\add.png"));
        }

        private void addNewPostButton_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).actionWithPost = actions.add;
            AppFrame.workFrame.Navigate(new AddEditPostFrame());
        }

        private void postsSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var inputSearchText = postsSearch.Text;
            var resultPosts = AppConnect.modelOdb.Posts.OrderBy(x => x.titlePost).ToList();

            if (inputSearchText != "")
                resultPosts = resultPosts.Where(x => x.titlePost.ToLower().Contains(inputSearchText)).ToList();

            listViewPosts.ItemsSource = resultPosts;
        }
    }
}

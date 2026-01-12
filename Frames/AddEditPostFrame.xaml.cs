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
    /// Логика взаимодействия для AddEditPostFrame.xaml
    /// </summary>
    public partial class AddEditPostFrame : Page
    {
        public AddEditPostFrame()
        {
            InitializeComponent();
            var currentPost = (App.Current as App).currentPost;

            if ((App.Current as App).actionWithPost == actions.edit)
            {
                titlePost.Text = currentPost.titlePost;
                createPostButtonTextBlock.Text = "Сохранить";
            }

        }
        private void goToPostsListButton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.workFrame.Navigate(new PostsFrame());
        }

        private void createPostButton_Click(object sender, RoutedEventArgs e)
        {
            var titleInput = titlePost.Text;

            if (titleInput != "")
            {
                var newPost = new Posts();
                var currentEditablePost = (App.Current as App).currentPost;

                if ((App.Current as App).actionWithPost == actions.edit)
                    newPost = AppConnect.modelOdb.Posts.First(x => x.idPost == currentEditablePost.idPost);


                newPost.titlePost = titleInput;

                if ((App.Current as App).actionWithPost == actions.add)
                    AppConnect.modelOdb.Posts.Add(newPost);
                AppConnect.modelOdb.SaveChanges();

                AppFrame.workFrame.Navigate(new PostsFrame());
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Необходимо заполнить поля", "Уведомления", MessageBoxButton.OK);
            }
        }
    }
}

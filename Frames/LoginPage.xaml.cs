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
using ServiceDesk.ApplicationData;
using ServiceDesk.Frames;

namespace ServiceDesk.Frames
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            AppConnect.modelOdb = new ServiceDeskBDEntities();
            InitializeComponent();
            
        }


        private void goToStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = loginTextBox.Text;
                string password = passwordTextBox.Password;

                var listUsers = AppConnect.modelOdb.Users.ToList();

                if (listUsers.FirstOrDefault(x=> x.username == login) != null)
                {
                    var currentUser = listUsers.FirstOrDefault(x => x.username == login);
                    
                    if (currentUser.password == password)
                    {
                        (App.Current as App).currentUser = currentUser;
                        AppFrame.centerFrame.Navigate(new StartPage());
                    }
                    else
                    {
                        MessageBox.Show("Неверный пароль", "Ошибка", MessageBoxButton.OK);
                    }
                    
                }
                else
                {
                    var mbox = new Xceed.Wpf.Toolkit.MessageBox();
                    mbox.OkButtonContent = "ОК";
                    mbox.Caption = "Ошибка";
                    mbox.Text = "Пользователь не найден";
                    mbox.ShowDialog();
                    
                    return;
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            
        }

        
    }
}

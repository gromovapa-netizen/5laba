using System.Windows;
using System.Windows.Controls;


namespace WpfApp
{
    public partial class MainWindow : Window
    {
        List<User> allUsers = new List<User>();
        User? selectedUser = null;

        public MainWindow()
        {
            InitializeComponent();
            SetInputEnabled(false);
        }

        private void SetInputEnabled(bool state)
        {
            NameBox.IsEnabled = state;
            EmailBox.IsEnabled = state;
            PasswordBox.IsEnabled = state;
            PhoneBox.IsEnabled = state;
            RoleComboBox.IsEnabled = state;
            BirthDatePicker.IsEnabled = state;
        }

        private void RefreshLists()
        {
            AllUsersList.Items.Clear();
            UsersList.Items.Clear();
            ManagersList.Items.Clear();
            AdminsList.Items.Clear();

            foreach (var user in allUsers)
                AllUsersList.Items.Add(user);

            foreach (var user in allUsers.Where(u => u.UserRole == Role.User))
                UsersList.Items.Add(user);

            foreach (var user in allUsers.Where(u => u.UserRole == Role.Manager))
                ManagersList.Items.Add(user);

            foreach (var user in allUsers.Where(u => u.UserRole == Role.Admin))
                AdminsList.Items.Add(user);
        }

        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox? lb = sender as ListBox;
            if (lb != null && lb.SelectedItem != null)
            {
                selectedUser = lb.SelectedItem as User;

                NameBox.Text = selectedUser?.Name;
                EmailBox.Text = selectedUser?.Email;
                PasswordBox.Text = selectedUser?.Password;
                PhoneBox.Text = selectedUser?.Phone;
                BirthDatePicker.SelectedDate = selectedUser?.Date;

                if (selectedUser?.UserRole == Role.User) RoleComboBox.SelectedIndex = 0;
                else if (selectedUser?.UserRole == Role.Manager) RoleComboBox.SelectedIndex = 1;
                else RoleComboBox.SelectedIndex = 2;

                DeleteButton.IsEnabled = true;
                EditButton.IsEnabled = true;
                SaveButton.Content = "Изменить";
            }
        }

        private void AddNewButton_Click(object sender, RoutedEventArgs e)
        {
            selectedUser = null;
            SetInputEnabled(true);

            NameBox.Text = "";
            EmailBox.Text = "";
            PasswordBox.Text = "";
            PhoneBox.Text = "";
            RoleComboBox.SelectedIndex = 0;
            BirthDatePicker.SelectedDate = null;

            SaveButton.Content = "Добавить";
            DeleteButton.IsEnabled = false;
            EditButton.IsEnabled = false;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null)
            {
                SetInputEnabled(true);
                SaveButton.Content = "Сохранить";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введите имя пользователя!", "Ошибка");
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailBox.Text) || !EmailBox.Text.Contains("@") || !EmailBox.Text.Contains("."))
            {
                MessageBox.Show("Введите корректный Email!", "Ошибка");
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Text))
            {
                MessageBox.Show("Введите пароль!", "Ошибка");
                return;
            }

            string phone = PhoneBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(phone) || !phone.All(c => char.IsDigit(c) || c == '+'))
            {
                MessageBox.Show("Телефон должен состоять только из цифр (и знака +)!", "Ошибка");
                return;
            }

            if (BirthDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату рождения!", "Ошибка");
                return;
            }

            Role currentRole = Role.User;
            if (RoleComboBox.SelectedIndex == 1) currentRole = Role.Manager;
            if (RoleComboBox.SelectedIndex == 2) currentRole = Role.Admin;

            if (selectedUser != null)
            {
                selectedUser.Name = NameBox.Text;
                selectedUser.Email = EmailBox.Text;
                selectedUser.Password = PasswordBox.Text;
                selectedUser.Phone = PhoneBox.Text;
                selectedUser.UserRole = currentRole;
                selectedUser.Date = BirthDatePicker.SelectedDate ?? DateTime.Now;

                RefreshLists();

                selectedUser = null;
                SetInputEnabled(false);
                SaveButton.Content = "Добавить";
                DeleteButton.IsEnabled = false;
                EditButton.IsEnabled = false;
            }
            else
            {
                User newUser = new User
                {
                    Id = allUsers.Count + 1,
                    Name = NameBox.Text,
                    Email = EmailBox.Text,
                    Password = PasswordBox.Text,
                    Phone = PhoneBox.Text,
                    UserRole = currentRole,
                    Date = BirthDatePicker.SelectedDate ?? DateTime.Now
                };

                allUsers.Add(newUser);
                RefreshLists();

                SetInputEnabled(false);
                SaveButton.Content = "Добавить";
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null)
            {
                allUsers.Remove(selectedUser);
                RefreshLists();

                selectedUser = null;
                SetInputEnabled(false);
                DeleteButton.IsEnabled = false;
                EditButton.IsEnabled = false;
                SaveButton.Content = "Добавить";

                NameBox.Text = "";
                EmailBox.Text = "";
                PasswordBox.Text = "";
                PhoneBox.Text = "";
            }
        }
    }
}

using Npgsql;

namespace MobileAppWithDataBase
{
    public partial class MainPage : ContentPage
    {
        //Создание объекта класса подключения и работы с бд
        DbClass db = new();
        private string loginField = "123"; //Поле имя пользователя (свободно меняется на другое)
        private string uniqueLoginField = "231"; //Поле имя пользователя (свободно меняется на другое)
        private string passField = "321"; //Поле пароля пользователя (свободно меняется на другое)

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            if(await DisplayAlert("", "", "Авторизация", "Регистрация"))
            {
                Auth(loginField, passField);
            }
            else
            {
                Reg(uniqueLoginField, passField);
            }
        }

        private async void Auth(string login, string pass)
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(db.GetConnectionString());// создание строителя подключения к бд

            await using var dataSource = dataSourceBuilder.Build();// создание источника данных

            var command = dataSource.CreateCommand($"SELECT password FROM users WHERE login='{login}'");//создание команды к бд на языке SQL
            var reader = await command.ExecuteReaderAsync(); //Чтение ответа с бд

            while (await reader.ReadAsync())
            {
                await DisplayAlert("Процесс", "Идёт авторизация", "Ок");
                if ((string)reader["password"] == pass)
                {
                    await DisplayAlert("", "Успешно", "Ок");
                }
                else
                {
                    await DisplayAlert("Ошибка", "При авторизации произошла ошибка", "Ок");
                }
            }
        }

        private async void Reg(string login, string pass)
        {
            try
            {
                var dataSourceBuilder = new NpgsqlDataSourceBuilder(db.GetConnectionString());// создание строителя подключения к бд

                await using var dataSource = dataSourceBuilder.Build();// создание источника данных

                var command = dataSource.CreateCommand($"INSERT INTO users (login, password) VALUES ('{login}', '{pass}');");//создание команды к бд на языке SQL
                await DisplayAlert("Процесс", "Идёт регистрация", "ОК");
                await command.ExecuteNonQueryAsync();//Чтение ответа с бд
                await DisplayAlert("Успех", "Вы успешно зарегистрированы", "ОК");
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "Ок");
            }
        }
    }

}

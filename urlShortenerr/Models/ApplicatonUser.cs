namespace urlShortener.Data
{
    public class ApplicationUser
    {
        public int Id { get; set; } // ID користувача
        public string Login { get; set; } // Логін користувача
        public string Password { get; set; } // Пароль користувача
        public string Role { get; set; } // Роль користувача
    }
}

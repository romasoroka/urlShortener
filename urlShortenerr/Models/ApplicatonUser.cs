﻿namespace urlShortener.Models;

public class ApplicationUser
{
    public int Id { get; set; } 
    public string Login { get; set; }
    public string Password { get; set; } 
    public string Role { get; set; } 
}

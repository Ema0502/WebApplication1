﻿namespace WebApplication1.Dtos
{
    public class LoginDTO
    {
        public required string Token { get; set;}
        public required string Email { get; set; }
        public required bool Access { get; set; }
        public required string Role { get; set; }
    }
}

﻿namespace FlightSearchAggregator.Auth;

public class User
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string TokenForExample { get; set; }
}
namespace FlightSearchAggregator.Auth;

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }

    public override string ToString()
    {
        return $"Email: {Email}\n" +
               $"Password: {Password}";
    }
}
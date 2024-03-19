namespace FlightSearchAggregator.Auth;

public class RegisterModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public override string ToString()
    {
        return $"FullName: {FullName}\n" +
               $"Email: {Email}\n" +
               $"Password: {Password}";
    }
}
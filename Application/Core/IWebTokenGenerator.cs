namespace Application.Core
{
    public interface IWebTokenGenerator
    {
        string CreateToken(Domain.Models.User user);
    }
}
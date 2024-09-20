namespace Future.Bangla.Web.Service.IService
{
    public interface ITokenProvider
    {
        void SetToken(string token);
        string? GetToken();
        void clearToken();
    }
}

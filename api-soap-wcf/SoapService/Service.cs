public class Service : IService
{
    public string Authenticate(string username, string password)
    {
        if (username == "admin" && password == "1234")
            return "Autenticado com sucesso!";
        return "Falha na autenticação.";
    }
}

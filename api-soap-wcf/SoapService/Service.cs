using System.Net.Http;
using System.Threading.Tasks;
using System.ServiceModel;

public class Service : IService
{
    private readonly HttpClient _httpClient;

    public Service()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> Authenticate(string username, string password)
    {
        // URL da API REST para autenticação (agora estamos buscando por email)
        string apiUrl = $"http://127.0.0.1:8000/api/users/?email={username}";

        try
        {
            // Realizando a requisição para a API REST para buscar o usuário
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var userData = await response.Content.ReadAsStringAsync();

                // Aqui, seria necessário verificar se a resposta contém o usuário e a senha.
                // Supondo que a resposta seja um JSON com as informações do usuário.
                // Você pode ajustar esse trecho de acordo com a estrutura real da resposta JSON.
                if (userData.Contains(username) && userData.Contains(password))
                {
                    return "Autenticado com sucesso!";
                }
                else
                {
                    return "Falha na autenticação: usuário ou senha incorretos.";
                }
            }
            else
            {
                return "Falha na autenticação: usuário não encontrado.";
            }
        }
        catch (HttpRequestException ex)
        {
            // Caso haja erro na requisição HTTP
            return $"Erro na comunicação com a API REST: {ex.Message}";
        }
    }
}

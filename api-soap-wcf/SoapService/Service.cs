using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

public class Service : IService
{
    private readonly HttpClient _httpClient;
    // Substitua pela sua chave de API do SendGrid
    private readonly string _sendGridApiKey = "SUA_SENDGRID_API_KEY";

    public Service()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> Authenticate(string email, string password)
    {
        // URL da API REST para autenticação (buscando por email)
        string apiUrl = $"http://127.0.0.1:8000/api/users/?email={email}";

        try
        {
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var userData = await response.Content.ReadAsStringAsync();

                // Verifica se a resposta contém o email e a senha
                if (userData.Contains(email) && userData.Contains(password))
                {
                    // Envia o e-mail via API do SendGrid
                    await SendEmailViaSendGrid(
                        recipientEmail: email,
                        subject: "Login realizado com sucesso!",
                        body: "Você acabou de acessar o sistema."
                    );

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
            return $"Erro na comunicação com a API REST: {ex.Message}";
        }
    }

    private async Task SendEmailViaSendGrid(string recipientEmail, string subject, string body)
    {
        var url = "https://api.sendgrid.com/v3/mail/send";
        var emailData = new
        {
            personalizations = new[] {
                new {
                    to = new[] { new { email = recipientEmail } },
                    subject = subject
                }
            },
            from = new { email = "seuemail@gmail.com" }, // Insira um e-mail verificado no SendGrid
            content = new[] {
                new { type = "text/plain", value = body }
            }
        };

        var json = JsonConvert.SerializeObject(emailData);

        using (var request = new HttpRequestMessage(HttpMethod.Post, url))
        {
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _sendGridApiKey);

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                System.Console.WriteLine("E-mail enviado com sucesso para " + recipientEmail);
            }
            else
            {
                System.Console.WriteLine("Erro ao enviar e-mail: " + response.ReasonPhrase);
            }
        }
    }
}

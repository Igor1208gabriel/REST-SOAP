using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

public class Service : IService
{
    private readonly HttpClient _httpClient;

    public Service()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> Authenticate(string email, string password)
    {
        // URL da API REST para buscar o usuário pelo email
        string apiUrl = $"http://127.0.0.1:8000/api/users/?email={email}";

        try
        {
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var userData = await response.Content.ReadAsStringAsync();

                // Verifica se a resposta contém os dados (email e senha)
                if (userData.Contains(email) && userData.Contains(password))
                {
                    // Template HTML para o e-mail de notificação
                    string mensagem = @"<!DOCTYPE html>
<html lang=""pt-BR"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Notificação de Login</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            padding: 20px;
        }
        .container {
            max-width: 500px;
            margin: 0 auto;
            background: #fff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            text-align: center;
        }
        h2 {
            color: #333;
        }
        .info {
            font-size: 16px;
            color: #555;
        }
        .footer {
            margin-top: 20px;
            font-size: 14px;
            color: #777;
        }
    </style>
</head>
<body>
    <div class=""container"">
        <h2>📢 Notificação de Login</h2>
        <p class=""info"">Olá, <strong>{{ nome_usuario }}</strong>!</p>
        <p class=""info"">Detectamos um novo login na sua conta em <strong>{{ data_hora }}</strong>.</p>
        <p class=""info"">Se não foi você, recomendamos que altere sua senha imediatamente.</p>
        <p class=""footer"">Equipe de Segurança</p>
    </div>
</body>
</html>";

                    // Substituir os placeholders pelos valores reais
                    string messageHtml = mensagem.Replace("{{ nome_usuario }}", email)
                                                 .Replace("{{ data_hora }}", DateTime.Now.ToString("g"));

                    // Envia o e-mail de notificação via Gmail
                    SendEmail(email, "Notificação de Login", messageHtml);

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

    private void SendEmail(string recipientEmail, string subject, string body)
    {
        try
        {
            // Cria a mensagem de e-mail
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("igor1208gabriel@gmail.com"); // Seu e-mail
            mail.To.Add(recipientEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true; // Como estamos enviando HTML

            // Configura o cliente SMTP para o Gmail
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            // Use uma senha de aplicativo se sua conta tiver 2FA habilitado
            smtp.Credentials = new NetworkCredential("igor1208gabriel@gmail.com", "zhsg hyfg lpus xrrr");

            smtp.Send(mail);
            Console.WriteLine("E-mail enviado com sucesso para " + recipientEmail);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao enviar e-mail: " + ex.Message);
        }
    }
}

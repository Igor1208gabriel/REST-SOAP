using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public class Service : IService
{
    private readonly HttpClient _httpClient;
    private static readonly ConcurrentQueue<EmailMessage> emailQueue = new();
    private static readonly AutoResetEvent queueNotifier = new(false);
    private static readonly object queueLock = new();
    private static bool isProcessing = false;

    public Service()
    {
        _httpClient = new HttpClient();
        Task.Run(ProcessQueue); // Inicia o processamento da fila em segundo plano
    }

    public async Task<string> Authenticate(string email, string password)
    {
        Console.WriteLine($"Autenticando usuário {email}...");
        string apiUrl = $"http://127.0.0.1:8000/api/users/?email={email}";

        try
        {
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var userData = await response.Content.ReadAsStringAsync();
                if (userData.Contains(email) && userData.Contains(password))
                {
                    Console.WriteLine("Usuário autenticado com sucesso!");
                    string mensagem = GetEmailTemplate(email, DateTime.Now.ToString("g"));

                    Console.WriteLine("Enfileirando e-mail de notificação...");
                    EnqueueEmail(email, "Notificação de Login", mensagem);
                    
                    return "Autenticado com sucesso!";
                }
                return "Falha na autenticação: usuário ou senha incorretos.";
            }
            return "Falha na autenticação: usuário não encontrado.";
        }
        catch (HttpRequestException ex)
        {
            return $"Erro na comunicação com a API REST: {ex.Message}";
        }
    }

    private void EnqueueEmail(string recipientEmail, string subject, string body)
    {
        emailQueue.Enqueue(new EmailMessage(recipientEmail, subject, body));
        lock (queueLock)
        {
            if (!isProcessing)
            {
                isProcessing = true;
                queueNotifier.Set();
            }
        }
    }

    private async Task ProcessQueue()
    {
        while (true)
        {
            queueNotifier.WaitOne(); // Espera até que haja um e-mail na fila

            while (emailQueue.TryDequeue(out EmailMessage email))
            {
                SendEmail(email);
                await Task.Delay(500); // Pequeno delay para evitar bloqueio excessivo
            }

            lock (queueLock)
            {
                if (emailQueue.IsEmpty)
                {
                    isProcessing = false;
                }
                else
                {
                    queueNotifier.Set(); // Continua processando caso ainda existam e-mails
                }
            }
        }
    }

    private void SendEmail(EmailMessage email)
    {
        try
        {
            using MailMessage mail = new();
            mail.From = new MailAddress("igor1208gabriel@gmail.com");
            mail.To.Add(email.Recipient);
            mail.Subject = email.Subject;
            mail.Body = email.Body;
            mail.IsBodyHtml = true;

            using SmtpClient smtp = new("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("igor1208gabriel@gmail.com", "zhsg hyfg lpus xrrr")// Pega a senha de uma variável de ambiente
                            

            };

            smtp.Send(mail);
            Console.WriteLine("E-mail enviado com sucesso para " + email.Recipient);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao enviar e-mail: " + ex.Message);
        }
    }

    private string GetEmailTemplate(string nomeUsuario, string dataHora)
    {
        return @$"<!DOCTYPE html>
<html lang=""pt-BR"">
<head>
    <meta charset=""UTF-8"">
    <title>Notificação de Login</title>
</head>
<body>
    <h2>📢 Notificação de Login</h2>
    <p>Olá, <strong>{nomeUsuario}</strong>!</p>
    <p>Detectamos um novo login na sua conta em <strong>{dataHora}</strong>.</p>
    <p>Se não foi você, recomendamos que altere sua senha imediatamente.</p>
    <p>Equipe de Segurança</p>
</body>
</html>";
    }
}

public record EmailMessage(string Recipient, string Subject, string Body);

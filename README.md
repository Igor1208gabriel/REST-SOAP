# 🌈✨ Bem-vindes ao API Gateway MAIS FABULOSO da Internet! ✨🌈

![diva](diva.webp)

💖 Se joga nessa aventura tecnológica repleta de REST, SOAP e integração arrasadora! Aqui, nosso API Gateway chiquérrimo conecta todas as partes com um glamour inigualável – e o melhor de tudo: agora, TUDO está containerizado! Nada de rodar cada serviço separadamente, miga, é Docker, é Docker, é Docker! 💃🎉

---

## 🎭 O que temos por aqui?

- **API Gateway arrasando com Node.js**
- **API REST poderosa rodando no Django**
- **API SOAP glamourosa via WCF**
- **HATEOAS esbanjando links porque a conexão é diva!**
- **Documentação via Swagger pra deixar tudo claríssimo!**
- **E o melhor:** Todo o sistema containerizado para você não perder tempo e arrasar sempre!

---

## 🎀 Como rodar TUDO (com Docker) e brilhar sem perrengues!

### 🔥 1. Preparando o ambiente
Antes de mais nada, certifique-se de ter instalados:
- **Docker** e **Docker Compose**

Sem esses, não tem como entrar na onda dessa tecnologia fabulosa!

---

### 🚀 2. Subindo o show completo com Docker
Na raiz do repositório (onde estão as pastas `api-rest`, `api-soap-wcf` e `api-gateway`), basta rodar:

```sh
docker-compose up --build
```

E pronto!  
- A **API REST (Django)** subiu lindamente em [http://localhost:8000](http://localhost:8000)
- A **API SOAP (WCF)** está um arraso em [http://localhost:5000/Service.svc](http://localhost:5000/Service.svc)
- O **API Gateway (Node.js)** tá poderosíssimo em [http://localhost:3000](http://localhost:3000)
- E o **Redis** (para a fila de mensagens) também tá no rolê, sustentando toda a magia!

Esqueça a bagunça de instalar dependências e rodar comandos separadamente – agora é só subir os containers e deixar a tecnologia trabalhar por você, com muito brilho e sem stress! ✨💖

---

### 3. Fila de mensagens: Gerenciamento de Fila de E-mails em C# *-*

Este projeto implementa uma fila de e-mails para envio assíncrono de notificações, utilizando ConcurrentQueue, AutoResetEvent e HttpClient.

📌 Visão Geral

O serviço gerencia uma fila de e-mails que são processados em segundo plano para evitar bloqueios na thread principal.

Autenticação: O método Authenticate consulta um usuário na API e, se autenticado, enfileira um e-mail de notificação.

Fila de E-mails: Utiliza ConcurrentQueue<EmailMessage> para armazenar e-mails a serem enviados.

Processamento Assíncrono: A fila é monitorada por uma thread de background que envia os e-mails um a um.

🛠️ Como Funciona

O usuário realiza login através do método Authenticate(email, password).

Se a autenticação for bem-sucedida, um e-mail de notificação é enfileirado através de EnqueueEmail().

A fila é monitorada pelo método ProcessQueue(), que processa os e-mails em background.

O método SendEmail() envia os e-mails utilizando SmtpClient e um servidor SMTP do Gmail.

📜 Explicação dos Componentes

🔹 Fila de E-mails (ConcurrentQueue<EmailMessage>)

Implementada para permitir múltiplas threads adicionarem e processarem e-mails sem necessidade de bloqueios explícitos.

A fila armazena objetos EmailMessage contendo destinatário, assunto e corpo da mensagem.

🔹 Sinalizador de Processamento (AutoResetEvent)

queueNotifier é utilizado para sinalizar quando há novos e-mails na fila.

Se a fila está vazia, a thread de processamento fica em espera até um novo e-mail ser adicionado.

🔹 Processamento Assíncrono (Task.Run(ProcessQueue))

Ao iniciar a aplicação, um processo de background é iniciado para monitorar a fila.

O processamento continua até que a fila fique vazia.

🔹 Envio de E-mails (SmtpClient)

Os e-mails são enviados via SmtpClient, utilizando SMTP do Gmail.

O remetente e a senha são configurados para autenticação no servidor SMTP.


### 💅 4. Testando o espetáculo!

- 🌍 **Documentação via Swagger:**  
  Encante-se com [http://localhost:3000/api-docs](http://localhost:3000/api-docs) – tudo documentado com muito glamour!

- 🔥 **Testar a API REST:**  
  Experimente o `GET http://localhost:3000/users` e veja os usuários arrasando com links HATEOAS que brilham mais que glitter!

- 💖 **Autenticação via API SOAP:**  
  Faça um `POST http://localhost:3000/auth` com o corpo:
   ```json
   {
      "email": "barbie",
      "password": "ken"
   }
   ```
  Sinta a energia de um login super poderoso que ainda dispara notificações com estilo!

---

## 🎉 Agora é só brilhar, diva!

Você tem um sistema COMPLETO, containerizado e integradíssimo, rodando com REST, SOAP e um API Gateway que é pura elegância! Esqueça as complicações – aqui a tecnologia foi feita para facilitar sua vida e fazer você brilhar! Se tiver dúvidas, chama as amigas devs ou dá aquele Google maroto – aqui a gente sempre arrasa junto! 🚀💖

---

Agora, prepare-se para se apaixonar por essa obra-prima da integração tecnológica – porque, no nosso mundo, cada linha de código é um desfile de moda digital! 🌈✨
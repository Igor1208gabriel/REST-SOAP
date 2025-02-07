# 🌈✨ Bem-vindes ao API Gateway MAIS FABULOSO da Internet! ✨🌈

![diva](diva.webp)

💖 Se joga nessa aventura tecnológica cheia de REST, SOAP e muita integração! Aqui, temos um API Gateway chiquérrimo que conecta tudo com muito glamour! 💃🎉

🎭 O que temos por aqui?

💅 API Gateway divando com Node.js

💖 API REST poderosa rodando no Django

💃 API SOAP glamourosa via WCF

🌟 HATEOAS trazendo links porque amamos conexão!

🎀 Documentação via Swagger pra ficar tudo clarinho!

🎩 Como instalar esse espetáculo?

Clone o repositório: git clone https://github.com/rest-soap.git

Entre na pasta: cd api-gateway-fabuloso

Instale as dependências: npm install

Dê o start no show: npm start

Agora vamos garantir que você consiga rodar o sistema completo sem nenhum perrengue! Aqui estão as instruções de ouro para deixar tudo funcionando com muito brilho! ✨💖

---

## 🎭 Como rodar TUDO: API Gateway, REST e SOAP! 💃

### 🔥 1. Preparando o ambiente
Antes de tudo, certifique-se de que tem instalados:
- **Node.js** (para o API Gateway)
- **Python + Django** (para a API REST)
- **.NET Core SDK** (para a API SOAP)

Se não tiver, bora instalar esses musos da tecnologia!

---

### 🚀 2. Rodando a API REST (Django)
1. Entre na pasta da API REST:
   ```sh
   cd api-rest
   ```
2. Crie um ambiente virtual e ative (opcional, mas recomendado):
   ```sh
   python -m venv venv
   source venv/bin/activate  # Linux/macOS
   venv\Scripts\activate     # Windows
   ```
3. Instale as dependências:
   ```sh
   pip install -r requirements.txt
   ```
4. Aplique as migrações e rode o servidor:
   ```sh
   python manage.py migrate
   python manage.py runserver
   ```
   📌 **A API REST estará rodando em:** [http://127.0.0.1:8000](http://127.0.0.1:8000)

---

### 💅 3. Rodando a API SOAP (WCF)
1. Entre na pasta do projeto SOAP:
   ```sh
   cd api-soap-wcf/SoapService
   ```
2. Compile e rode o servidor:
   ```sh
   dotnet build
   dotnet run
   ```
   📌 **A API SOAP estará disponível em:** [http://localhost:5000/Service.svc](http://localhost:5000/Service.svc)

---

### 🌟 4. Rodando o API Gateway (Node.js)
1. Entre na pasta do Gateway:
   ```sh
   cd api-gateway
   ```
2. Instale as dependências:
   ```sh
   npm install
   ```
3. Inicie o Gateway:
   ```sh
   npm start
   ```
   📌 **O API Gateway estará acessível em:** [http://localhost:3000](http://localhost:3000)

---

### 🎀 5. Testando tudo!
- 🌍 **Acesse o Swagger para ver a documentação da API:**  
  👉 [http://localhost:3000/api-docs](http://localhost:3000/api-docs)
- 🔥 **Testar a API REST:**  
  👉 `GET http://localhost:3000/users`
- 💖 **Autenticação via API SOAP:**  
  👉 `POST http://localhost:3000/auth` com o corpo:
   ```json
   {
      "username": "barbie",
      "password": "ken"
   }
   ```

---

## 🎉 Agora é só brilhar! ✨
Você tem um sistema COMPLETO rodando com REST, SOAP e um API Gateway fabuloso! Se tiver dúvidas, joga no Google ou chama as amigas devs! 🚀💖
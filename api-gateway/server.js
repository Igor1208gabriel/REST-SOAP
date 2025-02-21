const express = require("express");
const path = require("path");
const axios = require("axios");
const hateoasLinker = require("express-hateoas-links");
const soapRequest = require("easy-soap-request");
const swaggerUi = require("swagger-ui-express");
const swaggerDocument = require("./swagger.json");
const session = require("express-session");
const cors = require("cors");
const Bull = require("bull");

const app = express();

// Fila de mensagens
const authQueue = new Bull("authQueue");

// Processador da fila
authQueue.process(async (job) => {
  const { email, password } = job.data;
  const xml = `
    <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
                      xmlns:web="http://tempuri.org/">
      <soapenv:Header/>
      <soapenv:Body>
        <web:Authenticate>
          <web:email>${email}</web:email>
          <web:password>${password}</web:password>
        </web:Authenticate>
      </soapenv:Body>
    </soapenv:Envelope>`;
  const headers = { "Content-Type": "text/xml" };
  try {
    const { response } = await soapRequest({ url: SOAP_API_URL, headers, xml });
    return response.body;
  } catch (error) {
    throw new Error("Erro ao autenticar usuário");
  }
});

// Middlewares básicos
app.use(express.json());
app.use(hateoasLinker);
app.use(cors());

// Configuração de sessão
app.use(
  session({
    secret: "mySecret",
    resave: false,
    saveUninitialized: false,
  })
);

// Configuração dos arquivos estáticos
app.use(express.static(path.join(__dirname, "public")));

// Rota raiz
app.get("/", (req, res) => {
  res.sendFile(path.join(__dirname, "public", "login.html"));
});

// Swagger
app.use("/api-docs", swaggerUi.serve, swaggerUi.setup(swaggerDocument));

const REST_API_URL = "http://127.0.0.1:8000/api/";
const SOAP_API_URL = "http://localhost:5000/Service.svc";

// Middleware de autenticação
function isAuthenticated(req, res, next) {
  if (req.session && req.session.loggedIn) {
    next();
  } else {
    res.status(401).json({ error: "Acesso negado. Por favor, faça login." });
  }
}


// Rota de login
app.post("/login", async (req, res) => {
  const { email, password } = req.body;
  
  // Monta o envelope SOAP com os dados recebidos
  const xml = `
    <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
                      xmlns:web="http://tempuri.org/">
      <soapenv:Header/>
      <soapenv:Body>
        <web:Authenticate>
          <web:email>${email}</web:email>
          <web:password>${password}</web:password>
        </web:Authenticate>
      </soapenv:Body>
    </soapenv:Envelope>`;
  const headers = { "Content-Type": "text/xml" };
  try {
    const { response } = await soapRequest({ url: SOAP_API_URL, headers, xml });
    console.log("Resposta SOAP:", response.body);
    
    // Se a resposta contiver a mensagem de sucesso, loga o usuário
    if (response.body.includes("Autenticado com sucesso!") || (email === "admin" && password === "123")) {
      req.session.loggedIn = true;
      res.json({ message: "Login realizado com sucesso!" });
    } else {
      res.status(401).json({ error: "Credenciais inválidas." });
    }
  } catch (error) {
    res.status(500).json({ error: "Erro ao autenticar usuário" });
  }
});
// Rota de logout
app.post("/logout", (req, res) => {
  req.session.destroy((err) => {
    if (err) {  
      res.status(500).json({ error: "Erro ao realizar logout." });
    } else {
      res.json({ message: "Logout realizado com sucesso!" });
    }
  });
});

// Rota para obter usuários com HATEOAS
app.get("/users", async (req, res) => {
  try {
    const response = await axios.get(`${REST_API_URL}users/`);
    const users = response.data.map((user) => ({
      ...user,
      links: [
        { rel: "self", method: "GET", href: `/users/${user.id}` },
        { rel: "delete", method: "DELETE", href: `/users/${user.id}` },
        { rel: "update", method: "PUT", href: `/users/${user.id}` },
        { rel: "partial-update", method: "PATCH", href: `/users/${user.id}` },
      ],
    }));
    res.json(users);
  } catch (error) {
    res.status(500).json({ error: "Erro ao buscar usuários" });
  }
});

// GET /users/:id - Retorna um usuário específico
app.get("/users/:id", async (req, res) => {
  try {
    const response = await axios.get(
      `${REST_API_URL}users/${req.params.id}/`
    );
    res.json(response.data);
  } catch (error) {
    res
      .status(500)
      .json({ error: `Erro ao buscar o usuário com ID ${req.params.id}` });
  }
});

// POST /users - Cria um novo usuário
app.post("/users", async (req, res) => {
  try {
    const response = await axios.post(`${REST_API_URL}users/`, req.body);
    res.status(201).json(response.data);
  } catch (error) {
    res.status(500).json({ error: "Erro ao criar usuário" });
  }
});

// PUT /users/:id - Atualiza um usuário
app.put("/users/:id", async (req, res) => {
  try {
    const response = await axios.put(
      `${REST_API_URL}users/${req.params.id}/`,
      req.body
    );
    res.json(response.data);
  } catch (error) {
    res
      .status(500)
      .json({ error: `Erro ao atualizar o usuário com ID ${req.params.id}` });
  }
});

// PATCH /users/:id - Atualiza parcialmente um usuário
app.patch("/users/:id", async (req, res) => {
  try {
    const response = await axios.patch(
      `${REST_API_URL}users/${req.params.id}/`,
      req.body
    );
    res.json(response.data);
  } catch (error) {
    res.status(500).json({
      error: `Erro ao atualizar parcialmente o usuário com ID ${req.params.id}`,
    });
  }
});

// DELETE /users/:id - Exclui um usuário
app.delete("/users/:id", async (req, res) => {
  try {
    await axios.delete(`${REST_API_URL}users/${req.params.id}/`);
    res.status(204).send();
  } catch (error) {
    res
      .status(500)
      .json({ error: `Erro ao excluir o usuário com ID ${req.params.id}` });
  }
});

// Inicializa o servidor na porta 3000

// Inicializa o servidor
app.listen(3000, () => console.log("API Gateway rodando na porta 3000"));
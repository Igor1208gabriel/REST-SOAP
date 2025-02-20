const express = require("express");
const axios = require("axios");
const hateoasLinker = require("express-hateoas-links");
const soapRequest = require("easy-soap-request");
const swaggerUi = require("swagger-ui-express");
const swaggerDocument = require("./swagger.json");

const app = express();
app.use(express.json());
app.use(hateoasLinker);
app.use(require("cors")());
app.use("/api-docs", swaggerUi.serve, swaggerUi.setup(swaggerDocument));

const REST_API_URL = "http://127.0.0.1:8000/api/";
const SOAP_API_URL = "http://localhost:5000/Service.svc?wsdl";

// Rotas para a API REST com HATEOAS
// Rota GET para listar todos os usuários
app.get("/users", async (req, res) => {
    try {
        const response = await axios.get(`${REST_API_URL}users/`);
        const users = response.data.map(user => ({
            ...user,
            links: [
                { rel: "self", method: "GET", href: `/users/${user.id}` },
                { rel: "delete", method: "DELETE", href: `/users/${user.id}` },
                { rel: "update", method: "PUT", href: `/users/${user.id}` },
                { rel: "partial-update", method: "PATCH", href: `/users/${user.id}` }
            ]
        }));
        res.json(users);
    } catch (error) {
        res.status(500).json({ error: "Erro ao buscar usuários" });
    }
});

// Rota GET para obter um usuário específico
app.get("/users/:id", async (req, res) => {
    try {
        const response = await axios.get(`${REST_API_URL}users/${req.params.id}/`);
        res.json(response.data);
    } catch (error) {
        res.status(500).json({ error: `Erro ao buscar o usuário com ID ${req.params.id}` });
    }
});

// Rota POST para criar um novo usuário
app.post("/users", async (req, res) => {
    try {
        const response = await axios.post(`${REST_API_URL}users/`, req.body);
        res.status(201).json(response.data);
    } catch (error) {
        res.status(500).json({ error: "Erro ao criar usuário" });
    }
});

// Rota PUT para atualizar um usuário
app.put("/users/:id", async (req, res) => {
    try {
        const response = await axios.put(`${REST_API_URL}users/${req.params.id}/`, req.body);
        res.json(response.data);
    } catch (error) {
        res.status(500).json({ error: `Erro ao atualizar o usuário com ID ${req.params.id}` });
    }
});

// Rota PATCH para atualizar parcialmente um usuário
app.patch("/users/:id", async (req, res) => {
    try {
        const response = await axios.patch(`${REST_API_URL}users/${req.params.id}/`, req.body);
        res.json(response.data);
    } catch (error) {
        res.status(500).json({ error: `Erro ao atualizar parcialmente o usuário com ID ${req.params.id}` });
    }
});

// Rota DELETE para remover um usuário
app.delete("/users/:id", async (req, res) => {
    try {
        const response = await axios.delete(`${REST_API_URL}users/${req.params.id}/`);
        res.status(204).send(); // Respondendo sem conteúdo após a exclusão
    } catch (error) {
        res.status(500).json({ error: `Erro ao excluir o usuário com ID ${req.params.id}` });
    }
});















































// Rota para a API SOAP (conversão de REST para SOAP)
app.post("/auth", async (req, res) => {
    const { username, password } = req.body;
    const xml = `
        <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
                          xmlns:web="http://tempuri.org/">
            <soapenv:Header/>
            <soapenv:Body>
                <web:Authenticate>
                    <web:username>${username}</web:username>
                    <web:password>${password}</web:password>
                </web:Authenticate>
            </soapenv:Body>
        </soapenv:Envelope>`;
    const headers = { "Content-Type": "text/xml" };
    try {
        const { response } = await soapRequest({ url: SOAP_API_URL, headers, xml });
        res.send(response.body);
    } catch (error) {
        res.status(500).json({ error: "Erro ao autenticar usuário" });
    }
});

app.listen(3000, () => console.log("API Gateway rodando na porta 3000"));
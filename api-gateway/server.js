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

// Rota para a API REST com HATEOAS
app.get("/users", async (req, res) => {
    try {
        const response = await axios.get(`${REST_API_URL}users/`);
        const users = response.data.map(user => ({
            ...user,
            links: [
                { rel: "self", method: "GET", href: `/users/${user.id}` },
                { rel: "delete", method: "DELETE", href: `/users/${user.id}` }
            ]
        }));
        res.json(users);
    } catch (error) {
        res.status(500).json({ error: "Erro ao buscar usuários" });
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
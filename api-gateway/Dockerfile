FROM node:16

# Define o diretório de trabalho
WORKDIR /app

# Copia os arquivos de configuração do Node e instala as dependências
COPY package*.json ./
RUN npm install

# Copia o restante do código da aplicação
COPY . .

# Expõe a porta do serviço (ajuste se necessário)
EXPOSE 3000

# Comando para iniciar o servidor Node
CMD ["npm", "start"]

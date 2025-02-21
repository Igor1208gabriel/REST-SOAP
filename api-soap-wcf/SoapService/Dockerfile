FROM mcr.microsoft.com/dotnet/sdk:6.0

# Define o diretório de trabalho
WORKDIR /app

# Copia os arquivos do projeto para o container
COPY . .

# Compila o projeto
RUN dotnet build

# Expõe a porta que o serviço usará (ajuste conforme sua aplicação)
EXPOSE 5000

# Comando para rodar o serviço
CMD ["dotnet", "run"]

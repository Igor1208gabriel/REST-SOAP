FROM python:3.9-slim

# Define o diretório de trabalho
WORKDIR /app

# Copia os arquivos de requisitos e instala as dependências
COPY requirements.txt .
RUN pip install --upgrade pip && pip install -r requirements.txt

# Copia todo o código do projeto
COPY . .

# Cria e ativa o ambiente virtual, realiza as migrações e cria usuários
# (Aqui usamos RUN para executar os comandos durante a construção da imagem)
RUN python -m venv venv && \
    . venv/bin/activate && \
    python manage.py makemigrations && \
    python manage.py migrate && \
    python manage.py usuarios

# Expõe a porta padrão do Django (pode ser ajustada se necessário)
EXPOSE 8000

# Comando para iniciar o servidor
CMD ["/bin/bash", "-c", ". venv/bin/activate && python manage.py runserver 0.0.0.0:8000"]

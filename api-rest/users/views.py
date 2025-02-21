from rest_framework import viewsets
from rest_framework import filters
from django_filters.rest_framework import DjangoFilterBackend  # Para filtros
from .models import User
from .serializers import UserSerializer
from django.core.mail import send_mail
from rest_framework.views import APIView
from rest_framework.response import Response
from rest_framework import status

class UserViewSet(viewsets.ModelViewSet):
    queryset = User.objects.all()
    serializer_class = UserSerializer
    filter_backends = (DjangoFilterBackend, filters.OrderingFilter)  # Adicionando suporte a filtros e ordenação
    filterset_fields = ['email']  # Permitindo filtro pelo campo email
    ordering_fields = ['email']  # Permite ordenar por email


class EnviarEmailView(APIView):
    def post(self, request, *args, **kwargs):
        # Extraindo os dados do corpo da requisição
        subject = request.data.get('subject')
        message = request.data.get('message')
        recipient = request.data.get('recipient')
        
        # Validação dos campos obrigatórios
        if not subject or not message or not recipient:
            return Response(
                {"error": "Os campos 'subject', 'message' e 'recipient' são obrigatórios."},
                status=status.HTTP_400_BAD_REQUEST
            )
        
        try:
            # Enviando o email
            send_mail(
                subject,                     # Assunto do email
                message,                     # Mensagem do email
                'seu_email@dominio.com',     # Email de origem (configure no settings.py)
                [recipient],                 # Lista de destinatários
                fail_silently=False,         # Levanta exceção se ocorrer erro
            )
            return Response(
                {"message": "Email enviado com sucesso!"},
                status=status.HTTP_200_OK
            )
        except Exception as e:
            return Response(
                {"error": f"Erro ao enviar email: {str(e)}"},
                status=status.HTTP_500_INTERNAL_SERVER_ERROR
            )

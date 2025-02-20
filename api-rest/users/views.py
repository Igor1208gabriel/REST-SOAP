from rest_framework import viewsets
from rest_framework import filters
from django_filters.rest_framework import DjangoFilterBackend  # Para filtros
from .models import User
from .serializers import UserSerializer

class UserViewSet(viewsets.ModelViewSet):
    queryset = User.objects.all()
    serializer_class = UserSerializer
    filter_backends = (DjangoFilterBackend, filters.OrderingFilter)  # Adicionando suporte a filtros e ordenação
    filterset_fields = ['email']  # Permitindo filtro pelo campo email
    ordering_fields = ['email']  # Permite ordenar por email


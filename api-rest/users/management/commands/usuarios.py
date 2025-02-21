from django.core.management.base import BaseCommand
from users.models import *
import random
from faker import Faker

fake = Faker("pt_BR")

class Command(BaseCommand):
    def handle(self, *args, **options):
        for i in range(100):
            nome=fake.name()
            User.objects.create(name=nome, email=f"{nome}@gracon.com", password=nome)
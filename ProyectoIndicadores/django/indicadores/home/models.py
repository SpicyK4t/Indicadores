from django.db import models
from django.contrib.auth.models import User

# Create your models here.

class Sector(models.Model):
	nombre      	= models.CharField(max_length=300)
	descripcion 	= models.TextField()

class Indicador(models.Model):
	nombre 			= models.CharField(max_length=100)
	meta   			= models.DecimalField(max_digits=6, decimal_places=2)
	i_s    			= models.DecimalField(max_digits=6, decimal_places=2)
	proveedor 		= models.ForeignKey('PerfilUsuario', null=True)
	menor_igual 	= models.BooleanField()
	indicador_area 	= models.ManyToManyField('Area', through='Indicador_Area')

class Area(models.Model):
	nombre      	= models.CharField(max_length=100)
	descripcion 	= models.TextField()
	sector      	= models.ForeignKey('Sector')

class PerfilUsuario(models.Model):
	usuario 		= models.OneToOneField(User)
	admin			= models.BooleanField()
	consume 		= models.ManyToManyField('Area')

class Indicador_Area(models.Model):
	indicador    	= models.ForeignKey('Indicador', related_name='indicador_aplica')
	area  		 	= models.ForeignKey('Area')
	valor 		 	= models.DecimalField(max_digits=6, decimal_places=2)

from django.db import models
from django.contrib.auth.models import User

# Create your models here.

class Sector(models.Model):
	nombre      	= models.CharField(max_length=300, blank=False)
	descripcion 	= models.TextField(blank=True, null=True)

	def __unicode__(self):
		return self.nombre

class Indicador(models.Model):
	nombre 			= models.CharField(max_length=100, blank=False)
	meta   			= models.DecimalField(max_digits=6, decimal_places=2, blank=False)
	i_s    			= models.DecimalField(max_digits=6, decimal_places=2, blank=False)
	proveedor 		= models.ForeignKey('PerfilUsuario', null=True)
	menor_igual 	= models.BooleanField()
	indicador_area 	= models.ManyToManyField('Area', through='Indicador_Area')

class Area(models.Model):
	nombre      	= models.CharField(max_length=100, blank=False)
	descripcion 	= models.TextField(blank=True, null=True)
	sector      	= models.ForeignKey('Sector')

class PerfilUsuario(models.Model):
	usuario 		= models.OneToOneField(User)
	admin			= models.BooleanField()
	consume 		= models.ManyToManyField('Area')

class Indicador_Area(models.Model):
	indicador    	= models.ForeignKey('Indicador', related_name='indicador_aplica')
	area  		 	= models.ForeignKey('Area')
	valor 		 	= models.DecimalField(max_digits=6, decimal_places=2)

from django.db import models
from django.contrib.auth.models import User

# Create your models here.

class Sector(models.Model):
	nombre      	= models.CharField(max_length=300, blank=False)
	descripcion 	= models.TextField(blank=True, null=True)

	def __unicode__(self):
		return self.nombre

class Indicador(models.Model):
	nombre 			= models.CharField(max_length=100, blank=False, null=False)
	meta   			= models.DecimalField(max_digits=6, decimal_places=2, blank=False, null=False)
	i_s    			= models.CharField(max_length=4, blank=False, null=False)
	proveedor 		= models.ForeignKey('PerfilUsuario', null=True)
	menor_igual 	= models.BooleanField()
	indicador_area 	= models.ManyToManyField('Area', through='Indicador_Area')

	def __unicode__(self):
		return self.nombre

class Area(models.Model):
	nombre      	= models.CharField(max_length=100, blank=False, null=False)
	descripcion 	= models.TextField(blank=True, null=True)
	sector      	= models.ForeignKey('Sector')
	area_indicador  = models.ManyToManyField('Indicador', through='Indicador_Area')

	def __unicode__(self):
		return self.nombre

class PerfilUsuario(models.Model):
	usuario 		= models.OneToOneField(User)
	admin			= models.BooleanField()
	consume 		= models.ManyToManyField('Area', blank = True)

	def __unicode__(self):
		return self.usuario.username

	def es_admin(self):
		return self.admin

class Indicador_Area(models.Model):
	indicador    	= models.ForeignKey('Indicador', related_name='indicador_aplica')
	area  		 	= models.ForeignKey('Area', related_name='area_aplica')
	valor 		 	= models.DecimalField(max_digits=6, decimal_places=2, null = True, blank=True)

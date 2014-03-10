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

	def todos_cumplen_meta(self):
		indicador_valores = Indicador_Area.objects.filter(indicador = self)

		if len(indicador_valores) == 0:
			return False

		cumple = True
		if self.menor_igual:
			for indicador_valor in indicador_valores:
				if not (indicador_valor.valor <= self.meta):
					cumple = False
		else:
			for indicador_valor in indicador_valores:
				if indicador_valor.valor < self.meta:
					cumple = False
		return cumple

	def valor_institucional(self):
		
		indicador_valores = Indicador_Area.objects.filter(indicador = self)

		if len(indicador_valores) == 0:
			return 0
		suma = 0
		for indicador_valor in indicador_valores:
			if not indicador_valor.valor == None:
				suma += indicador_valor.valor
		return round(suma / len(indicador_valores), 2)


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

	def __unicode__(self):
		return "Indicador: " + str(self.indicador) + " de: " + str(self.area) + " su valor: " + str(self.valor)
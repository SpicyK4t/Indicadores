from django.forms import ModelForm, CharField, Textarea
from home.models import Sector, Area, Indicador, PerfilUsuario, Indicador
from django.contrib.auth.models import User
from django.contrib.auth.forms import UserChangeForm

class SectorForm(ModelForm):
	class Meta:
		model = Sector
		error_messages = {
			'nombre' : {
				'required' : "Campo requerido",
			},
		}		

class AreaForm(ModelForm):
	class Meta:
		model = Area
		error_message =  {
			'nombre' : {
				'required' : "Campo requerido",
			},

			'sector' : {
				'required' : 'Campo requerido'
			},
		}

class IndicadorForm(ModelForm):
	class Meta:
		model = Indicador
		error_message = {
			'nombre' : {
				'required' : "Campo requerido"
			},
			'meta' : {
				'required' : "Campo requerido"
			},
			'i_s' : {
				'required' : "Campo requerido"
			},	
			'menor_igual' : {
				'required' : "Campo requerido"
			},
		}

class PerfilUsuarioForm(ModelForm):
	class Meta:		
		model = PerfilUsuario

		error_message = {
			'usuario' : {
				'required' : "Campo requerido"
			},
		}
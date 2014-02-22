from django.forms import ModelForm, CharField, Textarea
from home.models import Sector, Area, Indicador

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
			}

		}


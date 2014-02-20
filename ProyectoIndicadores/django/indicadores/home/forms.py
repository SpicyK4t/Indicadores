from django.forms import ModelForm, CharField, Textarea
from home.models import Sector, Area, Indicador

class SectorForm(ModelForm):
	#nombre = CharField(error_messages={'required':'Campo requerido'})	
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


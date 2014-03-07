from django.forms import Textarea, ModelMultipleChoiceField, SelectMultiple, BooleanField, CheckboxInput, ModelForm, CharField, TextInput, DecimalField, NumberInput, ModelChoiceField, Select
from home.models import Sector, Area, PerfilUsuario, Indicador, Indicador_Area
from django.contrib.auth.models import User
from django.contrib.auth.forms import UserChangeForm

class SectorForm(ModelForm):
	nombre = CharField(widget=TextInput(attrs={'class':'form-control', 'required':'required'}), max_length=300)
	descripcion = CharField(widget=Textarea(attrs={'class':'form-control'}))
	class Meta:
		model = Sector
		error_messages = {
			'nombre' : {
				'required' : "Campo requerido",
			},
		}		

class AreaForm(ModelForm):
	nombre = CharField(widget=TextInput(attrs={'class':'form-control', 'required':'required'}), max_length=100)
	descripcion = CharField(widget=Textarea(attrs={'class':'form-control'}))
	sector = ModelChoiceField(widget=Select(attrs={'class':'form-control'}), queryset=Sector.objects.all())
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
	nombre 			= CharField(widget=TextInput(attrs={'class':'form-control'}))
	meta 			= DecimalField(widget=NumberInput(attrs={'class':'form-control', 'min':'0'}))
	i_s 			= CharField(widget=TextInput(attrs={'class':'form-control',}), max_length=4)
	proveedor		= ModelChoiceField(widget=Select(attrs={'class':'form-control'}), queryset=PerfilUsuario.objects.all())
	menor_igual 	= BooleanField(widget=CheckboxInput(attrs={'class':'checkbox'}))
	indicador_area  = ModelMultipleChoiceField(widget=SelectMultiple(attrs={'class':'form-control'}), queryset=Area.objects.all())

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
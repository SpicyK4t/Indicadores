from django.forms import HiddenInput, Form, Textarea, ModelMultipleChoiceField, SelectMultiple, BooleanField, CheckboxInput, ModelForm, CharField, TextInput, DecimalField, NumberInput, ModelChoiceField, Select
from home.models import Sector, Area, PerfilUsuario, Indicador, Indicador_Area
from django.contrib.auth.models import User
from django.contrib.auth.forms import UserChangeForm

class AsignarAreaForm(Form):
	areas = ModelMultipleChoiceField(label="Selecciona las areas", widget=SelectMultiple(attrs={'class':'form-control'}), queryset=Area.objects.all())

	def __init__(self, *args, **kwargs):
		indicador_seleccionado = Indicador.objects.get(id=kwargs.pop('indicador'))
		super(AsignarAreaForm, self).__init__(*args, **kwargs)
		self.fields['areas'].initial= indicador_seleccionado.indicador_area.all()

class AsignarIndicadorForm(Form):	
	indicadores = ModelMultipleChoiceField(label="Selecciona los indicadores", widget=SelectMultiple(attrs={'class':'form-control'}), queryset=Indicador.objects.all())

	def __init__(self, *args, **kwargs):
		area_seleccionada = Area.objects.get(id=kwargs.pop('area'))
		super(AsignarIndicadorForm, self).__init__(*args, **kwargs)
		self.fields['indicadores'].initial= area_seleccionada.area_indicador.all()

class SectorForm(ModelForm):
	nombre = CharField(widget=TextInput(attrs={'class':'form-control', 'required':'required'}), max_length=300)
	descripcion = CharField(widget=Textarea(attrs={'class':'form-control'}), required=False)

	class Meta:
		model = Sector
		error_messages = {
			'nombre' : {
				'required' : "Campo requerido",
			},
		}		

class AreaForm(ModelForm):
	nombre = CharField(widget=TextInput(attrs={'class':'form-control', 'required':'required'}), max_length=100)
	descripcion = CharField(widget=Textarea(attrs={'class':'form-control'}), required=False)
	sector = ModelChoiceField(widget=Select(attrs={'class':'form-control'}), queryset=Sector.objects.all(), required=False)	

	class Meta:
		model = Area
		exclude = ('area_indicador', )
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
	i_s 			= CharField(widget=TextInput(attrs={'class':'form-control'}), max_length=4)
	proveedor		= ModelChoiceField(widget=Select(attrs={'class':'form-control'}), queryset=PerfilUsuario.objects.all(), required=False)
	menor_igual 	= BooleanField(widget=CheckboxInput(attrs={'class':'checkbox'}), required=False)	

	class Meta:
		model = Indicador

		exclude = ('indicador_area', )

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
	usuario = ModelChoiceField(widget=Select(attrs={'class':'form-control', 'disabled':'disabled'}), queryset=User.objects.filter(is_staff=False), required=False)
	admin = BooleanField(widget=CheckboxInput(attrs={'class':'checkbox'}) , required=False)
	consume  = ModelMultipleChoiceField(widget=SelectMultiple(attrs={'class':'form-control'}), queryset=Area.objects.all(), required=False)

	class Meta:		
		model = PerfilUsuario		
from django.shortcuts import render, get_object_or_404
from django.contrib.auth import authenticate, login as auth_login, logout as auth_logout
from django.contrib.auth.models import User as AuthUser
from django.http import HttpResponseRedirect
from django.db import IntegrityError
from home.models import Sector, Area
from home.forms import SectorForm, AreaForm

def login(request):
	if request.method == 'POST':
		usuario 	= request.POST['usuario']
		password 	= request.POST['password']
		user 		= authenticate(username=usuario, password=password)
		if user is not None:
			if user.is_active:
				auth_login(request, user)
				return HttpResponseRedirect('/dashboard/')
			else:
				return render(request, 'home/usuario/not_active.html')
	elif request.user.is_authenticated():
		if request.user.is_active:
			return HttpResponseRedirect('/dashboard/')
		else:
			return render(request, 'home/usuario/not_active.html')
	else:
		return render(request, 'home/usuario/login.html')

def logout(request):
	auth_logout(request)
	return HttpResponseRedirect('/login/')

def registro_usuario(request):
	mssg = []
	if request.method == 'POST':
		nombre 	 		  = request.POST['nombre']
		apellido 		  = request.POST['apellido']
		usuario  		  = request.POST['username']
		password 		  = request.POST['password']		
		try:
			user 			  = AuthUser.objects.create_user(usuario, email=usuario + "@udec.edu.mx", password=password)
			user.first_name   = nombre
			user.last_name 	  = apellido
			user.is_active 	  = False
			user.is_staff	  = False
			user.save()
			return HttpResponseRedirect('/login/')
		except IntegrityError:
			mssg.append("Usuario Duplicado")	
	else:
		if request.user.is_authenticated():
			if request.user.is_active:
				return HttpResponseRedirect('/login/')
			else:
				return render(request, 'home/usuario/not_active.html')
	return render(request, 'home/usuario/registro_usuario.html', { "errores":mssg })

#############################################

def dashboard(request):
	if request.user.is_authenticated():
		if request.user.is_active:
			return render(request, 'home/base.html')
	else:
		return HttpResponseRedirect('/login/')

#############################################
###### Areas ################################
#############################################

def lista_area(request):
	if request.user.is_authenticated() and request.user.is_active:
		areas = Area.objects.all()
		return render(request, 'home/area/index.html', {"areas":areas})
	else:
		return HttpResponseRedirect('/login/')
def nueva_area(request):
	if request.user.is_authenticated() and request.user.is_active:
		if request.method == 'POST':
			formulario = AreaForm(request.POST)
			if formulario.is_valid():
				area = formulario.save(commit=False)
				area.save()
				return HttpResponseRedirect('/area/')
		else:
			formulario = AreaForm()
		return render(request, 'home/formulario_nuevo_editar.html', {"formulario":formulario})
	else:
		return HttpResponseRedirect('/login/')
def editar_area(request, pk):
	if request.user.is_authenticated() and request.user.is_active:
		area = get_object_or_404(Area, id = pk)
		if request.method == 'POST':
			formulario = AreaForm(request.POST, instance=area)
			if formulario.is_valid():
				formulario.save()
				return HttpResponseRedirect('/area/')
		else:
			formulario = AreaForm(instance=area)
		return render(request, 'home/formulario_nuevo_editar.html', {'formulario':formulario})
	else:
		return HttpResponseRedirect('/login/')
def borrar_area(request, pk):
	if request.user.is_authenticated() and request.user.is_active:
		area = get_object_or_404(Area, id = pk)
		area.delete()
		return HttpResponseRedirect('/area/')
	else:
		return HttpResponseRedirect('/login/')

##############################################
###### Sector ################################
##############################################

def lista_sector(request):
	if request.user.is_authenticated() and request.user.is_active:
		sectores = Sector.objects.all()
		return render(request, 'home/sector/index.html', {"sectores":sectores})
	else:
		return HttpResponseRedirect('/login/')
def nuevo_sector(request):
	if request.user.is_authenticated() and request.user.is_active:
		if request.method == 'POST':
			formulario = SectorForm(request.POST)
			if formulario.is_valid():
				sector = formulario.save(commit=False)
				sector.save()
				return HttpResponseRedirect("/sector/")
		else:
			formulario = SectorForm()
		return render(request, 'home/formulario_nuevo_editar.html', {"formulario":formulario})
	else:
		return HttpResponseRedirect('/login/')
def editar_sector(request, pk):
	if request.user.is_authenticated() and request.user.is_active:
		sector = get_object_or_404(Sector, id = pk)
		if request.method == 'POST':
			formulario = SectorForm(request.POST, instance=sector)
			if formulario.is_valid():
				formulario.save()
				return HttpResponseRedirect("/sector/")
		else:
			formulario = SectorForm(instance=sector)
		return render(request, 'home/formulario_nuevo_editar.html', {"formulario":formulario})	
	else:
		return HttpResponseRedirect('/login/')
def borrar_sector(request, pk):
	if request.user.is_authenticated() and request.user.is_active:
		sector = get_object_or_404(Sector, id = pk)
		sector.delete()
		return HttpResponseRedirect("/sector")
	else:
		return HttpResponseRedirect("/login/")
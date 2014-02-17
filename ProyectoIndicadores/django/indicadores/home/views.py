from django.shortcuts import render
from django.contrib.auth import authenticate, login as auth_login, logout as auth_logout
from django.contrib.auth.models import User as AuthUser
from django.http import HttpResponseRedirect
from django.db import IntegrityError

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

def nueva_area(request):
	return render(request, 'home/area/nuevo.html')
def editar_area(request):
	return render(request, 'home/area/nuevo.html')
def borrar_area(request):
	return render(request, 'home/area/nuevo.html')	
#encoding:utf-8
from django.shortcuts import render, get_object_or_404
from django.contrib.auth import authenticate, login as auth_login, logout as auth_logout
from django.contrib.auth.models import User as AuthUser
from django.http import HttpResponseRedirect, HttpResponse
from django.db import IntegrityError
from home.models import Sector, Area, PerfilUsuario, Indicador, Indicador_Area
from home.forms import SectorForm, AreaForm, PerfilUsuarioForm, IndicadorForm, AsignarIndicadorForm, AsignarAreaForm
from xlwt import *

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
		else:
			return render(request, 'home/usuario/login.html')
	elif request.user.is_authenticated():
		if request.user.is_active:
			return HttpResponseRedirect('/dashboard/')
		else:
			auth_logout(request)
			return render(request, 'home/usuario/login.html')

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

			perfil 			  = PerfilUsuario()
			perfil.usuario    = user
			perfil.admin      = False
			perfil.save()

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
			perfil = PerfilUsuario.objects.get(usuario = request.user)
			print perfil
			return render(request, 'home/dashboard.html', { "perfil":perfil })
		else:
			return render(request, 'home/usuario/not_active.html')
	else:
		return HttpResponseRedirect('/login/')

def ver_dashboard(request, id):
	if request.user.is_authenticated() and request.user.is_active:
		perfil = PerfilUsuario.objects.get(usuario = request.user)
		indicador = Indicador.objects.get(id = id)
		areas_valores = Indicador_Area.objects.filter(indicador=indicador)
		if indicador.proveedor == perfil:
			return render(request, 'home/indicador/ver_valores.html', {"perfil":perfil, "areas_valores":areas_valores, "indicador":indicador } )
		else:
			return HttpResponseRedirect("/error/")

def llenar_dashboard(request, id):
	if request.user.is_authenticated() and request.user.is_active:
		perfil = PerfilUsuario.objects.get(usuario = request.user)
		indicador = Indicador.objects.get(id = id)
		indicador_valores = Indicador_Area.objects.filter(indicador = indicador) 

		if request.method == "POST":
			if 	request.POST.getlist('areas_valor[]') and request.POST.getlist('valor[]'):
				areas_valor = request.POST.getlist('areas_valor[]')
				valor = request.POST.getlist('valor[]')

				for i in range(len(areas_valor)):
					indicador_valor = get_object_or_404(indicador_valores, id = areas_valor[i])
					if len(str(valor[i])) == 0:
						indicador_valor.valor = None
					else: 
						indicador_valor.valor = valor[i]
					indicador_valor.save()

		if indicador.proveedor == perfil:
			return render(request, 'home/indicador/llenar_valores.html', {"perfil":perfil, "indicador_valores":indicador_valores} )
		else:
			return HttpResponseRedirect("/error/")

def mis_indicadores(request):
	if request.user.is_authenticated() and request.user.is_active:
		perfil = PerfilUsuario.objects.get(usuario = request.user)
		indicadores = Indicador.objects.filter(proveedor = perfil)
		return render(request, 'home/indicador/mis_indicadores.html', { "perfil":perfil, "indicadores":indicadores })
	else:
		return HttpResponseRedirect('/login/')

def dashboard_condensado(request):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		
		perfil = PerfilUsuario.objects.get(usuario = request.user)	
		indicadores = Indicador.objects.all()
		areas = Area.objects.all()
		
		tabla = [ [ "" for i in range(len(areas) + 3) ] for j in range(len(indicadores)) ]		
		col = 3
		row = 0
		for indicador in indicadores:			
			tabla[row][0] = '<td>' + str(indicador) + '</td>'
			tabla[row][1] = '<td>' + str(indicador.meta) + '</td>'
			if indicador.menor_igual:
				if indicador.meta > indicador.valor_institucional():
					tabla[row][2] = '<td class="success">' + str(indicador.valor_institucional()) + '</td>'
				elif indicador.meta == indicador.valor_institucional():
					tabla[row][2] = '<td class="warning">' + str(indicador.valor_institucional()) + '</td>'
				else:
					tabla[row][2] = '<td class="danger">' + str(indicador.valor_institucional()) + '</td>'
			else:
				if indicador.meta < indicador.valor_institucional():
					tabla[row][2] = '<td class="success">' + str(indicador.valor_institucional()) + '</td>'
				elif indicador.meta == indicador.valor_institucional():
					tabla[row][2] = '<td class="warning">' + str(indicador.valor_institucional()) + '</td>'
				else:
					tabla[row][2] = '<td class="danger">' + str(indicador.valor_institucional()) + '</td>'

 			for area in areas:
 				valor_indicador = Indicador_Area.objects.filter(area = area).filter(indicador = indicador)
 				if len(valor_indicador) >= 1: 
 					if indicador.menor_igual:
						if indicador.meta > valor_indicador[0].valor:
							tabla[row][col] = '<td class="success">' + str(valor_indicador[0].valor) + '</td>'
						elif indicador.meta == valor_indicador[0].valor:
							tabla[row][col] = '<td class="warning">' + str(valor_indicador[0].valor) + '</td>'
						else:
							tabla[row][col] = '<td class="danger">' + str(valor_indicador[0].valor) + '</td>'
					else:
						if indicador.meta < valor_indicador[0].valor:
							tabla[row][col] = '<td class="success">' + str(valor_indicador[0].valor) + '</td>'
						elif indicador.meta == valor_indicador[0].valor:
							tabla[row][col] = '<td class="warning">' + str(valor_indicador[0].valor) + '</td>'
						else:
							tabla[row][col] = '<td class="danger">' + str(valor_indicador[0].valor) + '</td>'
				else:
 					tabla[row][col] = "<td>&nbsp;</td>"
 				col += 1
 			row += 1
 			col = 3

		return render(request, 'home/condensado.html', {"perfil": perfil, "indicadores":indicadores, "areas":areas, "matriz":tabla})				
	else:
		return HttpResponseRedirect('/login/')	

def reporte_condensado(request):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		
		perfil = PerfilUsuario.objects.get(usuario = request.user)	
		indicadores = Indicador.objects.all()
		areas = Area.objects.all()
		
		response = HttpResponse(mimetype="application/ms-excel")
		response['Content-Disposition'] = 'attachment; filename=%s' % "Reporte_Condensado.xls"

		xls = Workbook()
		doc = xls.add_sheet('Indicadores')
		doc.write(0, 0, "Indicador")
		doc.write(0, 1, "Meta")	
		doc.write(0, 2, "Institucional")
		col = 3
		for area in areas:
			doc.write(0, col, str(area))
			col += 1
		col = 3
		row = 1
		for indicador in indicadores:
			doc.write(row, 0, unicode(indicador))
			doc.write(row, 1, indicador.meta)
			doc.write(row, 2, indicador.valor_institucional())

			for area in areas:
				valor_indicador = Indicador_Area.objects.filter(area = area).filter(indicador = indicador)
				if len(valor_indicador) >= 1:
					doc.write(row, col, valor_indicador[0].valor)
				else:
					doc.write(row, col, "")			
				col += 1
			row += 1 
			col = 3
		xls.save(response)
		return response
	else:
		return HttpResponseRedirect('/login/')	
#############################################
###### Areas ################################
#############################################

def lista_area(request):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		perfil = PerfilUsuario.objects.get(usuario = request.user)
		areas = Area.objects.all()
		return render(request, 'home/area/index.html', {"areas":areas, "perfil":perfil })
	else:
		return HttpResponseRedirect('/login/')

def nueva_area(request):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		perfil = PerfilUsuario.objects.get(usuario = request.user)
		if request.method == 'POST':
			formulario = AreaForm(request.POST)
			if formulario.is_valid():
				area = formulario.save(commit=False)
				area.save()				
				return HttpResponseRedirect('/area/')
		else:
			formulario = AreaForm()
		return render(request, 'home/formulario_nuevo_editar.html', {"formulario":formulario, "perfil":perfil})
	else:
		return HttpResponseRedirect('/login/')

def editar_area(request, id):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		area = get_object_or_404(Area, id = id)
		perfil = PerfilUsuario.objects.get(usuario = request.user)
		if request.method == 'POST':

			formulario = AreaForm(request.POST, instance=area)
			if formulario.is_valid():
				formulario.save()
				return HttpResponseRedirect('/area/')
		else:
			formulario = AreaForm(instance=area)
		return render(request, 'home/formulario_nuevo_editar.html', {'formulario':formulario, "perfil":perfil})
	else:
		return HttpResponseRedirect('/login/')

def borrar_area(request, id):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		area = get_object_or_404(Area, id = id)
		area.delete()
		return HttpResponseRedirect('/area/')
	else:
		return HttpResponseRedirect('/login/')

def asignar_area(request, id):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		perfil = PerfilUsuario.objects.get(usuario = request.user)		
		if request.method == 'POST':
			area = get_object_or_404(Area, id = id)
			
			for element in Indicador_Area.objects.filter(area=area):
				element.delete()

			if 	request.POST.getlist('indicadores'):
				indicadores = request.POST.getlist('indicadores')

				for indicador in indicadores:				
					indi_ar = Indicador_Area()
					indi_ar.area = area
					indi_ar.indicador = get_object_or_404(Indicador, id=indicador)
					indi_ar.save()
			return HttpResponseRedirect('/area/')

		else:
			formulario = AsignarIndicadorForm(area = id)

		return render(request, 'home/formulario_nuevo_editar.html', {'formulario':formulario, "perfil":perfil}) 
		
	else:
		return HttpResponseRedirect('/login/')
##############################################
###### Sector ################################
##############################################

def lista_sector(request):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		sectores = Sector.objects.all()
		perfil = PerfilUsuario.objects.get(usuario = request.user)
		return render(request, 'home/sector/index.html', {"sectores":sectores, "perfil":perfil})
	else:
		return HttpResponseRedirect('/login/')

def nuevo_sector(request):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		perfil = PerfilUsuario.objects.get(usuario = request.user)
		if request.method == 'POST':
			formulario = SectorForm(request.POST)
			if formulario.is_valid():
				sector = formulario.save(commit=False)
				sector.save()
				return HttpResponseRedirect("/sector/")
		else:
			formulario = SectorForm()
		return render(request, 'home/formulario_nuevo_editar.html', {"formulario":formulario, "perfil":perfil})
	else:
		return HttpResponseRedirect('/login/')

def editar_sector(request, id):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		sector = get_object_or_404(Sector, id = id)
		perfil = PerfilUsuario.objects.get(usuario = request.user)
		if request.method == 'POST':
			formulario = SectorForm(request.POST, instance=sector)
			if formulario.is_valid():
				formulario.save()
				return HttpResponseRedirect("/sector/")
		else:
			formulario = SectorForm(instance=sector)
		return render(request, 'home/formulario_nuevo_editar.html', {"formulario":formulario, "perfil":perfil})
	else:
		return HttpResponseRedirect('/login/')

def borrar_sector(request, id):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		sector = get_object_or_404(Sector, id = id)
		sector.delete()
		return HttpResponseRedirect("/sector")
	else:
		return HttpResponseRedirect("/login/")
##############################################
####### Usuario ##############################
##############################################

def lista_usuario(request):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		perfil = PerfilUsuario.objects.get(usuario = request.user)
		usuarios = PerfilUsuario.objects.all()
		return render(request, 'home/usuario/index.html', {"usuarios":usuarios, "perfil":perfil})
	else:
		return HttpResponseRedirect('/login/')

def nuevo_usuario(request):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		perfil = PerfilUsuario.objects.get(usuario = request.user)
		if request.method == 'POST':
			formulario = PerfilUsuarioForm(request.POST)
			#formulario = UserForm(request.POST)
			if formulario.is_valid():
				usuario = formulario.save(commit=False)
				usuario.save()
				return HttpResponseRedirect("/usuario/")
		else:
			#formulario = UserForm()
			formulario = PerfilUsuarioForm()
		return render(request, 'home/formulario_nuevo_editar.html', {"formulario":formulario, "perfil":perfil})
	else:
		return HttpResponseRedirect('/login/')

def admin_usuario(request, id):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		perfil_usuario = get_object_or_404(PerfilUsuario, id = id)
		perfil_usuario.admin = not perfil_usuario.admin
		perfil_usuario.save();
		return HttpResponseRedirect("/usuario/")
	else:
		return HttpResponseRedirect("/dashboard/")

def habilitar_usuario(request, id):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		usuario = get_object_or_404(AuthUser, id = id)
		usuario.is_active = not usuario.is_active
		usuario.save()
		return HttpResponseRedirect("/usuario/")
	else:
		return HttpResponseRedirect("/dashboard/")

def borrar_usuario(request, id):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		usuario = get_object_or_404(AuthUser, id = id)
		perfil  = get_object_or_404(PerfilUsuario, id = id)
		perfil.delete()
		usuario.delete()
		return HttpResponseRedirect("/usuario/")
	else:
		return HttpResponseRedirect("/dashboard/")

def pass_usuario(request):
	if request.user.is_authenticated() and request.user.is_active:
		perfil = PerfilUsuario.objects.get(usuario = request.user)		
		if request.method == "POST":
			usuario = AuthUser.objects.get(id=perfil.usuario.id)
			password = request.POST['contrasenia']
			usuario.set_password(password)
			usuario.save()
			auth_logout(request)
			return HttpResponseRedirect('/login/')
		else:
			return render(request, 'home/usuario/cambio_pass.html', { "perfil":perfil })
	else:
		return HttpResponseRedirect('/login/')

##############################################
####### Indicador ############################
##############################################

def lista_indicador(request):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		perfil = PerfilUsuario.objects.get(usuario = request.user)
		indicadores = Indicador.objects.all()
		return render(request, 'home/indicador/index.html', {"indicadores":indicadores, "perfil":perfil})
	else:
		return HttpResponseRedirect('/login/')

def nuevo_indicador(request):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		perfil = PerfilUsuario.objects.get(usuario = request.user)
		if request.method == 'POST':
			formulario = IndicadorForm(request.POST)
			if formulario.is_valid():
				indicador = formulario.save(commit=False)
				indicador.save()
				return HttpResponseRedirect('/indicador/')
		else:
			formulario = IndicadorForm()
		return render(request, 'home/formulario_nuevo_editar.html', {"formulario":formulario, "perfil":perfil})

def editar_indicador(request, id):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		perfil = PerfilUsuario.objects.get(usuario = request.user)
		indicador = get_object_or_404(Indicador, id = id)
		if request.method == 'POST':
			formulario = IndicadorForm(request.POST, instance = indicador)
			if formulario.is_valid():
				formulario.save()
				return HttpResponseRedirect('/indicador/')
		else:
			formulario = IndicadorForm(instance = indicador)
		return render(request, 'home/formulario_nuevo_editar.html', {'formulario':formulario, "perfil":perfil})
	else:
		return HttpResponseRedirect('/login/')

def borrar_indicador(request, id):
	if request.user.is_authenticated() and request.user.is_active:# and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		indicador = get_object_or_404(Indicador, id = id)
		indicador.delete()
		return HttpResponseRedirect('/indicador/')
	else:
		return  HttpResponseRedirect('/login/')

def asignar_indicador(request, id):
	if request.user.is_authenticated() and request.user.is_active and PerfilUsuario.objects.get(usuario = request.user).admin == True:
		perfil = PerfilUsuario.objects.get(usuario = request.user)		
		if request.method == 'POST':
			indicador = get_object_or_404(Indicador, id = id)
			
			for element in Indicador_Area.objects.filter(indicador=indicador):
				element.delete()

			if 	request.POST.getlist('areas'):
				areas = request.POST.getlist('areas')

				for area in areas:				
					indi_ar = Indicador_Area()
					indi_ar.area = get_object_or_404(Area, id=area)
					indi_ar.indicador = indicador
					indi_ar.save()
			return HttpResponseRedirect('/indicador/')

		else:
			formulario = AsignarAreaForm(indicador = id)

		return render(request, 'home/formulario_nuevo_editar.html', {'formulario':formulario, "perfil":perfil}) 
		
	else:
		return HttpResponseRedirect('/login/')
##############################################
#######

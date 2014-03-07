from django.conf.urls import patterns, include, url
from django.conf import settings
from django.conf.urls.static import static
from django.contrib import admin
from home import views

admin.autodiscover()

urlpatterns = patterns('',
    # Examples:
    # url(r'^$', 'indicadores.views.home', name='home'),
    # url(r'^blog/', include('blog.urls')),

    ## Staff Login ##
    url(r'^admin/'		, include(admin.site.urls)),

    ## Login y Usuarios ##
    url(r'^login/$'		, views.login, name="login"),
    url(r'^logout/$'	, views.logout, name="logout"),
    url(r'^registro/$'	, views.registro_usuario, name="registro_usuario"),

    ## Usuarios ##
    url(r'^usuario/$'   , views.lista_usuario, name="lista_usuario" ),
    url(r'^usuario/nuevo/$', views.nuevo_usuario, name="nuevo_usuario"),
    url(r'^usuario/(?P<id>[0-9]+)/borrar/$', views.borrar_usuario, name="borrar_usuario"),
    url(r'^usuario/(?P<id>[0-9]+)/habilitar/$', views.habilitar_usuario, name="habilitar_usuario"),
    url(r'^usuario/(?P<id>[0-9]+)/admin/$', views.admin_usuario, name="admin_usuario"),


    ## Dashboars ##
    url(r'^dashboard/$'	, views.dashboard, name="dashboard"),
    url(r'^$', views.dashboard, name="home"),

    ## Sector ##
    url(r'^sector/$'    , views.lista_sector, name="lista_sector"),
    url(r'^sector/nuevo/$', views.nuevo_sector, name="nuevo_sector"),
    url(r'^sector/(?P<id>[0-9]+)/editar/$', views.editar_sector, name="editar_sector"),
    url(r'^sector/(?P<id>[0-9]+)/borrar/$', views.borrar_sector, name="borrar_sector"),

    ## Area ##
    url(r'^area/$', views.lista_area, name = "lista_area"),
    url(r'^area/nueva/' , views.nueva_area, name="nueva_area"),
    url(r'^area/(?P<id>[0-9]+)/editar/$', views.editar_area, name="editar_area"),
    url(r'^area/(?P<id>[0-9]+)/borrar/$', views.borrar_area, name="borrar_area"),
    url(r'^area/(?P<id>[0-9]+)/asignar/$', views.asignar_area, name="asignar_area"),

    ## Indicador ##
    url(r'^mis_indicadores/$', views.mis_indicadores, name = "mis_indicadores"),
    url(r'^indicador/$', views.lista_indicador, name = "lista_indicador"),
    url(r'^indicador/nuevo/', views.nuevo_indicador, name = "nuevo_indicador"),
    url(r'^indicador/(?P<id>[0-9]+)/editar/$', views.editar_indicador, name = "editar_indicador"),
    url(r'^indicador/(?P<id>[0-9]+)/borrar/$', views.borrar_indicador, name = "borrar_indicador"),
    url(r'^indicador/(?P<id>[0-9]+)/asignar/$', views.asignar_indicador, name = "asignar_indicador"),


) + static(settings.STATIC_URL, document_root = settings.STATIC_ROOT)

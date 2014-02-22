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
    
    ## Dashboars ##
    url(r'^dashboard/'	, views.dashboard, name="dashboard"),
    url(r'^$', views.dashboard, name="home"),

    ## Sector ##
    url(r'^sector/$', views.lista_sector, name="lista_sector"),
    url(r'^sector/nuevo/$', views.nuevo_sector, name="nuevo_sector"),
    url(r'^sector/(?P<pk>[0-9]+)/editar/$', views.editar_sector, name="editar_sector"),
    url(r'^sector/(?P<pk>[0-9]+)/borrar/$', views.borrar_sector, name="borrar_sector"),

    ## Area ##
    url(r'^area/$', views.lista_area, name = "lista_area"),
    url(r'^area/nueva/' , views.nueva_area, name="nueva_area"),
    url(r'^area/(?P<pk>[0-9]+)/editar/$', views.editar_area, name="editar_area"),    
    url(r'^area/(?P<pk>[0-9]+)/borrar/$', views.borrar_area, name="borrar_area"),

) + static(settings.STATIC_URL, document_root = settings.STATIC_ROOT)

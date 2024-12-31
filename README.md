# Seneca Test

Este proyecto contiene el test enviado

- Para poder ejecutar el proyecto debemos crear una base de datos y correr los scripts del archivo base.sql
- Dentro del archivo appsettings.json tenemos "SQLConnection" se debe colocar el servidor de base de datos SQL ejemplo: "Server=IP-BASE-DATOS;Database=NOMBRE-BASE-DATOS", solo con cambiar estos parámetros se tendrá la conexión a la base de datos.
- Para que funcione el envio de correos tenemos el archivo Mailjet.txt aqui estan las claves para el API de mailing, reemplazar los valores de "ApiKey", "ApiSecret" en el archivo appsettings.json.
- Dentro del appsettings.json tenemos "Url", esta es la ruta local del proyecto aqui se deberá colocar la ruta que se da al ejecutar el proyecto ejemplo: "http://localhost:5298", esta variable sirve para la redirección del correo hacia el proyecto.


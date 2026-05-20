#!/bin/bash

echo "Encendiendo Colima..."
colima start

echo "Encendiendo el contenedor de SQL Server..."
docker start sqlserver

echo "¡Listo! Colima y SQL Server han sido encendidos."

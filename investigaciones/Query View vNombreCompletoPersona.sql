-- CREATE VIEW [Esquema].[NombreVista] AS


create view Person.vNombreCompletoPersona as
select BusinessEntityID as ID,  FirstName + ' ' + MiddleName  + ' ' + LastName as NombreCompleto 
from Person.Person 
where MiddleName is not null


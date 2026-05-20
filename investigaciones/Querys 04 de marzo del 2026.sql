SELECT * FROM [Sales].[vStoreWithDemographics]
select * from Sales.vSalesPerson


select * from HumanResources.Employee

select * from HumanResources.vEmployee

select * from Person.Person

select BusinessEntityID as ID,  FirstName + ' ' + MiddleName  + ' ' + LastName as NombreCompleto 
from Person.Person 
where MiddleName is not null
order by ID

select * from [Person].[vNombreCompletoPersona]
order by ID


Select 
strMailingCity,
strMailingState,
(select top(1) CityName from City where CityName = strMailingCity) as City	

From [TAHR-CSIF].dbo.tblEmployeeContact

where 
(select top(1) CityName from City where CityName = strMailingCity) is null
--strMailingState <> 'PR'
union 
Select 
strHomeCity,
strHomeState,
(select top(1) CityName from City where CityName = strHomeCity) as City	

From [TAHR-CSIF].dbo.tblEmployeeContact

where 
(select top(1) CityName from City where CityName = strHomeCity) is null
--strHomeState <> 'PR'
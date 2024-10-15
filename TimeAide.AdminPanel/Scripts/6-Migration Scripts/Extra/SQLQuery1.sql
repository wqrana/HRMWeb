Select 
strHomeCity,
strHomeState,
(select top(1) CityName from City where CityName = strHomeCity) as City	

From [TAHR-CSIF].dbo.tblEmployeeContact

where 
--(select top(1) CityName from City where CityName = strHomeCity) is null
strHomeState <> 'PR'





Select 
(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID) as strEmpID	

From [TAHR-CSIF].dbo.tblEmployeeContact

where 
(select top(1) UserInformationId from UserInformation where EmployeeId = strEmpID) is null

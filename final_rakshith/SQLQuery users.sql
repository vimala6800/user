select* from AspNetRoles;
select * from AspNetUsers;
select * from AspNetUserRoles;
select * from AspNetUsers 
select AspNetUsers.UserName,AspNetRoles.Name as Name from AspNetUsers cross join AspNetUserRoles cross join AspNetRoles where AspNetUserRoles.UserId=AspNetUsers.Id
select distinct AspNetUsers.Id as UserId,AspNetUsers.UserName as Name, AspNetRoles.Id as RoleId from AspNetUsers cross join AspNetUserRoles cross join AspNetRoles where AspNetUserRoles.UserId=AspNetUsers.Id
--6977285d-cf0c-4bba-b4dd-221fd1aa611a

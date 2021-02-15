/* 
* BEGIN 
* Author : ZLI
* DATE : 14/02/2021 
* Description : Add user role
*/

  IF NOT EXISTS (SELECT * FROM [AspNetRoles])
  BEGIN
	INSERT INTO [AspNetRoles](Name, NormalizedName, Label)
	VALUES('Finance','FINANCE',N'财务')

	INSERT INTO [AspNetRoles](Name, NormalizedName, Label)
	VALUES('SuperAdmin','SUPERADMIN', N'超级管理员')

	INSERT INTO [AspNetRoles](Name, NormalizedName, Label)
	VALUES('Employee','EMPLOYEE', N'普通员工')

  END
  GO	
/* 
* END 
* Author : ZLI
* DATE : 14/02/2021 
* Description : Add user role
*/
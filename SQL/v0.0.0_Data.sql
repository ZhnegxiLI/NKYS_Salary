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


/* 
* START 
* Author : ZLI
* DATE : 16/02/2021 
* Description : Add salary search export 
*/
IF NOT EXISTS (SELECT * FROM ExportConfiguration WHERE ExportName = 'SalariesWorkingHours')
BEGIN
	INSERT INTO [ExportConfiguration]([ExportName], [ExportModel])
	VALUES('SalariesWorkingHours',N'  [
			{
			"Name": "CycleName",
			"DisplayName": "周期",
			"Order" : 1
			},{
			"Name": "DepartmentName",
			"DisplayName": "部门",
			"Order" : 2
			},{
			"Name": "GroupName",
			"DisplayName": "车间",
			"Order" : 3
			},{
			"Name": "EmployeeName",
			"DisplayName": "员工",
			"Order" : 4
			},{
			"Name": "WorkingHours",
			"DisplayName": "总工时",
			"Order" : 5
			},{
			"Name": "WorkingHoursDay",
			"DisplayName": "日班工时",
			"Order" : 6
			},{
			"Name": "WorkingHoursNight",
			"DisplayName": "夜班工时",
			"Order" : 7
			},{
			"Name": "WorkingHoursHoliday",
			"DisplayName": "假日工时",
			"Order" : 8
			},
			{
			"Name": "WorkingScore",
			"DisplayName": "分数",
			"Order" : 9
			},
			{
			"Name": "AbsentHours",
			"DisplayName": "缺勤时间",
			"Order" : 10
			},
			{
			"Name": "DeferredHolidayHours",
			"DisplayName": "可补休工时",
			"Order" : 11
			},
			{
			"Name": "WorkingDays",
			"DisplayName": "上班天数",
			"Order" : 12
			},
			{
			"Name": "DormFee",
			"DisplayName": "住宿费用",
			"Order" : 13
			},
			{
			"Name": "DormOtherFee",
			"DisplayName": "水,电费用",
			"Order" : 14
			},
			{
			"Name": "OtherRewardFee",
			"DisplayName": "奖励费用",
			"Order" : 15
			},
			{
			"Name": "OtherPenaltyFee",
			"DisplayName": "惩罚费用",
			"Order" : 16
			}
		]')
END
GO
/* 
* END 
* Author : ZLI
* DATE : 16/02/2021 
* Description : Add salary search export 
*/
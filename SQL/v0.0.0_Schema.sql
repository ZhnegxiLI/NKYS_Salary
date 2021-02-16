/* 
* BEGIN 
* Author : ZLI
* DATE : 23/12/2020 
* Description : FN_CalculRevenueTax
*/
IF EXISTS (SELECT * FROM   sys.objects WHERE  object_id = OBJECT_ID(N'[dbo].[FN_CalculRevenueTax]') AND type IN ( N'FN', N'IF', N'TF', N'FS', N'FT' ))
    DROP FUNCTION [dbo].[FN_CalculRevenueTax]
GO 

CREATE FUNCTION [dbo].[FN_CalculRevenueTax](
       @NetRevenue DECIMAL(18,2)
    )
    RETURNS DECIMAL(18,2)
AS
    BEGIN
        DECLARE @Tax DECIMAL(18,2) = 0 --应缴税额

		DECLARE @TaxRate DECIMAL(18,2) = 0 -- 所在税率
		DECLARE @QuickDeduction DECIMAL(18,2) = 0 -- 速算扣除数

		DECLARE @TaxableIncome DECIMAL(18,2) = @NetRevenue - 5000 -- 可交税部分

		IF @TaxableIncome >=0 AND @TaxableIncome < 3000 
		BEGIN
			SET @QuickDeduction = 0
			SET @TaxRate = 0.03
		END
		ELSE IF @TaxableIncome >=3000 AND @TaxableIncome < 12000 
		BEGIN
			SET @QuickDeduction = 210
			SET @TaxRate = 0.1
		END
		ELSE IF @TaxableIncome >=12000 AND @TaxableIncome < 25000 
		BEGIN
			SET @QuickDeduction = 1410
			SET @TaxRate = 0.2
		END
		ELSE IF @TaxableIncome >=25000 AND @TaxableIncome < 35000 
		BEGIN
			SET @QuickDeduction = 2660
			SET @TaxRate = 0.25
		END
		ELSE IF @TaxableIncome >=35000 AND @TaxableIncome < 55000 
		BEGIN
			SET @QuickDeduction = 4410
			SET @TaxRate = 0.3
		END
		ELSE IF @TaxableIncome >=55000 AND @TaxableIncome < 80000 
		BEGIN
			SET @QuickDeduction = 7160
			SET @TaxRate = 0.35
		END
		ELSE IF @TaxableIncome >=80000 
		BEGIN
			SET @QuickDeduction = 15160
			SET @TaxRate = 0.45
		END

		SET @Tax = @TaxableIncome*@TaxRate - @QuickDeduction
		IF @Tax <0
		BEGIN
			SET @Tax = 0
		END
		Return @Tax
    END

GO 
/* 
* END 
* Author : ZLI
* DATE : 23/12/2020 
* Description : FN_CalculRevenueTax
*/

/* 
* BEGIN 
* Author : ZLI
* DATE : 22/12/2020 
* Description : SP_CalculSalaries
*/
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'SP_CalculSalaries')
                    AND type IN ( N'P', N'PC' ) ) 
BEGIN
    DROP PROCEDURE [dbo].[SP_CalculSalaries]
END
GO

CREATE PROCEDURE SP_CalculSalaries(@CycleId BIGINT, @UserId BIGINT, @EmployeeId BIGINT, @IsUpdate BIT ) 
AS
BEGIN
	DECLARE @CalculStartTime DATETIME = GETDATE()
	/* Step 1: Create temp employee table */
	DROP TABLE IF EXISTS #tempEmployees
	CREATE TABLE #tempEmployees(EmployeId BIGINT, ExternalId BIGINT, TechnicalLevel DECIMAL(18,2), HasTransportFee BIT, DormFee DECIMAL(18,2), SocialSercurityFee DECIMAL(18,2),
	HousingReservesFee DECIMAL(18,2), SelfPaySocialSercurityFee DECIMAL(18,2),PositionPay DECIMAL(18,2), SeniorityPay DECIMAL(18,2), FixSalary DECIMAL(18,2), IsTemporaryEmploye BIT
	, GroupId BIGINT, SharePropotion DECIMAL(18,4), IsFixSalary BIT, GroupProductionValueTypeId INT , GroupVariableSharePropotion  DECIMAL(18,4))

	/* Step 2: Fill employee information according to Employe and Groups table (if @EmployeId is sent, calcul only the specific employe) */
	INSERT INTO #tempEmployees(EmployeId, ExternalId, TechnicalLevel, HasTransportFee, DormFee, SocialSercurityFee, HousingReservesFee,
	SelfPaySocialSercurityFee, PositionPay, SeniorityPay, FixSalary, IsTemporaryEmploye, GroupId,  IsFixSalary, SharePropotion, GroupProductionValueTypeId, GroupVariableSharePropotion)
	SELECT E.Id , E.ExternalId, E.TechnicalLevel, E.HasTransportFee, E.DormFee, E.SocialSercurityFee,  E.HousingReservesFee, E.SelfPaySocialSercurityFee,
	E.PositionPay, E.SeniorityPay, E.FixSalary, e.IsTemporaryEmploye, E.GroupsId, G.IsFixSalary, G.SharePropotion,  G.ProductionValueTypeId, G.GroupVariableSharePropotion
	FROM Employe E
	INNER JOIN Groups G ON E.GroupsId = G.Id
	WHERE DepartDate IS NULL AND (@EmployeeId IS NULL OR E.Id = @EmployeeId)

	/* Step 3: Create temp salary table (for display and final update) */
	DROP TABLE IF EXISTS #tempSalaries
	CREATE TABLE #tempSalaries(SalaryId BIGINT, CycleId BIGINT, CycleLabel NVARCHAR(50) , EmployeId BIGINT, GroupId BIGINT, GroupLabel NVARCHAR(50), EmployeName NVARCHAR(50), 
	WorkingHoursDay DECIMAL(18,2) default 0, WorkingHoursNight DECIMAL(18,2) default 0, WorkingHoursHoliday DECIMAL(18,2) default 0,WorkingHours DECIMAL(18,2) default 0, 
	WorkingScore DECIMAL(18,2) default 0,AbsentHours DECIMAL(18,2) default 0,DeferredHolidayHours DECIMAL(18,2) default 0,WorkingDays DECIMAL(18,2) default 0, CycleStandardWorkingHours DECIMAL(18,2) default 0, CycleStandardWorkingDays DECIMAL(18,2) default 0,
	AbsentDeduct DECIMAL(18,2) default 0, OvertimePay DECIMAL(18,2) default 0,  SocialSercurityFee DECIMAL(18,2) default 0, SelfPaySocialSercurityFee DECIMAL(18,2) default 0,HousingReservesFee DECIMAL(18,2) default 0, 
	OtherRewardFee DECIMAL(18,2) default 0,OtherPenaltyFee DECIMAL(18,2) default 0,  FullPresencePay DECIMAL(18,2) default 0, SeniorityPay DECIMAL(18,2) default 0, TransportFee DECIMAL(18,2) default 0, DormFee DECIMAL(18,2) default 0,
	DormOtherFee DECIMAL(18,2) default 0, PositionPay DECIMAL(18,2) default 0,   
	Salary_FixPart DECIMAL(18,2) default 0, Salary_VariablePart DECIMAL(18,2) default 0, BasicSalary DECIMAL(18,2), NetSalary DECIMAL(18,2), SalaryTax DECIMAL(18,2), FinalSalary DECIMAL(18,2))

	/* Step 4: Fill the temp salary table according to the pre-insert data */
	IF @CycleId IS NOT NULL
	BEGIN
		INSERT INTO #tempSalaries(SalaryId ,CycleId, EmployeId, GroupId, WorkingHoursDay,WorkingHoursNight, WorkingHoursHoliday, WorkingScore, OtherRewardFee, OtherPenaltyFee, FullPresencePay, TransportFee, E.DormFee, DormOtherFee, CycleStandardWorkingHours)
		SELECT DISTINCT S.Id, s.CycleId, E.EmployeId, E.GroupId, ISNULL(S.WorkingHoursDay,0), ISNULL(S.WorkingHoursNight,0), ISNULL(S.WorkingHoursHoliday,0), ISNULL(S.WorkingScore,0), ISNULL(OtherRewardFee,0), ISNULL(OtherPenaltyFee,0), ISNULL(FullPresencePay,0), ISNULL(TransportFee,0), ISNULL(E.DormFee,0), ISNULL(DormOtherFee,0), ISNULL(C.StandardWorkingHours,0)
		FROM Salary S 
		INNER JOIN #tempEmployees E ON S.EmployeId = E.EmployeId
		INNER JOIN Cycle C ON S.CycleId = C.Id
		WHERE S.CycleId = @CycleId
	END

	UPDATE S
	SET S.WorkingHours = S.WorkingHoursDay + S.WorkingHoursNight + S.WorkingHoursHoliday
	FROM #tempSalaries S 

	/* Step 5: Update salary information according to temp employee table */
	/* Step 5-1:  Update AbsentHours */
	UPDATE S
	SET S.AbsentHours = S.WorkingHours - S.CycleStandardWorkingHours
	FROM #tempSalaries S 
	INNER JOIN #tempEmployees E ON S.EmployeId = E.EmployeId
	WHERE S.WorkingHours - S.CycleStandardWorkingHours <0

	/* Step 5-2:  Update DeferredHolidayHours */
	UPDATE S
	SET S.DeferredHolidayHours = S.WorkingHours - S.CycleStandardWorkingHours
	FROM #tempSalaries S 
	INNER JOIN #tempEmployees E ON S.EmployeId = E.EmployeId
	WHERE S.WorkingHours - S.CycleStandardWorkingHours >0

	/* Step 5-3:  Update SocialSercurityFee, SelfPaySocialSercurityFee, HousingReservesFee */
	UPDATE S
	SET S.SocialSercurityFee = ISNULL(E.SocialSercurityFee,0), S.SelfPaySocialSercurityFee = ISNULL(E.SelfPaySocialSercurityFee,0), S.HousingReservesFee = ISNULL(E.HousingReservesFee,0)
	FROM #tempSalaries S 
	INNER JOIN #tempEmployees E ON S.EmployeId = E.EmployeId

	
	/* Step 5-4:  Update SeniorityPay , PositionPay */
	UPDATE S
	SET S.SeniorityPay = ISNULL(S.SeniorityPay,0),  S.PositionPay = ISNULL(E.PositionPay,0)
	FROM #tempSalaries S 
	INNER JOIN #tempEmployees E ON S.EmployeId = E.EmployeId


	/* Step 5-5:  Update DormFee (单月住宿费用为100) */
	UPDATE S
	SET S.DormFee = E.DormFee
	FROM #tempSalaries S 
	INNER JOIN #tempEmployees E ON S.EmployeId = E.EmployeId
	WHERE E.DormFee IS NOT NULL 

	/* Step 5-6:  Update WorkingDays (每日应工作时长为7.5, 日班工时/7.5 = 出勤天数) */
	UPDATE S
	SET S.WorkingDays = CAST ((S.WorkingHoursDay / 7.5) AS decimal(18,2)), S.CycleStandardWorkingDays = CAST ((S.CycleStandardWorkingHours / 7.5) AS decimal(18,2))
	FROM #tempSalaries S 
	INNER JOIN #tempEmployees E ON S.EmployeId = E.EmployeId


	/* Step 5-7:  Update TransportFee (每日通勤费用为10) */
	-- todo: 确认计算天数时向下取整
	UPDATE S
	SET S.TransportFee = CAST (S.WorkingHoursDay/7.5 AS INT) * 10
	FROM #tempSalaries S 
	INNER JOIN #tempEmployees E ON S.EmployeId = E.EmployeId
	WHERE E.HasTransportFee = 1

	/* Step 5-8: Fix salary employee */
	UPDATE S
	SET S.Salary_FixPart = ISNULL(E.FixSalary,0)
	,S.AbsentDeduct = CAST( ((E.FixSalary/(S.CycleStandardWorkingDays*7.5)) * S.AbsentHours) AS DECIMAL(18,2))
	,S.OvertimePay = CAST( ((E.FixSalary/(S.CycleStandardWorkingDays*7.5)) * S.DeferredHolidayHours) AS DECIMAL(18,2))
	FROM #tempSalaries S 
	INNER JOIN #tempEmployees E ON S.EmployeId = E.EmployeId
	WHERE E.IsFixSalary = 1 
	/* TODO Step 5-8bis: Fix salary employee (variable part) */

	/* Create a temp table to calcul fix salary  employee variable part */
	DROP TABLE IF EXISTS #tempFixedSalaryDeduction 
	CREATE TABLE #tempFixedSalaryDeduction(EmployeId BIGINT, CycleId BIGINT, Value DECIMAL(18,2) DEFAULT 0)
	INSERT INTO #tempFixedSalaryDeduction(EmployeId, CycleId, Value)
	SELECT S.EmployeId, S.CycleId, SUM(PV.Value* EC.DeductionSharePropotion)
	FROM #tempSalaries S
	INNER JOIN EmployeDeductionConfiguration EC ON S.EmployeId = EC.EmployeId 
	INNER JOIN ProductionValue PV ON PV.CycleId = S.CycleId AND EC.LinkedProductionValueTypeId = PV.ProductionValueTypeId
	GROUP BY S.CycleId, S.EmployeId

	/* Update principal table */
	UPDATE S
	SET S.Salary_VariablePart = SD.Value
	FROM #tempSalaries S 
	INNER JOIN #tempEmployees E ON S.EmployeId = E.EmployeId
	INNER JOIN #tempFixedSalaryDeduction SD ON E.EmployeId = SD.EmployeId AND S.CycleId = SD.CycleId
	WHERE E.IsFixSalary = 1 

	/* Module production value based salary(new version) */
	-- We need to create a temp table here to make the debug esaier
	/* Step 1: Create a temp table to calcul production value based salary */
	DROP TABLE IF EXISTS #tempProductionValueBasedSalary
	CREATE TABLE #tempProductionValueBasedSalary(SalaryId BIGINT, CycleId BIGINT, EmployeId BIGINT,  WorkingScore DECIMAL(18,2), WorkingHours DECIMAL(18,2),
	TechnicalLevel DECIMAL(18,2), Fix_EmployeHoursScore DECIMAL(18,2),
    GroupProductionValue DECIMAL(18,2), --小组可分配总值
	Fix_GroupAverageHour DECIMAL(18,2), Fix_GroupAverageHourScore DECIMAL(18,2),Fix_GroupTotalHourScore DECIMAL(18,2), Fix_GroupDelta DECIMAL(18,2),
	GroupVariableSalary DECIMAL(18,2), -- 小组浮动可分配总值
	Variable_GroupTotalScore DECIMAL(18,2) ,Variable_GroupDelta DECIMAL(18,2),  GroupVariableSharePropotion DECIMAL(18,2))

	/* Step 2: filter the targeted salary, and bind some basic information */
	INSERT INTO #tempProductionValueBasedSalary(SalaryId, CycleId, EmployeId,   GroupProductionValue, WorkingScore, WorkingHours, GroupVariableSharePropotion, TechnicalLevel)
	SELECT S.SalaryId, S.CycleId, S.EmployeId,  (PV.Value * E.SharePropotion) , S.WorkingScore, S.WorkingHours, E.GroupVariableSharePropotion, E.TechnicalLevel
	FROM #tempSalaries S
	INNER JOIN #tempEmployees E ON S.EmployeId = E.EmployeId
	INNER JOIN ProductionValue PV ON PV.CycleId = S.CycleId AND PV.ProductionValueTypeId = E.GroupProductionValueTypeId
	WHERE E.IsFixSalary != 1 AND E.TechnicalLevel IS NOT NULL AND E.SharePropotion IS NOT NULL

	/* Step 2bis: Calcul employe HoursScore(In new version TechnicalLevel * WorkingHours, In old version WorkingHours * WorkingScore )*/
	UPDATE PS
	SET PS.Fix_EmployeHoursScore = PS.WorkingHours * PS.TechnicalLevel
	FROM #tempProductionValueBasedSalary PS


	/* Step 3: Create a new temp table Calcul group information: totalHoursScore, AverageWorkingHours .... */
	DROP TABLE IF EXISTS #tempGroupHourScore

	CREATE TABLE #tempGroupHourScore(GroupId BIGINT, Fix_GroupTotalHourScore DECIMAL(18,2), NumberOfGroupMember INT,  Fix_GroupAverageHour DECIMAL(18,2), Fix_GroupAverageHourScore DECIMAL(18,2),
	Variable_GroupTotalScore DECIMAL(18,2))
	INSERT INTO #tempGroupHourScore(GroupId, Fix_GroupTotalHourScore, NumberOfGroupMember, Fix_GroupAverageHour, Fix_GroupAverageHourScore, Variable_GroupTotalScore)
	SELECT E.GroupsId, SUM(S.WorkingHours * E.TechnicalLevel) AS N'总工时', COUNT(E.Id) AS N'部门人数', (SUM(S.WorkingHours)/ COUNT(E.Id)) AS N'平均工时', (ISNULL(G.GroupVariableSharePropotion,1) *(SUM(S.WorkingHours)/ COUNT(E.Id))) N'平均工时分', SUM(S.WorkingScore)  AS '总分'
	FROM Employe E 
	INNER JOIN #tempProductionValueBasedSalary S ON E.Id = S.EmployeId
	INNER JOIN Groups G ON E.GroupsId = G.Id
	WHERE G.SharePropotion IS NOT NULL AND G.ProductionValueTypeId IS NOT NULL
	GROUP BY E.GroupsId, G.GroupVariableSharePropotion

	/* Step 3bis: Add the total variable salary's HoursScore into the totalHourScore */
	UPDATE #tempGroupHourScore
	SET Fix_GroupTotalHourScore = Fix_GroupTotalHourScore + Fix_GroupAverageHourScore

	/* Step 4: Final calcul */
	UPDATE PS
	SET PS.Fix_GroupTotalHourScore = GS.Fix_GroupTotalHourScore, PS.Fix_GroupDelta = (PS.GroupProductionValue/ GS.Fix_GroupTotalHourScore), 
	PS.Variable_GroupTotalScore = GS.Variable_GroupTotalScore, PS.Fix_GroupAverageHour = GS.Fix_GroupAverageHour, PS.Fix_GroupAverageHourScore = GS.Fix_GroupAverageHourScore,
	PS.GroupVariableSalary = PS.GroupVariableSharePropotion * GS.Fix_GroupAverageHour * (PS.GroupProductionValue/ GS.Fix_GroupTotalHourScore)
	FROM #tempProductionValueBasedSalary PS
	INNER JOIN Employe E ON PS.EmployeId = E.Id
	INNER JOIN #tempGroupHourScore GS ON E.GroupsId = GS.GroupId

	/* Step 4bis: Calcul the Variable part delta */
	UPDATE PS
	SET PS.Variable_GroupDelta = PS.GroupVariableSalary/ PS.Variable_GroupTotalScore
	FROM #tempProductionValueBasedSalary PS

	UPDATE S
	SET S.Salary_FixPart = PS.Fix_EmployeHoursScore * PS.Fix_GroupDelta , S.Salary_VariablePart = PS.Variable_GroupDelta * PS.WorkingScore
	FROM #tempSalaries S 
	INNER JOIN #tempProductionValueBasedSalary PS ON S.SalaryId = PS.SalaryId

	/* Calcul final salary */
	UPDATE S 
	SET S.BasicSalary = ISNULL(S.Salary_FixPart,0) + ISNULL(S.Salary_VariablePart,0)
	FROM #tempSalaries S 
	INNER JOIN #tempEmployees E ON S.EmployeId = E.EmployeId

	/* NET Salary */
	UPDATE S
	SET S.NetSalary = S.BasicSalary + S.AbsentDeduct + S.OvertimePay + S.SocialSercurityFee + S.SelfPaySocialSercurityFee + S.HousingReservesFee + S.OtherPenaltyFee + S.OtherPenaltyFee
	+ S.FullPresencePay + S.SeniorityPay + S.TransportFee + S.DormFee + S.DormOtherFee
	FROM #tempSalaries S 

	/* Calcul tax and final salary */
	UPDATE S
	SET S.SalaryTax = - [dbo].[FN_CalculRevenueTax](S.NetSalary), S.FinalSalary = S.NetSalary - [dbo].[FN_CalculRevenueTax](S.NetSalary)
	FROM #tempSalaries S 


	UPDATE S 
	SET S.EmployeName = E.Name
	FROM #tempSalaries S
	INNER JOIN Employe E ON E.Id = S.EmployeId

	UPDATE S 
	SET S.CycleLabel = C.Label
	FROM #tempSalaries S
	INNER JOIN Cycle C ON S.CycleId = C.Id

	UPDATE S 
	SET S.GroupLabel = G.Name
	FROM #tempSalaries S
	INNER JOIN Groups G ON S.GroupId = G.Id
	
	IF @IsUpdate IS NOT NULL AND @IsUpdate = 1 
	BEGIN
		DECLARE @CalculEndTime DATETIME = GETDATE()
		--INSERT INTO SalaryCalculLog(UserId, PeriodId, StatusSuccess, CreatedBy, CreatedOn, CalculTime)
		--VALUES(@UserId, @CycleId, 'Success', -1, @CalculEndTime,@CalculStartTime)

		/* Step 1: Update data */
		UPDATE S 
		SET S.UpdatedBy = @UserId, S.Validity = 0, S.ValidatedBy = NULL, S.ValidatedOn = NULL, -- Reset to invalid
		S.FinalSalary = TS.FinalSalary, S.SalaryTax = TS.SalaryTax, S.NetSalary = TS.NetSalary,  
		S.SelfPaySocialSercurityFee = TS.SelfPaySocialSercurityFee, S.SocialSercurityFee = TS.SocialSercurityFee, S.HousingReservesFee = TS.HousingReservesFee,  
		S.DormFee = TS.DormFee, S.SeniorityPay = TS.SeniorityPay, S.DormOtherFee = TS.DormOtherFee, S.OtherRewardFee = TS.OtherRewardFee,  S.OtherPenaltyFee = TS.OtherPenaltyFee, 
		S.TransportFee = TS.TransportFee, S.PositionPay = TS.PositionPay,  S.AbsentDeduct = TS.AbsentDeduct, S.OvertimePay = TS.OvertimePay,  S.BasicSalary = TS.BasicSalary, 
		S.WorkingDays = TS.WorkingDays,  S.DeferredHolidayHours = TS.DeferredHolidayHours,  S.AbsentHours = TS.AbsentHours, S.WorkingScore = TS.WorkingScore, S.WorkingHoursHoliday = TS.WorkingHoursHoliday,
		S.WorkingHoursNight = TS.WorkingHoursNight, S.WorkingHoursDay = TS.WorkingHoursDay, S.WorkingHours = TS.WorkingHours
		FROM Salary S 
		INNER JOIN #tempSalaries TS ON S.Id = TS.SalaryId
		
	END
	
	SELECT * FROM #tempSalaries order by GroupId
	
END
GO
/* 
* BEGIN 
* Author : ZLI
* DATE : 22/12/2020 
* Description : SP_CalculSalaries
*/
WITH OrderData AS (
	SELECT 
		float_number,
		ROW_NUMBER() OVER (ORDER BY float_number) as RowNum,
		Count(*) OVER() AS TotalCount
	From dbo.data
)
Select (Sum(number)) As  SumValue,
	(Select 
		Case
			When TotalCount % 2 = 1 THEN
				(Select float_number FROM OrderData Where RowNum = (TotalCount + 1) / 2)
			ELSE	
				(Select AVG(float_number)
				FROM OrderData 
				WHERE RowNum IN (TotalCount / 2, TotalCount / 2 + 1))
		END
	From (Select DISTINCT TOTALCOUNT FROM OrderData) As CountTable) as Median
From data
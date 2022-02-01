--truncate table [raw-metrics]
select top 1000 
	* 
from [raw-metrics] 
where MaintainabilityIndex < 50 and LinesOfCode > 1000 
	and Member not like '%InitializeComponent%'
	and Member not like '%LoadEntity%'
order by linesofcode desc

select * from (
select top 1000 
	(100 - MaintainabilityIndex) * CyclomaticComplexity as WeightedScore,
	* 
from [raw-metrics]
order by (100 - MaintainabilityIndex) * CyclomaticComplexity desc
) t
order by t.LinesOfCode 


select count(*) from [raw-metrics] where MaintainabilityIndex < 50

-- 651488

select * from [raw-metrics] where type like 'MgrEDI%'
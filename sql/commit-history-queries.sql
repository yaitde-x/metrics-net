select count(*) from gitHistory
select top 1000 * from gitHistory

select sourcefile, count(*) as ChanggeCount
from gitHistory 
where sourcefile not like '%content.txt' and sourcefile not like '%proj'
group by sourcefile
order by count(*) desc
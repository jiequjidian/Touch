--
--向表中添加测试数据
--

declare @DateTime datetime
declare @EndDateTime datetime
declare @Val float
set @DateTime='2015-5-27'
set @EndDateTime = '2015-5-29'
while @DateTime <= @EndDateTime 
begin
set @Val=cast(floor(rand()*1000) as float)
insert into 出水COD值(DateTime,Val) values(@DateTime,@Val)
--每分钟记录一个数据
set @DateTime=DATEADD(n, +1, @DateTime) 
end

update 出水COD值 set Ann = '' where DateTime ='' 

select *  from 出水COD值
select *  from 出水NH3N值
select *  from 出水PH值
select *  from 出水TN值
select *  from 出水TP值
select *  from 出水流量值
select *  from 二期1#DO
select *  from 二期2#DO
select *  from 二期MLSS
select *  from 二期风管值

delete from 出水COD值
delete from 出水NH3N值
delete from 出水PH值
delete from 出水TN值
delete from 出水TP值
delete from 出水流量值
delete from 二期1#DO
delete from 二期2#DO
delete from 二期MLSS
delete from 二期风管值

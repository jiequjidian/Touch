--
--�������Ӳ�������
--

declare @DateTime datetime
declare @EndDateTime datetime
declare @Val float
set @DateTime='2015-5-27'
set @EndDateTime = '2015-5-29'
while @DateTime <= @EndDateTime 
begin
set @Val=cast(floor(rand()*1000) as float)
insert into ��ˮCODֵ(DateTime,Val) values(@DateTime,@Val)
--ÿ���Ӽ�¼һ������
set @DateTime=DATEADD(n, +1, @DateTime) 
end

update ��ˮCODֵ set Ann = '' where DateTime ='' 

select *  from ��ˮCODֵ
select *  from ��ˮNH3Nֵ
select *  from ��ˮPHֵ
select *  from ��ˮTNֵ
select *  from ��ˮTPֵ
select *  from ��ˮ����ֵ
select *  from ����1#DO
select *  from ����2#DO
select *  from ����MLSS
select *  from ���ڷ��ֵ

delete from ��ˮCODֵ
delete from ��ˮNH3Nֵ
delete from ��ˮPHֵ
delete from ��ˮTNֵ
delete from ��ˮTPֵ
delete from ��ˮ����ֵ
delete from ����1#DO
delete from ����2#DO
delete from ����MLSS
delete from ���ڷ��ֵ

-- The following sql queries for each of the ToDO's

--TODO 1--
select p.groupId,p.groupCode,p.fypAssigned,f.title,f.fypCategory,f.fypType
from ProjectGroup p,FYProject f where p.fypAssigned=f.fypId and reader is NULL 
order by p.groupCode asc;

--TODO 2-- Answer=2 for reader='ray'
select count(*) from ProjectGroup where reader= 'ray';

--TODO 3-- Works Testing
update ProjectGroup set reader='ray' where groupId='3';

--TODO 4--
select f.facultyName, p.groupCode, fyp.title from Faculty f,FYProject fyp,
ProjectGroup p where p.fypAssigned=fyp.fypId and p.reader=f.username and p.reader is NOT NULL
order by p.groupCode asc;

--TODO 5--Result: NR for username='ray'
select facultyCode from Faculty where username='ray';

--TODO 6--Result: MC1,MC2 for the facultyCode='NR'
select p.groupId,p.groupCode,p.fypAssigned from ProjectGroup p,Faculty f where
p.reader=f.username and f.facultyCode='NR' order by p.groupId asc;

--TODO 7--
select s.Username, s.studentname,r.proposalGrade,r.progressGrade,r.finalGrade,r.presentationGrade
from Requirement r, Students s,ProjectGroup p where r.studentUsername=s.username and 
p.groupId=s.groupId and s.groupId=1 and p.fypAssigned=2 and r.facultyUsername in
(select username from Supervises);

--TODO 8--
update Requirement set progressGrade= ,finalGrade= ,presentationGrade= ,
proposalGrade= where studentUsername='' and facultyUsername in 
(select username from Supervises where fypId= );

--TODO 9--
select f.facultyCode from Faculty f,Supervises s where s.username=f.username 
and s.fypId=13;

--TODO 10--
select count(*) from ProjectGroup where substr(groupCode,0,(Length(groupCode)-1))='MC';

--TODO 11--
select DISTINCT i.groupid,i.fypPriority,s.studentName,s.username from Students s, InterestedIn i, ProjectGroup p where
s.groupId=i.groupId and p.groupId=i.groupId and p.groupId=s.groupId and i.fypId=3
and p.fypAssigned is null and (select isAvailable from FYProject where fypId=1)='Y' order by i.groupId asc,s.studentName asc;

--TODO 12--
select p.groupId,p.groupCode,s.StudentName from Students s,ProjectGroup p
where s.groupId=p.groupId and p.fypAssigned=5
order by p.groupId asc,s.studentName asc;


--TODO 13--
select f.fypId,f.title from FYProject f,Supervises s where
s.fypId=f.fypId and s.username ='cafarella' order by f.title asc;

--TODO 14--
select isAvailable from FYProject where fypId=1;

--TODO 15--
update ProjectGroup set groupCode='' and fypAssigned= 
where groupId= ;

--TODO 16--
select count(p.groupId) from ProjectGroup p, Supervises s 
where p.fypAssigned=s.fypId and s.username='cafarella';

--TODO 17--
insert into FYProject values( 31,'Binay Project','fypDescription','Theory','project','requirement',3,4,'Y');


--TODO 18--
insert into Supervises values('cafarella', 31);

--TODO 19--
select fp.fypId,fp.title,fp.fypCategory,fp.fypType,fp.minStudents,fp.maxStudents
from FYProject fp,Supervises s where s.fypId=fp.fypId 
and s.username='cafarella' order by fp.title asc;

--TODO 20--
select * from InterestedIn where fypId=2 ;

--TODO 21--
select distinct s2.username from Supervises s1,Supervises s2 
where s1.fypId=s2.fypId and s1.username!=s2.username and s1.username='pantel';

--TODO 22--
update FYProject set title='title',fypDescription='Description',fypCategory='category',
fyptype='type',requirement='requirement',minStudents=minstds,maxstudents=maxstds,isAvailable='isAvailable'
where fypId=projectId;

--TODO 23--
delete from Supervises where username='cafarella' and fypId=1;
--select * from supervises where username='cafarella';
--TODO 24--
insert into InterestedIn values(1,1,3);
--select * from InterestedIn where groupId=1;

--TODO 25--
select f.title from FYProject f,ProjectGroup p where p.fypAssigned=f.fypId
and p.groupId=6;

--TODO 26--
update students set groupId=groupId where username='username';

--TODO 27--
select * from Students where username='carlchan';

--TODO 28--
update Students set groupId=NULL where username='clintchu';
select * from Students;
--TODO 29--
delete from ProjectGroup where groupId=groupId;

--TODO 30--
insert into ProjectGroup values(groupId,NULL,NULL,NULL);

--TODO 31--
select f.fypId, f.title from FYProject f,ProjectGroup p,Students s 
where s.groupId=p.groupId and p.fypAssigned=f.fypId and s.username='kathyko';

--TODO 32--
select f.facultyName,r.proposalGrade,r.progressGrade,r.finalGrade,r.presentationGrade 
from Faculty f,Requirement r where f.username=r.facultyUsername and r.studentUsername='brunoho';

--TODO 33--
insert into Requirement values('facultyusername','studentusername',proposalGrade,progressGrade,finalgrade,presentationGrade);

--TODO 34--
select fypAssigned from ProjectGroup where groupId=groupId;

--TODO 35--
select username and facultyName from Faculty;

--TODO 36--
select fp.fypId,fp.title,fp.fypCategory,fp.fypType,fp.minStudents,fp.maxStudents 
from FYProject fp where fp.isAvailable='Y' 
and fp.fypId NOT in (select fypId from InterestedIn where groupId=1) 
and (select count(*) from Students where groupId=1) between fp.minStudents and fp.maxStudents;

--TODO 37--
select * from Students where groupId = groupId;

--TODO 38--
select GroupId from Students where username='username';

--TODO 39--
select f.username,f.facultyName from Faculty f,Supervises s 
where f.username=s.uername and s.fypId=fypId;

--TODO 40--
SELECT f.fypId,f.title,f.fypCategory,f.fyptype,i.fypPriority from
InterestedIn i, FYProject f where f.fypId=i.fypId and i.groupId=groupId
order by i.fypPriority asc;

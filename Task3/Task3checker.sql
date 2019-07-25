-- The following sql queries for each of the ToDO's

--TODO 1--
select p.groupId,p.groupCode,p.fypAssigned,f.title,f.fypCategory,f.fypType
from ProjectGroup p,FYProject f where p.fypAssigned=f.fypId and reader is NULL 
order by p.groupCode asc;

--TODO 2--
select count(*) from ProjectGroup where reader= --username--;

--TODO 3--
update ProjectGroup set reader='' where groupId=''

--TODO 4--
select f.facultyName, p.groupCode, fyp.title from Faculty f,FYProject fyp,
ProjectGroup p where p.fypAssigned=fyp.fypId and p.reader=f.username and p.reader is NOT NULL
order by p.groupCode asc;

--TODO 5--
select facultyCode from Faculty where username='';

--TODO 6--
select p.groupId,p.groupCode,p.fypAssigned from ProjectGroup p,Faculty f where
p.reader=f.username and f.facultyCode='' order by p.groupId asc;

--TODO 7--
select r.studentUsername, s.studentname,r.proposalGrade,r.progressGrade,r.finalGrade,r.presentationGrade
from Requirement r, Students s,ProjectGroup p where r.studentUsername=s.username and 
p.groupId=s.groupId and s.groupId='' and p.fypAssigned= ;

--TODO 8--
update Requirement set progressGrade= ,finalGrade= ,presentationGrade= ,
proposalGrade= where studentUsername='' and facultyUsername in 
(select username from Supervises where fypId= );

--TODO 9--
select f.facultyCode from Faculty f,Supervises s where s.username=f.username 
and s.fypId= ;

--TODO 10--
select count(*) from ProjectGroup having substr(groupCode,0,(Length(groupCode)-1))='';

--TODO 11--
select i.groupId,i.fypPriority,s.studentName,s.username from Students s,InterestedIn i,ProjectGroup p
where s.groupId=p.groupId and p.groupId=i.groupId and p.fypAssigned is null and i.fypId in
(select fypId from FYProject where isAvailable='N') order by p.groupId asc,
s.studentName asc;

--TODO 12--
select p.groupId,p.groupCode,s.StudentName from Students s,ProjectGroup p
where s.groupId=p.groupId and p.fypAssigned= 
order by p.groupId asc,s.studentName asc;

--TODO 13--
select f.fypId,f.title from FYProject f,Supervises s where
s.fypId=f.fypId and s.username ='' order by f.title asc;

--TODO 14--
select isAvailable from FYProject where fypId= ;

--TODO 15--
update ProjectGroup set groupCode='' and fypAssigned= 
where groupId= ;

--TODO 16--
select count(p.groupId) from ProjectGroup p, Supervises s 
where p.fypAssigned=s.fypId and s.username='';

--TODO 17--


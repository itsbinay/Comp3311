select p.groupid,p.groupCode,f.title,f.fypCategory,f.fypType from ProjectGroup p,FYProject f 
where p.fypAssigned=f.fypid and reader is NULL order by p.groupCode;

select f.facultyName,p.groupCode,fyp.title from  Faculty f, FYProject fyp,
                ProjectGroup p where p.fypAssigned=fyp.fypId and p.reader=f.username and p.reader is NOT NULL 
                order by p.groupCode asc;
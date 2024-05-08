-- numCheckins
update business b
set numcheckins = sq.numCheckins
from (select count(business_id) as numCheckins, business_id from checkins
group by business_id
order by numCheckins) as sq
where b.business_id = sq.business_id;

--numTips
update business b 
set numtips = sq.numTips
from (select count(business_id) as numTips, business_id from tip
group by business_id
order by numTips) as sq
where b.business_id = sq.business_id;

--totalLikes
update users u
set totallikes = lc.totalLikes
from (select sum(likes) as totalLikes, user_id from tip group by user_id) as lc
where u.user_id = lc.user_id;

--tipCount
update users u
set tipcount = lc.tipCount
from (select count(business_id) as tipCount, user_id from tip group by user_id) as lc
where u.user_id = lc.user_id;

--update to have 0 lat and lon instead of null
UPDATE users Set user_latitude = 0, user_longitude = 0 where user_latitude is null or user_longitude is null;
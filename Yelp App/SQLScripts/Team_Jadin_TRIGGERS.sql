--add a tip, tip business and user tables are updated
create or replace function updatetipcount() returns trigger as '
begin
	update business
	set numtips = numtips + 1
	where business_id = new.business_id;
	
	update users
	set tipcount = tipcount + 1
	where user_id = new.user_id;
	
	return new;
end
' language plpgsql;


Create trigger addtip
	after insert on tip
	for each row
	execute procedure updatetipcount();
	

--check-in business, checkin and business table

create or replace function updatecheckins() returns trigger as 
'begin
	update business
	set numcheckins = numcheckins + 1
	where business_id = new.business_id;
	
	return new;
end'
language plpgsql;
 
create trigger addcheckins
	after insert on checkins
	for each row
	execute procedure updatecheckins();

--like a tip, update tip and user table

create or replace function updatelikes() returns trigger as 
'begin
	update users
	set totallikes = totallikes + 1
	where old.user_id = new.user_id;
	
	return new;
end'
language plpgsql;
 
create trigger liketip
	after update on tip
	for each row
	execute procedure updatelikes();



create function getDistance(ulat double precision, ulng double precision, blat double precision, blng double precision) returns double precision
    immutable
    language sql
as
$$
SELECT asin(sqrt(sin(radians($3-$1)/2)^2 +sin(radians($4-$2)/2)^2 *cos(radians($1)) *cos(radians($3)))) * 7926.3352
    AS distance;
$$;

alter function getDistance(double precision, double precision, double precision, double precision) owner to postgres;
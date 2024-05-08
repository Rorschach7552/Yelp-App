CREATE TABLE Users(
user_id CHAR(22),
firstName VARCHAR,
lastName VARCHAR,
average_stars INTEGER,
fans INTEGER,
cool INTEGER,
tipCount INTEGER,
totalLikes INTEGER,
funny INTEGER,
useful INTEGER,
yelping_since DATE,
user_latitude DECIMAL,
user_longitude DECIMAL,
PRIMARY KEY (user_id) 
);

CREATE TABLE Friends (
user_id CHAR(22),
Friend_id CHAR(22),
PRIMARY KEY (user_id,friend_id),
FOREIGN KEY (user_id) REFERENCES Users(user_id),
FOREIGN KEY (Friend_id) REFERENCES Users(user_id)
);


CREATE TABLE Business (
business_id CHAR(33),
zipcode CHAR(10),
name VARCHAR,
city VARCHAR,
state VARCHAR,
user_latitude DECIMAL,
user_longitude DECIMAL,
address VARCHAR,
numtips INTEGER,
numCheckins INTEGER,
is_open VARCHAR,
stars INTEGER,
PRIMARY KEY (business_id) 
);

CREATE TABLE Tip (
tipDate TIMESTAMP NOT NULL,
tipText VARCHAR NOT NULL,
likes INTEGER NOT NULL,
user_id CHAR(22) NOT NULL,
business_id CHAR(33) NOT NULL,

PRIMARY KEY (tipDate,user_id,business_id),
FOREIGN KEY (user_id) REFERENCES Users(user_id),
FOREIGN KEY (business_id) REFERENCES Business(business_id)
);

CREATE TABLE  Categories (
category_name VARCHAR,
business_id CHAR(33),
PRIMARY KEY (business_id,category_name),
FOREIGN KEY (business_id) REFERENCES Business(business_id)
);

CREATE TABLE  Attributes (
attr_name VARCHAR,
value VARCHAR,
business_id CHAR(33),
PRIMARY KEY (business_id,attr_name),
FOREIGN KEY (business_id) REFERENCES Business(business_id)
);

CREATE TABLE  Hours (
dayofweek VARCHAR,
close VARCHAR,
open VARCHAR,
business_id CHAR(33),
PRIMARY KEY (business_id,dayofweek),
FOREIGN KEY (business_id) REFERENCES Business(business_id)
);

CREATE TABLE  Checkins (
year INTEGER,
month INTEGER,
day INTEGER,
time time, 
business_id CHAR(33),
PRIMARY KEY (business_id,year,month,day,time),
FOREIGN KEY (business_id) REFERENCES Business(business_id)
);
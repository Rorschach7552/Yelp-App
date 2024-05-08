# CptS 451 - Spring 2022
# https://www.psycopg.org/docs/usage.html#query-parameters

#  if psycopg2 is not installed, install it using pip installer :  pip install psycopg2  (or pip3 install psycopg2) 
import json
import psycopg2


def cleanStr4SQL(s):
    return s.replace("'", "`").replace("\n", " ")


def int2BoolStr(value):
    if value == 0:
        return 'False'
    else:
        return 'True'


def flatten_dictionary(d):
    result = {}
    for key, value in d.items():
        if isinstance(value, dict):
            result |= flatten_dictionary(value)
        else:
            result[key] = value
    return result


def flatten_hours(hours, business_id):
    result = []
    for day in hours.items():
        hours = day[1].split("-", 1)
        insert_sttmnt = ""
        day = day[0]
        # insert_sttmnt = ("INSERT INTO hours (dayOfWeek, open, close, business_id)"
        #     + " VALUES (%s, %s, %s, %s)", '" + str(day) + ", '" + time[0] + "', '" + time[1] + "', '"  + business_id + "'')
        insert_sttmnt = ("INSERT INTO hours (dayOfWeek, open, close, business_id) VALUES (%s, %s, %s, %s)",
                         (str(day), hours[0], hours[1], business_id))
        result.append(insert_sttmnt)
    return result


def create_attribiute_statments(attributes, business_id):
    result = []
    for attribute in attributes.items():
        insert_sttmnt = ("INSERT INTO attributes (attr_name, value, business_id) VALUES (%s, %s, %s)",
                         (attribute[0], attribute[1], business_id))
        result.append(insert_sttmnt)
    return result


def create_catagories_statments(catagories_str, business_id):
    result = []
    catagories = catagories_str.split(", ")
    for catagory in catagories:
        insert_sttmnt = ("INSERT INTO categories (category_name, business_id) VALUES (%s, %s)", (catagory, business_id))

        result.append(insert_sttmnt)
    return result


def create_checkins_statments(checkins_str, business_id):
    result = []
    checkins = checkins_str.split(",")
    for checkin in checkins:
        date = checkin.split(" ")[0]
        time = checkin.split(" ")[1]
        year = int(date.split("-")[0])
        month = int(date.split("-")[1])
        day = int(date.split("-")[2])
        insert_sttmnt = ("INSERT INTO checkins(year,month,day,time,business_id) VALUES (%s, %s,%s, %s,%s)",
                         (year, month, day, time, business_id))
        result.append(insert_sttmnt)
    return result


def insert2checkins():
    # reading the JSON file
    with open('./yelp_checkin.JSON', 'r') as f:  # TODO: update path for the input file
        # outfile =  open('./yelp_business_out.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        # connect to yelpdb database on postgres server using psycopg2
        try:
            # TODO: update the database name, username, and password
            conn = psycopg2.connect("dbname='yelpData' user='postgres' host='localhost' password='password'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)

            insert_statments = create_checkins_statments(data['date'], data['business_id'])
            for insert_statment in insert_statments:

                try:
                    cur.execute(insert_statment[0], insert_statment[1])
                except Exception as e:
                    print("Insert to checkins failed!", e)
            conn.commit()

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()

    print(count_line)
    # outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()


def insert2friendsTable():
    # reading the JSON file
    with open('.//yelp_user.JSON', 'r') as f:
        # outfile = open('.//yelp_user.JSON', 'w')
        line = f.readline()
        count_line = 0

        # connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='yelpData' user='postgres' host='localhost' password='password'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)

            for val in data['friends']:
                insert_sttmnt = "INSERT INTO Friends (user_id, friend_id) " "VALUES ('" + cleanStr4SQL(
                    data['user_id']) + "', '" + str(val) + "');"

                try:
                    cur.execute(insert_sttmnt)

                except Exception as e:
                    print("Insert to categories failed!", e)
                conn.commit()

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()

    print(count_line)
    # outfile.close()
    f.close()


def insert2TipTable():
    # reading the JSON file
    with open('.//yelp_tip.JSON', 'r') as f:
        # outfile = open('.//yelp_user.JSON', 'w')
        line = f.readline()
        count_line = 0

        # connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='yelpData' user='postgres' host='localhost' password='password'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)

            insert_sttmnt = "INSERT INTO tip (user_id, business_id,tipdate, likes, tiptext) " \
                            "VALUES ('" + cleanStr4SQL(data['user_id']) + "','" + cleanStr4SQL(data["business_id"]) + \
                            "','" + cleanStr4SQL(data["date"]) + "'," + str(data["likes"]) + ",'" + cleanStr4SQL(
                data["text"]) + "');"

            try:
                cur.execute(insert_sttmnt)

            except Exception as e:
                print("Insert to categories failed!", e)
            conn.commit()

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()

    print(count_line)
    # outfile.close()
    f.close()


def insert2categoriesTable():
    # reading the JSON file
    with open('./yelp_business.JSON', 'r') as f:  # TODO: update path for the input file
        # outfile =  open('./yelp_business_out.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        # connect to yelpdb database on postgres server using psycopg2
        try:
            # TODO: update the database name, username, and password
            conn = psycopg2.connect("dbname='yelpData' user='postgres' host='localhost' password='password'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)

            insert_statments = create_catagories_statments(data['categories'], data['business_id'])
            for insert_statment in insert_statments:

                try:
                    cur.execute(insert_statment[0], insert_statment[1])
                except Exception as e:
                    print("Insert to categories failed!", e)
            conn.commit()

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()

    print(count_line)
    # outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()


def insert2attributesTable():
    # reading the JSON file
    with open('./yelp_business.JSON', 'r') as f:  # TODO: update path for the input file
        # outfile =  open('./yelp_business_out.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        # connect to yelpdb database on postgres server using psycopg2
        try:
            # TODO: update the database name, username, and password
            conn = psycopg2.connect("dbname='yelpData' user='postgres' host='localhost' password='password'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            attributes = data["attributes"]
            attributes = flatten_dictionary(attributes)
            insert_statments = create_attribiute_statments(attributes, data['business_id'])
            for insert_statment in insert_statments:

                try:
                    cur.execute(insert_statment[0], insert_statment[1])
                except Exception as e:
                    print("Insert to attributes failed!", e)
            conn.commit()

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()

    print(count_line)
    # outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()


def insert2BusinessTable():
    # reading the JSON file
    with open('./yelp_business.JSON', 'r') as f:  # TODO: update path for the input file
        # outfile =  open('./yelp_business_out.SQL', 'w')  #uncomment this line if you are writing the INSERT statements to an output file.
        line = f.readline()
        count_line = 0

        # connect to yelpdb database on postgres server using psycopg2
        try:
            # TODO: update the database name, username, and password
            conn = psycopg2.connect("dbname='yelpData' user='postgres' host='localhost' password='password'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the current business
            # TODO: The below INSERT statement is based on a simple (and incomplete) businesstable schema. Update the statement based on your own table schema and
            # include values for all businessTable attributes
            try:
                cur.execute(
                    "INSERT INTO business (business_id, name, address, state, city, zipcode, user_latitude, user_longitude, stars, numCheckins, numTips, is_open)"
                    + " VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)",
                    (data['business_id'], cleanStr4SQL(data["name"]), cleanStr4SQL(data["address"]), data["state"],
                     data["city"], data["postal_code"], data["latitude"], data["longitude"], data["stars"], 0, 0,
                     [False, True][data["is_open"]]))
            except Exception as e:
                print("Insert to businessTABLE failed!", e)
            conn.commit()
            # optionally you might write the INSERT statement to a file.
            # sql_str = ("INSERT INTO businessTable (business_id, name, address, state, city, zipcode, latitude, longitude, stars, numCheckins, numTips, is_open)"
            #           + " VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, {7}, {8}, {9}, {10}, {11})").format(data['business_id'],cleanStr4SQL(data["name"]), cleanStr4SQL(data["address"]), data["state"], data["city"], data["postal_code"], data["latitude"], data["longitude"], data["stars"], 0 , 0 , [False,True][data["is_open"]] )            
            # outfile.write(sql_str+'\n')

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()

    print(count_line)
    # outfile.close()  #uncomment this line if you are writing the INSERT statements to an output file.
    f.close()


def insert2UsersTable():
    # reading the JSON file
    with open('./yelp_user.JSON', 'r') as f:
        # outfile =  open('.//yelp_dataset//yelp_user.SQL', 'w')
        line = f.readline()
        count_line = 0

        # connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='yelpData' user='postgres' host='localhost' password='password'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            # Generate the INSERT statement for the current user
            # include values for all userTable attributes

            try:
                cur.execute(
                    "INSERT INTO Users (user_id, firstname,  average_stars, fans, cool, tipcount, totalLikes, funny, useful, yelping_since) "
                    + " VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s )",
                    (cleanStr4SQL(data['user_id']), cleanStr4SQL(data["name"]), data["average_stars"],
                     data["fans"], data["cool"], data["tipcount"], 0, data["funny"], data["useful"],
                     data["yelping_since"]))
            except  Exception as e:
                print("Insert to userTABLE failed!" + " " + data['user_id'] + " " + data['name'] + " Exception:", e)
            conn.commit()
            # write the INSERT statement to a file.
            # outfile.write(sql_str + '\n')

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()

    print(count_line)
    # outfile.close()
    f.close()


def insert2Hours():
    # reading the JSON file
    with open('./yelp_business.JSON', 'r') as f:
        # outfile =  open('.//yelp_dataset//yelp_user.SQL', 'w')
        line = f.readline()
        count_line = 0

        # connect to yelpdb database on postgres server using psycopg2
        try:
            conn = psycopg2.connect("dbname='yelpData' user='postgres' host='localhost' password='password'")
        except:
            print('Unable to connect to the database!')
        cur = conn.cursor()

        while line:
            data = json.loads(line)
            hours = flatten_hours(data['hours'], data['business_id'])
            # Generate the INSERT statement for the current user
            # include values for all userTable attributes
            for items in hours:
                val = items

                try:
                    cur.execute(val[0], val[1])

                except  Exception as e:
                    print("Insert to hoursTABLE failed!" + " " + data['user_id'] + " " + data['name'] + " Exception:",
                          e)
            conn.commit()
            # write the INSERT statement to a file.
            # outfile.write(sql_str + '\n')

            line = f.readline()
            count_line += 1

        cur.close()
        conn.close()

    print(count_line)
    # outfile.close()
    f.close()


if __name__ == '__main__':
    insert2BusinessTable()
    insert2UsersTable()
    insert2Hours()
    insert2attributesTable()
    insert2categoriesTable()
    insert2checkins()
    insert2friendsTable()
    insert2TipTable()

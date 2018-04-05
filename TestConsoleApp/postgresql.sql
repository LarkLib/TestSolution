CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
SELECT * FROM pg_extension;
--https://www.postgresql.org/docs/9.4/static/uuid-ossp.html
SELECT  
  uuid_generate_v1(),
  uuid_generate_v1mc(),
  uuid_generate_v3(uuid_ns_url(), 'http://www.postgresql.org'),
  uuid_generate_v4(),
  uuid_generate_v5(uuid_ns_url(), 'http://www.postgresql.org'),
  uuid_nil(),
  uuid_ns_url(),
  uuid_ns_dns(),
  uuid_ns_oid(),
  uuid_ns_x500()	
SELECT current_database(),current_date,current_time,now(),current_user,current_timestamp;
SELECT pg_sleep(1.5),pg_sleep_for('5 minutes'),SELECT pg_sleep_until('tomorrow 03:00');
SELECT rolname FROM pg_roles; --query role list
SELECT * FROM pg_shadow; --query user list

GRANT ALL PRIVILEGES ON DATABASE test to testuser;
GRANT ALL ON SEQUENCE business.persion_seq_seq TO testuser WITH GRANT OPTION;

CREATE SEQUENCE business.testseq;
ALTER SEQUENCE business.testseq OWNER TO testuser;
SELECT nextval('business.testseq'), currval('business.testseq');
SELECT setval('business.testseq', 42),nextval('business.testseq'); --Next nextval will return 43
SELECT setval('business.testseq', 42, true),nextval('business.testseq'); --Same as above
SELECT setval('business.testseq', 42, false),nextval('business.testseq'); --Next nextval will return 42
SELECT CASE WHEN is_called=TRUE THEN last_value ELSE last_value-1 END AS cvalue FROM business.testseq;
SELECT * FROM business.testseq;

--https://www.postgresql.org/docs/10/static/functions-datetime.html
SELECT DATE '1999-12-31' - DATE '1999-12-30' AS d;
SELECT interval '1 year' - interval '1 hour',interval '1 year' + interval '100 hour',interval '100',now() + '-10 y';
SELECT to_char(current_timestamp, 'HH12:MI:SS'),age(timestamp '2007-09-15');
SELECT
  extract(day from timestamp '2013-04-13') AS day,
  extract(doy from now()) AS day_of_year,
  EXTRACT(DOW FROM TIMESTAMP '2001-02-16 20:38:40') AS weeks, --The day of the week as Sunday (0) to Saturday (6)
  extract(epoch from now()) AS second_from_1970_1_1_UTC,
  EXTRACT(EPOCH FROM INTERVAL '5 days 3 hours'),
  extract(DAY FROM INTERVAL '40 days 1 minute') AS IntervalDay,
  EXTRACT(CENTURY FROM TIMESTAMP '2000-12-16 12:21:13') AS CENTURY,
  EXTRACT(DECADE FROM TIMESTAMP '2001-02-16 20:38:40') AS year_divided_by_10
  date_part('hour', timestamp '2001-02-16 20:38:40'),
  date_part('month', interval '2 years 3 months'),
  date_trunc('day', timestamp '2001-02-16 20:38:40'),
  date_trunc('hour', interval '2 days 3 hours 40 minutes'),
  isfinite(interval '0 years 0 mons 2 days 3 hours 0 mins 0.00 secs'),
  justify_days(interval '35 days'),
  justify_hours(interval '27 hours 300 minutes'),
  justify_interval(interval '3 years -1 month 50 day -240 hour'),
 (DATE '2001-02-16', DATE '2001-12-21') OVERLAPS (DATE '2001-10-30', DATE '2002-10-30'),
  date_part('day', '2015-02-15 17:05'::timestamp - '2015-01-14 16:05'::timestamp);
  
CREATE TABLE persion
(
  id             UUID DEFAULT uuid_generate_v1()     NOT NULL
    CONSTRAINT persion_pkey
    PRIMARY KEY,
  seq            SERIAL                              NOT NULL,
  age            INTEGER,
  name           VARCHAR(50)                         NOT NULL,
  wage           MONEY DEFAULT 0.00                  NOT NULL,
  hiredate       DATE,
  lastupdatetime TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL
);


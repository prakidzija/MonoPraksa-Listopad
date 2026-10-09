create extension if not exists pgcrypto;


create table clubs(
	id uuid primary key default gen_random_uuid(),
	name varchar(30) not null unique
);


create table players(
	id uuid primary key default gen_random_uuid(),
	name varchar(30),
	age integer not null
		check (age > 0),
	position varchar(20)
		check (position in ('Setter', 'Libero', 'Middle', 'Opposite', 'Outside'))
);

create table player_registrations(
	id uuid primary key default gen_random_uuid(),
	player_id uuid not null,
	club_id uuid not null,
	registration_type varchar(20) not null
		check(registration_type in ('permanent', 'loan', 'dual_license')),
	jersey_number integer not null,
	start_date date not null,
	end_date date,
	foreign key (player_id) references players(id),
	foreign key (club_id) references clubs(id),
	unique (club_id, jersey_number),
	check (end_date is null or end_date >= start_date)
);

alter table clubs
add column adress varchar(40) not null;

alter table players
alter column name set not null;

create index idx_registrations_player_id on player_registrations(player_id);
create index idx_registrations_club_id on player_registrations(club_id);

insert into clubs (name, adress) values
	('Porto', 'Porto Street 5'),
	('Coster', 'Coster Street 56'),
	('Lakers', 'Lake Square 109');

select * from clubs;

insert into players (name, age, position) values 
	('Periqa', 23, 'Opposite'),
	('Sloane', 20, 'Opposite'),
	('Leona', 19, 'Libero'),
	('Rene', 25, 'Setter'),
	('Silvia', 23, 'Outside'),
	('Maria', 22, 'Outside'),
	('Tiana', 20, 'Middle'),
	('Rika', 21, 'Middle'),
	('Odette', 24, 'Middle'),
	('Hannah', 22, 'Outside'),
	('Monique', 27, 'Setter'),
	('Barbara', 26, 'Setter'),
	('Kate', 23, 'Opposite'),
	('Luna', 20, 'Libero'),
	('Nikol', 24, 'Opposite'),
	('Megi', 21, 'Outside'),
	('Gwen', 24, 'Setter'),
	('Sasha', 20, 'Middle'),
	('Diana', 18, 'Libero'),
	('Helena', 29, 'Opposite');

insert into player_registrations (player_id, club_id, registration_type, jersey_number, start_date) values 
(
	'3cedaa5a-4385-40c8-a553-3adc8ca692d2',
	(select id from clubs where name = 'Porto'),
	'permanent',
	10,
	'2024-10-06'
),
(
	'48460bd5-b119-4220-a5cc-8e3c0fc083e4',
	(select id from clubs where name = 'Porto'),
	'permanent',
	7,
	'2024-10-06'
),
(
	'499763f0-d595-4487-bb6a-2e87d1c6d9fb',
	(select id from clubs where name = 'Porto'),
	'permanent',
	1,
	'2025-1-30'
),
(
	'4d0b2080-4201-47e6-a350-ac1c8553b765',
	(select id from clubs where name = 'Lakers'),
	'permanent',
	1,
	'2026-10-03'
),
(
	'4d4480ec-49d2-406a-9346-3a4683612aab',
	(select id from clubs where name = 'Porto'),
	'permanent',
	2,
	'2025-11-30'
),
(
	'5f2726b1-d00e-4a85-beab-ee815dd271aa',
	(select id from clubs where name = 'Lakers'),
	'dual_license',
	13,
	'2025-11-11'
),
(
	'5f2726b1-d00e-4a85-beab-ee815dd271aa',
	(select id from clubs where name = 'Coster'),
	'dual_license',
	12,
	'2025-11-11'
),
(
	'744d82c1-25fc-4517-819a-54c876a1b509',
	(select id from clubs where name = 'Coster'),
	'permanent',
	8,
	'2026-08-17'
),
(
	'7f329ba1-33e3-4147-aed7-b33ef8e3eaea',
	(select id from clubs where name = 'Coster'),
	'permanent',
	9,
	'2026-06-16'
),
(
	'7f329ba1-33e3-4147-aed7-b33ef8e3eaea',
	(select id from clubs where name = 'Porto'),
	'loan',
	4,
	'2026-09-13'
),
(
	'8e63bbff-c353-42f1-b127-1a27c036dab5',
	(select id from clubs where name = 'Lakers'),
	'permanent',
	6,
	'2025-06-06'
),
(
	'906188e2-aeff-4e8e-ad54-2efd4558c321',
	(select id from clubs where name = 'Lakers'),
	'permanent',
	3,
	'2025-04-26'
),
(
	'95fd7165-4135-489e-8731-92fd5ebe7bd1',
	(select id from clubs where name = 'Coster'),
	'permanent',
	5,
	'2025-03-20'
),
(
	'9ec6ee5d-bbf2-4115-90db-df4df1f479d4',
	(select id from clubs where name = 'Lakers'),
	'permanent',
	19,
	'2024-07-29'
),
(
	'a0b8bba9-a558-433d-89f3-956876927b95',
	(select id from clubs where name = 'Porto'),
	'dual_license',
	19,
	'2024-07-02'
),
(
	'a0b8bba9-a558-433d-89f3-956876927b95',
	(select id from clubs where name = 'Coster'),
	'dual_license',
	11,
	'2024-07-02'
),
(
	'b10d0380-ed1f-49d9-a996-86746ac92a07',
	(select id from clubs where name = 'Lakers'),
	'permanent',
	14,
	'2024-08-28'
),
(
	'48460bd5-b119-4220-a5cc-8e3c0fc083e4',
	(select id from clubs where name = 'Coster'),
	'loan',
	7,
	'2024-11-16'
);

select * from player_registrations;

select * from players;
select * from players where players.id = 'd55d94f5-ca51-41a2-adca-ed24d10ef4c5';

update players set age = 20 where id = 'd55d94f5-ca51-41a2-adca-ed24d10ef4c5';
select * from players where id = 'd55d94f5-ca51-41a2-adca-ed24d10ef4c5';

update players set club_id = null where id = 'f02a0bbb-6a67-4884-87c2-61f3e69128ab';

delete from players where id = 'd55d94f5-ca51-41a2-adca-ed24d10ef4c5';
select * from players order by id;


select players.name, clubs.name, player_registrations.registration_type
from players
left join player_registrations
	on players.id = player_registrations.player_id
left join clubs
	on player_registrations.club_id = clubs.id
order by clubs.id;



select clubs.name, players.position, count(*)
from players 
inner join player_registrations 
	on players.id = player_registrations.player_id
inner join clubs 
	on player_registrations.club_id = clubs.id
group by clubs.id, players.position
order by clubs.id, players.position;

drop table clubs;
drop table players;
drop table player_registrations;

delete from player_registrations;

select * from clubs;





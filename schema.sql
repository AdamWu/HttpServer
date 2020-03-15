create table if not exists user(
	id int PRIMARY KEY AUTO_INCREMENT,
	name varchar(100) unique NOT NULL,
	password varchar(100) NOT NULL,
	type int DEFAULT'0' NOT NULL,
	create_time datetime DEFAULT CURRENT_TIMESTAMP NOT NULL,
	last_login_time datetime DEFAULT CURRENT_TIMESTAMP NOT NULL
) default charset=utf8;

create table if not exists room(
	id int PRIMARY KEY AUTO_INCREMENT,
	title varchar(255) NOT NULL,
	description text NOT NULL,
	time int DEFAULT'120' NOT NULL,
	attendance int DEFAULT'100' NOT NULL,
	user_id int NOT NULL,
	create_time datetime DEFAULT CURRENT_TIMESTAMP NOT NULL,
	update_time datetime DEFAULT CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,
	state int DEFAULT'0' NOT NULL,
	is_deleted tinyint(1) DEFAULT'0' NOT NULL
) default charset=utf8;

create table if not exists record(
	id int PRIMARY KEY AUTO_INCREMENT,
	room_id varchar(255) NOT NULL,
	create_time datetime NOT NULL,
	end_time datetime DEFAULT CURRENT_TIMESTAMP NOT NULL,
	file varchar(100) NOT NULL,
	is_deleted tinyint(1) DEFAULT'0' NOT NULL
) default charset=utf8;

create table if not exists scene(
	id int PRIMARY KEY AUTO_INCREMENT,
	name varchar(255) NOT NULL,
	description text NOT NULL,
	create_time datetime DEFAULT CURRENT_TIMESTAMP NOT NULL,
	file varchar(100) NOT NULL,
	is_deleted tinyint(1) DEFAULT'0' NOT NULL
) default charset=utf8;

-- insert data
insert into user(name,password,type) values('admin','123456',1);

-- Tạo cơ sở dữ liệu
CREATE DATABASE ShopManagement;
USE ShopManagement;

-- Tạo bảng role
CREATE TABLE role (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

-- Tạo bảng account
CREATE TABLE account (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(255) NOT NULL,
    password VARCHAR(255) NOT NULL,
    email VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tạo bảng account_role (liên kết giữa account và role)
CREATE TABLE account_role (
    id INT AUTO_INCREMENT PRIMARY KEY,
    account_id INT NOT NULL,
    role_id INT NOT NULL,
    FOREIGN KEY (account_id) REFERENCES account(id) on Delete CASCADE,
    FOREIGN KEY (role_id) REFERENCES role(id) on Delete CASCADE
);

-- Tạo bảng adminseller
CREATE TABLE adminseller (
    id INT AUTO_INCREMENT PRIMARY KEY,
    account_id INT NOT NULL,
    name VARCHAR(255),
    address VARCHAR(255),
    phone VARCHAR(20),
    FOREIGN KEY (account_id) REFERENCES account(id) on Delete CASCADE
);

-- Tạo bảng customer
CREATE TABLE customer (
    id INT AUTO_INCREMENT PRIMARY KEY,
    account_id INT NOT NULL,
    name VARCHAR(255),
    address VARCHAR(255),
    phone VARCHAR(20),
    image VARCHAR(255),
    FOREIGN KEY (account_id) REFERENCES account(id) on Delete CASCADE
);

-- Tạo bảng payment
CREATE TABLE payment (
    id INT AUTO_INCREMENT PRIMARY KEY,
    method VARCHAR(50) NOT NULL
);

-- Tạo bảng orders
CREATE TABLE orders (
    id INT AUTO_INCREMENT PRIMARY KEY,
    customer_id INT NOT NULL,
    adminseller_id INT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    status VARCHAR(50),
    payment_id INT,
    delivered_at DATETIME,
    expected_start_date DATETIME,
    expected_end_date DATETIME,
    total DECIMAL(10, 2),
    FOREIGN KEY (customer_id) REFERENCES customer(id) on Delete CASCADE,
    FOREIGN KEY (adminseller_id) REFERENCES adminseller(id) on Delete CASCADE,
    FOREIGN KEY (payment_id) REFERENCES payment(id) on Delete CASCADE
);
-- Tạo bảng category_details
CREATE TABLE category (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);
-- Tao bang mood_details
Create table mood(
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

Create table artist(
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

-- Tạo bảng brand
CREATE TABLE brand (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT,
    website VARCHAR(255)
);

-- Tạo bảng product
CREATE TABLE product (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
	image_url VARCHAR(255),
	description Text,
	price decimal(10,2) not null,
	adminseller_id Int,
	FOREIGN KEY (adminseller_id) REFERENCES adminseller(id) on Delete CASCADE
);

-- Tạo bảng Vinyl
Create table vinyl(
    id INT AUTO_INCREMENT PRIMARY KEY,
    disk_id varchar(255),
    product_id Int,
    years int,
    tracklist text,
    FOREIGN KEY (product_id) REFERENCES product(id) on Delete CASCADE
);

-- Tao bang phu kien 
Create table accessories(
	id int auto_increment primary key,
	product_id int,
    brand_id int,
	FOREIGN KEY (product_id) REFERENCES product(id) on Delete CASCADE ,
    FOREIGN KEY (brand_id) REFERENCES brand(id) on Delete CASCADE
);

-- Tao bang phu kien 
Create table recordplayer(
	id int auto_increment primary key,
	product_id int,
    brand_id int,
	FOREIGN KEY (product_id) REFERENCES product(id) on Delete CASCADE ,
    FOREIGN KEY (brand_id) REFERENCES brand(id) on Delete CASCADE
);

-- Tạo bảng categories
create table categories_vinyl(
	id int auto_increment primary key,
	vinyl_id int,
	category_id int,
	foreign key (vinyl_id) references vinyl(id) on Delete CASCADE,
	foreign key (category_id) references category(id) on Delete CASCADE
);
-- Tao bang mood
Create table mood_vinyl(
	id int auto_increment primary key,
	vinyl_id int,
	mood_id int,
	foreign key (mood_id) references mood(id) on Delete CASCADE,
	foreign key (vinyl_id) references vinyl(id) on Delete CASCADE
);
-- Tao bang artist
create table artist_vinyl(
	id int auto_increment primary key,
	vinyl_id int,
	artist_id int,
	foreign key (artist_id) references artist(id) on Delete CASCADE,
	foreign key (vinyl_id) references vinyl(id) on Delete CASCADE
);

-- Tạo bảng order_detail
CREATE TABLE order_detail (
    id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (order_id) REFERENCES orders(id) on Delete CASCADE,
    FOREIGN KEY (product_id) REFERENCES product(id) on Delete CASCADE
);


-- Tạo bảng cart
CREATE TABLE cart (
    id INT AUTO_INCREMENT PRIMARY KEY,
    customer_id INT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (customer_id) REFERENCES customer(id) on Delete CASCADE
);

-- Tạo bảng cart_detail
CREATE TABLE cart_detail (
    id INT AUTO_INCREMENT PRIMARY KEY,
    cart_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    price DECIMAL(10, 2),
    FOREIGN KEY (cart_id) REFERENCES cart(id) on Delete CASCADE,
    FOREIGN KEY (product_id) REFERENCES product(id) on Delete CASCADE
);

-- Tạo bảng blog
create table blog(
id int auto_increment primary key,
nameblog varchar(255),
Create_at timestamp,
admin_id int not null,
DescriBlog varchar(255),
foreign key (admin_id) references adminseller(id) on Delete CASCADE
);
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
    FOREIGN KEY (account_id) REFERENCES account(id),
    FOREIGN KEY (role_id) REFERENCES role(id)
);

-- Tạo bảng adminseller
CREATE TABLE adminseller (
    id INT AUTO_INCREMENT PRIMARY KEY,
    account_id INT NOT NULL,
    name VARCHAR(255),
    address VARCHAR(255),
    phone VARCHAR(20),
    FOREIGN KEY (account_id) REFERENCES account(id)
);

-- Tạo bảng customer
CREATE TABLE customer (
    id INT AUTO_INCREMENT PRIMARY KEY,
    account_id INT NOT NULL,
    name VARCHAR(255),
    address VARCHAR(255),
    phone VARCHAR(20),
    image VARCHAR(255),
    FOREIGN KEY (account_id) REFERENCES account(id)
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
    FOREIGN KEY (customer_id) REFERENCES customer(id),
    FOREIGN KEY (adminseller_id) REFERENCES adminseller(id),
    FOREIGN KEY (payment_id) REFERENCES payment(id)
);

-- Tạo bảng category
CREATE TABLE category (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    description TEXT
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
    description TEXT,
    image VARCHAR(255),
    price DECIMAL(10, 2) NOT NULL,
    category_id INT,
    brand_id INT,
    adminseller_id INT,
    FOREIGN KEY (category_id) REFERENCES category(id),
    FOREIGN KEY (brand_id) REFERENCES brand(id),
    FOREIGN KEY (adminseller_id) REFERENCES adminseller(id)
);

-- Tạo bảng order_detail
CREATE TABLE order_detail (
    id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (order_id) REFERENCES orders(id),
    FOREIGN KEY (product_id) REFERENCES product(id)
);


-- Tạo bảng cart
CREATE TABLE cart (
    id INT AUTO_INCREMENT PRIMARY KEY,
    customer_id INT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (customer_id) REFERENCES customer(id)
);

-- Tạo bảng cart_detail
CREATE TABLE cart_detail (
    id INT AUTO_INCREMENT PRIMARY KEY,
    cart_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL,
    price DECIMAL(10, 2),
    FOREIGN KEY (cart_id) REFERENCES cart(id),
    FOREIGN KEY (product_id) REFERENCES product(id)
);

-- Tạo bảng blog
create table blog(
id int auto_increment primary key,
nameblog varchar(255),
Create_at timestamp,
admin_id int not null,
DescriBlog varchar(255),
foreign key (admin_id) references adminseller(id)
);
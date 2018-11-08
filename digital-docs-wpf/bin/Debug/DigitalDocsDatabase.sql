CREATE TABLE Products
(
	ProductID int IDENTITY(1,1) PRIMARY KEY not null,
	ProdcutType int not null,
	Detail varchar(100) not null,
	QuantityAvailable int
);

CREATE TABLE Workers
(
	WorkerID int IDENTITY(1,1) PRIMARY KEY not null,
	Name varchar(20) not null,
	Surname varchar(30) not null,
	Email varchar(50) not null
);

CREATE TABLE Clients
(
	ClientID int IDENTITY(1,1) PRIMARY KEY not null,
	Name varchar(20) not null,
	Surname varchar(30) not null,
	Email varchar(50) not null,
	Adres varchar(100) not null
);

CREATE TABLE Orders
(
	OrderID int IDENTITY(1,1) PRIMARY KEY not null,
	ProcessPoint varchar(10) not null,
	OrderStatus varchar(50) not null,
	OrdererID int REFERENCES Clients not null,
	WorkerID int REFERENCES Workers not null
);
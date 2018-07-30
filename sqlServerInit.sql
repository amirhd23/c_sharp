IF OBJECT_ID('spProductDetail')  IS NOT NULL DROP PROCEDURE spProductDetail;
IF OBJECT_ID('spGetAllProducts') IS NOT NULL DROP PROCEDURE spGetAllProducts;
IF OBJECT_ID('spStoresByRegion') IS NOT NULL DROP PROCEDURE spStoresByRegion;
IF OBJECT_ID('spFindEmployee')   IS NOT NULL DROP PROCEDURE spFindEmployee;
IF OBJECT_ID('spFindProduct')    IS NOT NULL DROP PROCEDURE spFindProduct;
IF OBJECT_ID('Employee', 'U')    
	IS NOT NULL DROP TABLE Employee;
IF OBJECT_ID('ProductPurchaseOrder', 'U')    
	IS NOT NULL DROP TABLE ProductPurchaseOrder;
IF OBJECT_ID('PurchaseOrder', 'U')    
	IS NOT NULL DROP TABLE PurchaseOrder;
IF OBJECT_ID('ProductInvoice', 'U')   
	IS NOT NULL DROP TABLE ProductInvoice;
IF OBJECT_ID('ProductInvoiceWithQuantity', 'U')    
	IS NOT NULL DROP TABLE ProductInvoiceWithQuantity
IF OBJECT_ID('Invoice', 'U')    
	IS NOT NULL DROP TABLE Invoice;
IF OBJECT_ID('Product', 'U')    
	IS NOT NULL DROP TABLE Product;
IF OBJECT_ID('Manufacturer', 'U')    
	IS NOT NULL DROP TABLE Manufacturer;
IF OBJECT_ID('Supplier', 'U')    
	IS NOT NULL DROP TABLE Supplier;
IF OBJECT_ID('Store', 'U')    
	IS NOT NULL DROP TABLE Store;
IF OBJECT_ID('Building', 'U')    
	IS NOT NULL DROP TABLE Building;
    GO
    
CREATE TABLE Manufacturer(
	mfg        VARCHAR(25) PRIMARY KEY,
	mfgDiscount DECIMAL);
INSERT INTO Manufacturer VALUES('Duncan Hines', 10.5);
INSERT INTO Manufacturer VALUES('Florida Orange', 11.0);
INSERT INTO Manufacturer VALUES('Pilsbury', 25.0);
INSERT INTO Manufacturer VALUES('Hot House',15.0);
INSERT INTO Manufacturer VALUES('McCain', 20.0);
CREATE TABLE Supplier(
	vendor         VARCHAR(25) PRIMARY KEY,
	supplier_email VARCHAR(30));
INSERT INTO Supplier VALUES('Sysco', 'frank@sysco.com');
INSERT INTO Supplier VALUES('GFS',   'jane@gfs.com');
    GO
CREATE TABLE Product(
	productID INT	PRIMARY KEY,
	name	  VARCHAR(25),	
	mfg       VARCHAR(25) FOREIGN KEY REFERENCES Manufacturer(mfg),
	vendor  VARCHAR(25) FOREIGN KEY REFERENCES Supplier(vendor),
	price	  MONEY CHECK(price>0));
INSERT INTO Product VALUES(1, 'Cake Mix', 'Duncan Hines', 'Sysco', 2.99);
INSERT INTO Product VALUES(2, 'Cookie Dough', 'Duncan Hines', 'Sysco', 1.25);
INSERT INTO Product VALUES(3, 'Orange Juice', 'Florida Orange', 'GFS', 4.25);
INSERT INTO Product VALUES(4, 'Cookie Dough', 'Pilsbury', 'GFS', 1.45);
INSERT INTO Product VALUES(5, 'Carrots', 'Hot House', 'GFS', 1.01);
    GO
CREATE TABLE Building(
    building_name VARCHAR(20),
    unit_num      INT,
    capacity      INT,
    PRIMARY KEY(building_name, unit_num));
INSERT INTO Building VALUES('City Center', 2380, 40000);
INSERT INTO Building VALUES('City Center', 2381, 5000);
INSERT INTO Building VALUES('Vineyard Estates', 180, 20400);
INSERT INTO Building VALUES('Peaceful Place', 226, 18000);
INSERT INTO Building VALUES('Fairlane Square', 17235,45000);
    GO
CREATE TABLE Store(
	branch  VARCHAR(25) PRIMARY KEY,
	region	VARCHAR(25),
	building_name VARCHAR(20),
	unit_num INT,
	FOREIGN KEY (building_name, unit_num) 
	REFERENCES Building(building_name, unit_num));
INSERT INTO Store VALUES('Vancouver', 'BC', 'City Center', 2380);
INSERT INTO Store VALUES('Richmond', 'BC', null, null);
INSERT INTO Store VALUES('Kamloops', 'BC', 'Vineyard Estates', 180);
INSERT INTO Store VALUES('Mission', 'BC', 'Peaceful Place', 226);
INSERT INTO Store VALUES('Seattle', 'WA', 'Fairlane Square', 17235);
    GO
CREATE TABLE Invoice(
	invoiceNum INT PRIMARY KEY,
	branch	   VARCHAR(25) FOREIGN KEY REFERENCES Store(branch));
INSERT INTO Invoice VALUES(1001, 'Vancouver');
INSERT INTO Invoice VALUES(1002, 'Vancouver');
INSERT INTO Invoice VALUES(1003, 'Kamloops');
INSERT INTO Invoice VALUES(1004, 'Mission');
INSERT INTO Invoice VALUES(1005, 'Seattle');
INSERT INTO Invoice VALUES(1006, null);
    GO
CREATE TABLE ProductInvoice(
	productID	INT FOREIGN KEY REFERENCES Product(productID),
	invoiceNum	INT FOREIGN KEY REFERENCES Invoice(invoiceNum),
	PRIMARY KEY (productID, invoiceNum));
INSERT INTO ProductInvoice VALUES(1, 1001);
INSERT INTO ProductInvoice VALUES(2, 1001);
INSERT INTO ProductInvoice VALUES(3, 1001);
INSERT INTO ProductInvoice VALUES(4, 1002);
INSERT INTO ProductInvoice VALUES(2, 1003);
INSERT INTO ProductInvoice VALUES(3, 1003);
INSERT INTO ProductInvoice VALUES(1, 1004);
INSERT INTO ProductInvoice VALUES(2, 1004);
INSERT INTO ProductInvoice VALUES(3, 1004);
INSERT INTO ProductInvoice VALUES(3, 1005);
    GO
CREATE TABLE ProductInvoiceWithQuantity(
	productID	INT FOREIGN KEY REFERENCES Product(productID),
	invoiceNum	INT FOREIGN KEY REFERENCES Invoice(invoiceNum),
	quantity    INT,
	PRIMARY KEY (productID, invoiceNum));
INSERT INTO ProductInvoiceWithQuantity VALUES(1, 1001, 3);
INSERT INTO ProductInvoiceWithQuantity VALUES(2, 1001, 2);
INSERT INTO ProductInvoiceWithQuantity VALUES(3, 1001, 1);
INSERT INTO ProductInvoiceWithQuantity VALUES(4, 1002, 3);
INSERT INTO ProductInvoiceWithQuantity VALUES(2, 1003, 2);
INSERT INTO ProductInvoiceWithQuantity VALUES(3, 1003, 2);
INSERT INTO ProductInvoiceWithQuantity VALUES(1, 1004, 4);
    GO

CREATE TABLE PurchaseOrder(
    po_num  INT PRIMARY KEY,
    branch  VARCHAR(25) FOREIGN KEY REFERENCES Store(branch));
INSERT INTO PurchaseOrder VALUES (100, 'Vancouver');
INSERT INTO PurchaseOrder VALUES (101, 'Seattle');
INSERT INTO PurchaseOrder VALUES (102, 'Vancouver');
    GO
CREATE TABLE ProductPurchaseOrder(
	productID INT FOREIGN KEY REFERENCES Product(productID),
	po_num    INT FOREIGN KEY REFERENCES PurchaseOrder(po_num),
	PRIMARY KEY (productID, po_num));
INSERT INTO ProductPurchaseOrder VALUES(1, 100);
INSERT INTO ProductPurchaseOrder VALUES(2, 100);
INSERT INTO ProductPurchaseOrder VALUES(1, 101);
INSERT INTO ProductPurchaseOrder VALUES(3, 101);
INSERT INTO ProductPurchaseOrder VALUES(4, 102);
    GO
CREATE TABLE Employee(
	employee_id	INT PRIMARY KEY,
	last_name   VARCHAR(25),
	first_name  VARCHAR(25),
	branch VARCHAR(25) FOREIGN KEY REFERENCES Store(branch));
INSERT INTO Employee VALUES(9001, 'Chen', 'Jane', 'Vancouver');
INSERT INTO Employee VALUES(9002, 'Johnson', 'Jeff', 'Kamloops');
INSERT INTO Employee VALUES(9003, 'Rogers', 'Pam', 'Richmond');
INSERT INTO Employee VALUES(9004, 'Singh', 'Baljeet', 'Seattle');
    GO

CREATE PROCEDURE spProductDetail (@name VARCHAR(25), @vendor VARCHAR(25))
    AS SELECT * FROM Product WHERE @name = name AND @vendor = vendor;
    GO
CREATE PROCEDURE spGetAllProducts 
    AS SELECT * FROM Product;
    GO
CREATE PROCEDURE spStoresByRegion (@region VARCHAR(25))
    AS SELECT    Store.region, Store.building_name FROM Store 
    WHERE        region = @region;
    GO
CREATE PROCEDURE spFindEmployee(@lname VARCHAR(25)) AS
    SELECT * FROM Employee WHERE Employee.last_name = @lname;
    GO
CREATE PROCEDURE spFindProduct(@productID INT) AS
    SELECT productID, name, price FROM Product 
    WHERE productID = @productID;
    GO

DROP DATABASE IF EXISTS capgeminiassess;

CREATE DATABASE CAPGEMINIaSSESS;
use capgeminiassess;
CREATE TABLE Sales_Raw
(
    OrderID INT primary Key,

    OrderDate VARCHAR(20),

    CustomerName VARCHAR(100),

    CustomerPhone VARCHAR(20),

    CustomerCity VARCHAR(50),

    ProductNames VARCHAR(200),

    Quantities VARCHAR(100),

    UnitPrices VARCHAR(100),

    SalesPerson VARCHAR(100)
);


INSERT INTO Sales_Raw VALUES
(101, '2024-01-05', 'Ravi Kumar', '9876543210', 'Chennai', 'Laptop,Mouse', '1,2', '55000,500', 'Anitha'),(102, '2024-01-06', 'Priya Sharma', '9123456789', 'Bangalore', 'Keyboard,Mouse', '1,1', '1500,500', 'Anitha'),(103, '2024-01-10', 'Ravi Kumar', '9876543210', 'Chennai', 'Laptop', '1', '54000', 'Suresh'),(104, '2024-02-01', 'John Peter', '9988776655', 'Hyderabad', 'Monitor,Mouse', '1,1', '12000,500', 'Anitha'),
(105, '2024-02-10', 'Priya Sharma', '9123456789', 'Bangalore', 'Laptop,Keyboard', '1,1', '56000,1500', 'Suresh');


create table Customers (
	CustomerId int primary key auto_increment,
    CustomerName VARCHAR(100),
    CustomerPhone VARCHAR(20),
    CustomerCity VARCHAR(50) );
    
CREATE TABLE SalesPersons (
    SalesPersonID INT PRIMARY KEY AUTO_INCREMENT,
    SalesPersonName VARCHAR(100)
);


CREATE TABLE Products (
    ProductID INT PRIMARY KEY AUTO_INCREMENT,
    ProductName VARCHAR(100),
    UnitPrice DECIMAL(10,2)
);

CREATE TABLE Orders (
    OrderID INT PRIMARY KEY,
    OrderDate DATE,
    CustomerID INT,
    SalesPersonID INT,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (SalesPersonID) REFERENCES SalesPersons(SalesPersonID)
);

CREATE TABLE OrderItems (
    OrderItemID INT PRIMARY KEY AUTO_INCREMENT,
    OrderID INT,
    ProductID INT,
    Quantity INT,
    UnitPrice DECIMAL(10,2),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

WITH OrderTotals AS (
    SELECT
	sr.OrderID,
	SUM(q.qty * p.price) AS TotalSales
    FROM Sales_Raw sr
    JOIN JSON_TABLE(
        CONCAT('[', sr.Quantities, ']'),
        '$[*]' COLUMNS (qty INT PATH '$')
    ) q
    JOIN JSON_TABLE(
        CONCAT('[', sr.UnitPrices, ']'),
        '$[*]' COLUMNS (price INT PATH '$')
    ) p
    GROUP BY sr.OrderID
)
SELECT OrderID, TotalSales
FROM (
SELECT *,
DENSE_RANK() OVER (ORDER BY TotalSales DESC) AS rnk
FROM OrderTotals
) t
WHERE rnk = 3;

SELECT
sr.SalesPerson,
SUM(q.qty * p.price) AS TotalSales
FROM Sales_Raw sr
JOIN JSON_TABLE(
	CONCAT('[', sr.Quantities, ']'),
	'$[*]' COLUMNS (qty INT PATH '$')
) q
JOIN JSON_TABLE(
	CONCAT('[', sr.UnitPrices, ']'),
	'$[*]' COLUMNS (price INT PATH '$')
) p
GROUP BY sr.SalesPerson
HAVING SUM(q.qty * p.price) > 60000;

WITH CustomerTotals AS (
    SELECT 
	sr.CustomerName,
	SUM(q.qty * p.price) AS TotalSpent
    FROM Sales_Raw sr
    JOIN JSON_TABLE(CONCAT('[', sr.Quantities, ']'),
		'$[*]' COLUMNS (qty INT PATH '$')) q
    JOIN JSON_TABLE(CONCAT('[', sr.UnitPrices, ']'),
		'$[*]' COLUMNS (price INT PATH '$')) p
    GROUP BY sr.CustomerName
)
SELECT * FROM CustomerTotals
WHERE TotalSpent > (SELECT AVG(TotalSpent) FROM CustomerTotals);

INSERT INTO Sales_Raw VALUES
(106, '2026-01-08', 'Amit Singh', '9876501234',
 'Delhi', 'Laptop', '1', '55000', 'Suresh');

SELECT
UPPER(CustomerName) AS CustomerName,
MONTH(STR_TO_DATE(OrderDate, '%Y-%m-%d')) AS OrderMonth
FROM Sales_Raw
WHERE YEAR(STR_TO_DATE(OrderDate, '%Y-%m-%d')) = 2026
AND MONTH(STR_TO_DATE(OrderDate, '%Y-%m-%d')) = 1;
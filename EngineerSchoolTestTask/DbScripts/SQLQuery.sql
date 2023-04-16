
-----------------
--- Задание 1 ---
-----------------

-- Очевидный вариант
SELECT *
FROM Employee
WHERE Salary = (SELECT MAX(Salary) FROM Employee);

-- Реляционная алгебра
SELECT Emp1.Id, Emp1.DepartmentId, Emp1.ChiefId, Emp1.Name, Emp1.Salary
FROM Employee AS Emp1 
LEFT JOIN Employee AS Emp2 ON Emp1.Salary < Emp2.Salary
WHERE Emp2.Salary IS NULL;

-- Прокатило бы, если бы он был точно один единственный
SELECT TOP 1 *
FROM Employee
ORDER BY Salary DESC;


-----------------
--- Задание 2 ---
-----------------

-- В Oracle CONNECT BY

WITH Hierarchical AS (
    SELECT Id, DepartmentId, ChiefId, Name, Salary, 1 AS Depth
    FROM Employee
    WHERE ChiefId IS NULL 
    UNION ALL
    SELECT emp.Id, emp.DepartmentId, emp.ChiefId, emp.Name, emp.Salary, h.Depth + 1 AS Depth 
    FROM Employee AS emp
    INNER JOIN Hierarchical h
    ON h.id = emp.ChiefId
)
SELECT MAX(Depth) AS MaxDepth FROM Hierarchical;


-----------------
--- Задание 3 ---
-----------------

WITH DepartmentSumSalaries AS (
SELECT d.Id, d.Name, Sum(e.Salary) AS SumSalaries
FROM Department AS d
JOIN Employee AS e
ON d.Id = e.DepartmentId
GROUP BY d.Id, d.Name)

SELECT * 
FROM DepartmentSumSalaries
WHERE SumSalaries = (SELECT Max(SumSalaries) FROM DepartmentSumSalaries);


-----------------
--- Задание 4 ---
-----------------

SELECT * 
FROM Employee
WHERE Name LIKE N'[А-ЯA-Z][а-яa-z]% Р[а-яa-z]%н [А-ЯA-Z][а-яa-z]%';





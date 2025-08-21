create database employ
create table department (
	dep_ID int primary key Identity(1,1),
	name varchar(255)
);
create table employees (
	ID int primary KEY IDENTITY(1,1),
	name varchar(255),
	salary int,
	dep_ID INT
    FOREIGN KEY (dep_ID) REFERENCES department(dep_ID)
);

-- table form mangers realtion betweeen emp and dep
-- grouping by departname mangname total salary for emp to the d
INSERT INTO department (name) VALUES
('IT'),
('HR'),
('Sales');
INSERT INTO employees VALUES
('Alice Johnson', 80000, 1),
('Bob Williams', 95000, 1),
('Grace Hopper', 9000, 1);
INSERT INTO employees VALUES
('Charlie Brown', 52000, 2),
('Diana Prince', 60000, 2);
INSERT INTO employees VALUES
('Ethan Hunt', 48000, 3),
('Fiona Glenanne', 120000, 3);
go


create procedure addOneK
as
	update employees
    set salary = salary + 1000;
go

create procedure add25Per
as
	update employees
    set salary = salary + salary * 0.25;
go

create procedure addPerCateg
as
BEGIN
	update employees
    set salary = salary * 1.25
    where salary <= 10000;

    update employees
    set salary = salary * 1.15
    where salary > 10000 AND salary <= 50000;

    update employees
    set salary = salary * 1.10
    where salary > 50000 AND salary <= 100000;

    update employees
    set salary = salary * 1.05
    where salary > 100000;
END;
go

create procedure showEmpPerDep
as
	select employees.* , department.name as depName
	from employees join department on 
	employees.dep_ID = department.dep_ID;
go

create procedure secHigherEmp
as
	select name , salary as secMaxSalary
	from employees
	where salary = (select MAX(salary) from employees where salary < (select MAX(salary) from employees) )
go

create procedure tierListDep
as
	select top 1 department.name , SUM(salary) as total_Salary_per_Dep
	from department join employees on
	department.dep_ID = employees.dep_ID
	group by department.name
	order by total_Salary_per_Dep desc
go


exec addOneK
exec add25Per
exec addPerCateg
exec showEmpPerDep
exec secHigherEmp
exec tierListDep

select * from employees
order by salary asc;
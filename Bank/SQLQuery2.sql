
CREATE TABLE Клиенты (
  ID_клиента INT PRIMARY KEY,
  Имя VARCHAR(255) NOT NULL,
  Адрес VARCHAR(255)
);

CREATE TABLE Счета (
  Номер_счета INT PRIMARY KEY,
  ID_клиента INT REFERENCES Клиенты(ID_клиента),
  Тип_счета VARCHAR(255),
  Баланс DECIMAL(10, 2) DEFAULT 0.00
);

CREATE TABLE Кредиты (
  ID_кредита INT PRIMARY KEY,
  Учетная_запись INT REFERENCES Счета(Номер_счета),
  Сумма_кредита DECIMAL(10, 2) NOT NULL,
  Процентная_ставка DECIMAL(5, 2) NOT NULL,
  Срок_погашения INT NOT NULL,
  Ежемесячный_платеж DECIMAL(10, 2)
);

CREATE TABLE Платежи_по_кредитам (
  ID_платежа_кредита INT PRIMARY KEY,
  Кредит INT REFERENCES Кредиты(ID_кредита),
  Сумма_платежа DECIMAL(10, 2) NOT NULL,
  Дата_платежа DATE
);

CREATE TABLE Транзакции (
  ID_транзакции INT PRIMARY KEY,
  Учетная_запись INT REFERENCES Счета(Номер_счета),
  Сумма_транзакции DECIMAL(10, 2),
  Тип_транзакции VARCHAR(255),
  Дата_транзакции DATE
);

CREATE TABLE Отделение_банка (
  ID_отдела INT PRIMARY KEY,
  Название_отдела VARCHAR(255) NOT NULL,
  Адрес_отдела VARCHAR(255)
);

CREATE TABLE Сотрудники (
  ID_сотрудника INT PRIMARY KEY,
  Имя VARCHAR(255) NOT NULL,
  Должность VARCHAR(255),
  Отдел INT REFERENCES Отделение_банка(ID_отдела)
);

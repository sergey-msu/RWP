USE [RWP];
GO

INSERT INTO [dbo].[Doctor] ([FirstName], [MiddleName], [LastName], [Position], [Print], [Note]) VALUES
('����', '���������', '��������', '������', null, null);
GO

INSERT INTO [dbo].[Customer] ([Name], [Address], [ContactName], [ContactEMail], [Note]) VALUES
('��������� ����������� ��������', '��. ���������, �. 256', '������ ���� ��������', 'ivanov@mail.ru', null);
GO

INSERT INTO [dbo].[Patient] ([FirstName], [MiddleName], [LastName], [DOB], [Sex], [Note]) VALUES
('Sergey', null, 'Kozlov', '1960-06-19', 1, null),
('Ivan', '��������', '�����', '1990-06-19', 1, null),
('Petr', '�����', '����������', '1969-06-19', 1, null),
('�������', null, 'Kozlov', '1978-07-29', 0, '�����'),
('�����', null, 'Kozlov', '1978-06-28', 0, null),
('�����', '����������', '���������', '1957-07-09', 1, '���������'),
('�����', null, '���������', '1988-06-01', 0, null);
GO

INSERT INTO [dbo].[MedicalResearch] ([IdCustomer], [IdPatient], [IdDoctor], [IdResearchTemplate], [ExaminationDate], [ResearchDate], [Number], [SliceThickness], [UseContrast], [Content], [Conclusion]) VALUES
(1, 1, 1, 1, '2016-06-19', '2016-06-20', 'NUM0001-A', '13', 0, '... ������������ ...', '... ���������� ...'),
(1, 1, 1, 1, '2016-06-19', '2016-06-20', 'NUM0002-A', '10', 0, '... ������������ ...', '... ���������� ...'),
(1, 1, 1, 1, '2016-06-19', '2016-06-20', 'NUM0003-A', '17', 1, '... ������������ ...', '... ���������� ...'),
(1, 2, 1, 1, '2016-06-22', '2016-06-23', 'NUM0004-A', '13', 0, '... ������������ ...', '... ���������� ...'),
(1, 2, 1, 1, '2016-06-22', '2016-06-23', 'NUM0005-A', '16', 1, '... ������������ ...', '... ���������� ...'),
(1, 3, 1, 1, '2016-06-22', '2016-06-23', 'NUM0006-A', '16', 0, '... ������������ ...', '... ���������� ...'),
(1, 4, 1, 1, '2016-07-26', '2016-07-27', 'NUM0007-A', '5',  1, '... ������������ ...', '... ���������� ...'),
(1, 7, 1, 1, '2016-07-26', '2016-07-27', 'NUM0007-A', '7',  0, '... ������������ ...', '... ���������� ...');
GO

INSERT INTO [dbo].[MedicalResearchScope] ([IdMedicalResearch], [IdResearchScope]) VALUES
(1, 1), (1, 3), (1, 4),
(2, 1),
(3, 2),
(4, 2), (4, 3),
(5, 1),
(6, 1), (6, 2), (6, 3),
(7, 1), (7, 2),
(8, 5);
GO

INSERT INTO [dbo].[MedicalScanRegime] ([IdMedicalResearch], [IdScanRegime]) VALUES
(1, 1), (1, 2),
(2, 1),
(3, 2),
(4, 2), (4, 1),
(5, 1),
(6, 1), (6, 2),
(7, 1), (7, 2),
(8, 1);
GO
# Core-Banking-App-Demo
A core banking application for managing banking processes. Processes like setting up customers, bank tellers, deposit and withdrawal, running end of day transactions, displaying profit and loss statements, configuration of Bank ATM Terminals can be done using this application. 
It can be used by Single branch or Multi branch Banks
This application was developed using Asp.Net(C#). It makes use of either Nhibernate or ADO.Net as ORMs, and this can be easily switched via the Dependency Injection functionalities built in the app.



**PROJECTS IN SOLUTION**
1) CBA.Data.ADO.Net - Contains the Database access logic using ADO.Net
2) CBA.Data.NHibernate - Contains the Database access logic using NHibernate
3) CBA.Data.NHibernate - Contains the Dependency Injection functionalities
4) CBA.EODService - End of Day (EOD) Service that runs in banks to balance all records at the end of each day
5) CBAPractice.Core - Core Class definitions for the application
6) CBAPractice.Data - Contains methods for reuseable queries that utilize the Database access in 3 above
7) CBAPractice.Logic - Contains the business logic of the application
8) CBAPractice - The web application (containing all web pages in the app)
9) FEP -  A replica of a Bank's Front End Processor, meant to recieve financial (ISO8583) messages from all channels (ATM,POS,WEB) 
10) FEP.WindowsService - A windows service built to serve same purpose as FEP (number 9 above)

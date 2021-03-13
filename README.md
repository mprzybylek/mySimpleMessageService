# mySimpleMessageService
Simple message service

Used technologies:
Automapper,
Odata,
Swagger,
.NET5,
ASP MVC CORE

System architecture:

![alt text](https://github.com/mprzybylek/mySimpleMessageService/blob/main/Images/Architecture.jpg)



Layers :

API
Domain ( logic of application
Persistance ( db layer )


Requirements:
Messages
Send a message to another contact within the engine.
Read messages
Delete a message
Apply filtering, pagination and sorting

Contacts
CRUD operations

Sample sorting: GET /api/Contacts?$orderby=name%20desc 
Sample filtering: GET /api/Contacts?$filter=name%20eq%20%27Mateusz%27 
Sample pagination: GET /api/Contacts?$skip=1&$top=1 

Others:
Unit tests

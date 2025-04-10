📝 Todos API
A simple and extensible ASP.NET Core Web API for managing Todo items with support for filtering, weather condition integration, and CRUD operations.

📦 Project Structure
This API exposes endpoints to perform various operations on Todos such as creating, updating, deleting, searching, and fetching todos with optional filters. It also includes weather condition fetching for a specific task.

🚀 Features
✅ Create, Update, and Delete Todo items

🔍 Search Todos by title, location, and due date

🌤️ Fetch weather data for a task based on ID

🔄 API responses wrapped in IActionResult

📋 Clean service abstraction via ITodoService

📚 Integrated logging via ILogger

📂 Controller: TodosController
Base Route: api/todos
Endpoints
Method	Route	                      Description
GET	    /fetch-todo-data	          Get all Todos with optional filters (location, dueDate, title)
GET    	/weather-condition	        Get weather for a specific Todo (id)
POST	  /post-todos-item	          Create a new Todo
PUT	    /update-todos-item	        Update an existing Todo
DELETE	/delete-todos-item/{id}	    Delete a Todo by ID
POST	  /search-todos-item	        Search Todos by filters (e.g., due date)

📌 Models Used
TodosModel: Represents the data structure for creating a Todo.

TodosUpdateRapper: Wrapper model for updating Todo items.

DataSearch: Used for advanced search capabilities.

🛠️ Requirements
.NET 6.0+

Visual Studio 2022 / VS Code

⚙️ How to Run
Clone the repository

Install dependencies

Configure your services and logging in Program.cs

Run the API:
dotnet run

❗ Possible Improvements
Add authentication/authorization (e.g., JWT)

Async cancellation tokens for graceful shutdown

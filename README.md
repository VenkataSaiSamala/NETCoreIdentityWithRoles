It is a sample project leveraging Authentication and Authorization using ASP.NET Core Identity in .NET Core v8.

I have used SQLite as the database with Entity Framework Core for migrations

Roles: User, Admin, Manager

Operations done by Accounts Controller:
  
	-- Registering user
    -- Properties
      - Username
      - Email
      - Password
    -- Returns Success if created or Bad Request when error is thrown.
 
	-- Login 
    -- Properties
      - Username
      - Password
    -- Returns Jwt Token if credentials are right else throws Bad Request error.
  
	-- Add-Role (One role at a time)
    -- Property
      - {RoleName} -> string
    -- Returns Success if created or Bad Request when error is thrown.
  
	-- Assign-Role
    -- Properties
      - Username
      - Role name (Must present in database) 
    -- Returns Assigned Role Successfully !!! if username and existance of role else throws bad request error.
  
	-- Unassign-Role
    -- Properties
      - Username
      - Role
    -- Returns Assigned Role Successfully !!! if username and existance of role else throws bad request error.
    -- To test this scenario we need to call login and create a new session.
    

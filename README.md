# Resource Manager - Interview Task

## How to start?

1. Build project using: `dotnet build` or IDE
1. Provide database connection string, for example:
   ```
    "DefaultConnection": "Data Source=localhost;Initial Catalog=ResourceManagerDB;User Id=[USER];Password=[Password];TrustServerCertificate=True;"
   ```
1. Apply migrations using: 
   ```
    dotnet ef database update -p .\src\ResourceManager.Infrastructure -s .\src\ResourceManager.Api\
   ```
1. Run the startup project `ResourceManager.Api`


## How to Use?
This project contains a mocked authentication to simulate real life world. Database is created with two default users.
Admin and a Standard user. You can list all users without authentication.

To perform operations as a specific user, you need to Authenticate.

### Authorization
- List users using GET to `\Users`
- Choose UserId to include in `\Identity\token`
- Authorize using `Authorize` button, or by sending received token as Bearer. Curl example:
    ```
    curl -X 'GET' \
      'https://localhost:7037/Resources' \
      -H 'accept: */*' \
      -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIzMTNlYmY5Ny1lZGIxLTRhZDAtYmU5Yy1kZjI2ZDc5MDQyNGEiLCJzdWIiOiJhZG1pbkBybS5pbyIsImVtYWlsIjoiYWRtaW5Acm0uaW8iLCJ1c2VySWQiOiIzZmE4NWY2NC01NzE3LTQ1NjItYjNmYy0yYzk2M2Y2NmFmYTYiLCJ1c2VyUm9sZSI6IkFkbWluIiwidXNlckVtYWlsIjoiYWRtaW5Acm0uaW8iLCJuYmYiOjE3MDA4MTA2MDIsImV4cCI6MTcwMDgzOTQwMiwiaWF0IjoxNzAwODEwNjAyLCJpc3MiOiJodHRwczovL2lkLnJlc291cmNlbWFuYWdlci5jb20iLCJhdWQiOiJodHRwczovL2FwaS5yZXNvdXJjZW1hbmFnZXIuY29tIn0.hxGiA1lAuaZka9H8DriwMR95E8VcKbYcEd3IuCx4TlA'
    ```

## Assumptions
As only 8h were available for the task, I've made the following assumptions:
- Resource can be locked once at a time.
  However, database is designed in a way that it would be easy to transition into more complex locking.
- When Resource is Withdrawn, locks are removed
- Only a sample of tests has been added
- Resources are Withdrawn forever (subject to a simple change)
- Lock, Withdraw and Unlock operations are idempotent and locking resource by the same user many times will always succeed.

## What would be the next steps if given more time?
1. Add Integrations tests and more Unit Tests
2. Some classes are a bit large, it would be great to revisit them
3. In a real-world and Identity Server should be provided. **Current way of authentication is just a mock**.
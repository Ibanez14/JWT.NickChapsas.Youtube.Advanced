1) Configure Swagger
-- Look at startup

2) Implement Versioning of API
-- Take a look ad ApiRoutes and ValuesController class

3) Clean Service Registation via assembly reflection and IIInstaller interface implementation
-- take a look at Startup adn InstallerExtensions class
-- (regions in InstallerExtensions Clean Service Registration) and DataInstaller & MVCInstaller classes

4) Resource creating good practise 
=> During POST method, after createing model 
it's better to use retunr Created() that will 
send in response the URI to get this method 
(source code github Nick Chapsas (ValuesController/Create))

5) Nice to have Response and Request folders respectively having models inside
6)  Using [FromRoute] and [FromBody] attribute to get parameter from ur query
7) HttpPut is for update and HttpPost is for adding or creating a model
8) It's good to return NoConten() in Delete action after deleting a model. If htere isn't found model to delete, use NotFound()
9) JWT Token => 
- Configure in appsettings
- Add JwtAuthenticationInstaller.cs
- Add authentication features in Swagger
- JwtAuthentication section in SwaggerInstaller.cs
- App.UseAuthentication()

10 ) User Registration and Controller Auth
- Added IdentityController which registers and logins users

11) JWT Authenticaion User Login
 - Updated IdentityController

 12) User specific content with JWT claims


 13) Refreshing JWT with Refresh Token
 





## Tags:
Youtube
Nick Chapsas
Swagger
API Versioning
JWT
.NET Core
WebAPI
Clean Service Registration with Reflection
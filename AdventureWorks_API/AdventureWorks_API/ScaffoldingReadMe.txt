Support for ASP.NET Core Identity was added to your project.

For setup and configuration information, see https://go.microsoft.com/fwlink/?linkid=2116645.

to generate Security database into existing database we need to :

1- Run "add-migration updateIndentity -context AdventureWorks_APIContext" with AdventureWorks_APIContext created in Areas\Identity\Data by right
   click and chooose Add Scafording Item\Identity
2_ Run "update-database -context AdventureWorks_APIContext"


https://quizdeveloper.com/tips/jwt-authentication-and-refresh-token-in-aspdotnet-core-web-api-aid111?fbclid=IwAR1FyE1R6MZL-efhIa8dYRMQ1yX4DawS6nQrRCbCVKAdwMAwHE-HREh0e3c

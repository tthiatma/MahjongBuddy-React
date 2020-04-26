# MahjongBuddy with ASP.Net Core 3 + React Typescript + Mobx 

## Setup
In order for the application to work, navigate to **MahjongBuddy.API** folder and do below steps: 
- Add user secret for Token key by running  `dotnet user-secrets set "TokenKey" "[InsertLongSecretKeyHere]"`
- create a new file called **appsettings.json** like below as example

```json
{
    "ConnectionStrings":{
        "DefaultConnection": "Data Source=MahjongBuddy.db"
    },
    "Logging": {
    "LogLevel": {
        "Default": "Information",
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    "AllowedHosts": "*"
}
```
## IDE & Software
- [.Net Core 3](https://dotnet.microsoft.com/download)
- [Node](https://nodejs.org/en/download/)
- [Visual studio code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

### Recommended Visual studio code extension:
- C#
- C# Extensions
- Auto Close Tag
- Auto Rename Tag
- Bracket Pair Colorizer 2  
- ES7 React/Redux/GraphQL/React-Native snippets
- Material Icon Theme
- Nuget Package Manager
- Prettier - Code Formatter
- SQLite
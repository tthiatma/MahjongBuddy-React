# MahjongBuddy-React
In order the application to work navigate to MahjongBuddy.API folder 
1.) run dotnet user-secrets set "TokenKey" "[InsertSecretKeyHere]"
2.) create a new file appsettings.json like below as example


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

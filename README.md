# AspNetCore
A simple example of ASP.Net Core project

## Build
    $files=@("SoaWebsite.Common\project.json", "SoaWebsite.Services\project.json", "SoaWebsite.Web\project.json")
    dotnet restore $files
    dotnet build $files

## Run
Services :

    cd SoaWebsite.Services; dotnet run --server.urls=http://*:5555/

Website :

    cd SoaWebsite.Web; dotnet run 

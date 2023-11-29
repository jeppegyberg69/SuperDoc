FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

ENV HTTP_PORT=80


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SuperDoc.Customer.API/SuperDoc.Customer.API.csproj", "SuperDoc.Customer.API/"]
RUN dotnet restore "SuperDoc.Customer.API/SuperDoc.Customer.API.csproj"
COPY . .
WORKDIR "/src/SuperDoc.Customer.API"
RUN dotnet build "SuperDoc.Customer.API.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/SuperDoc.Customer.API"
RUN dotnet publish "SuperDoc.Customer.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .



# Set environment variables for ports
ENV ASPNETCORE_URLS=http://+:$HTTP_PORT

ENTRYPOINT ["dotnet", "SuperDoc.Customer.API.dll"]

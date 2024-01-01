FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/CleanArchitecture.Api/CleanArchitecture.Api.csproj", "CleanArchitecture.Api/"]
COPY ["src/CleanArchitecture.Application/CleanArchitecture.Application.csproj", "CleanArchitecture.Application/"]
COPY ["src/CleanArchitecture.Domain/CleanArchitecture.Domain.csproj", "CleanArchitecture.Domain/"]
COPY ["src/CleanArchitecture.Contracts/CleanArchitecture.Contracts.csproj", "CleanArchitecture.Contracts/"]
COPY ["src/CleanArchitecture.Infrastructure/CleanArchitecture.Infrastructure.csproj", "CleanArchitecture.Infrastructure/"]
COPY ["Directory.Packages.props", "./"]
COPY ["Directory.Build.props", "./"]
RUN dotnet restore "CleanArchitecture.Api/CleanArchitecture.Api.csproj"
COPY . ../
WORKDIR /src/CleanArchitecture.Api
RUN dotnet build "CleanArchitecture.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_HTTP_PORTS=5001
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitecture.Api.dll"]
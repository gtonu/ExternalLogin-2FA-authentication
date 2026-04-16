FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
RUN apt update && apt install -y nodejs
COPY ["ExternalLoginAnd2FA.Web/ExternalLoginAnd2FA.Web.csproj","ExternalLoginAnd2FA.Web/"]
COPY ["ExternalLoginAnd2FA.Domain/ExternalLoginAnd2FA.Domain.csproj","ExtenalLoginAnd2FA.Domain/"]
COPY ["ExternalLoginAnd2FA.Infrastructure/ExternalLoginAnd2FA.Infrastructure.csproj","ExternalLoginAnd2FA.Infrastructure/"]
COPY ["ExternalLoginAnd2FA.Application/ExternalLoginAnd2FA.Application.csproj","ExternalLoginAnd2FA.Application/"]

RUN dotnet restore "ExternalLoginAnd2FA.Web/ExternalLoginAnd2FA.Web.csproj"

COPY . .
WORKDIR "/src/ExternalLoginAnd2FA.Web"
RUN dotnet build "ExternalLoginAnd2FA.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ExternalLoginAnd2FA.Web.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 80
ENTRYPOINT ["dotnet","ExternalLoginAnd2FA.Web.dll"]


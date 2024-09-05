### CONTEXT AT PROJECTS FOLDER ###
# Build the dotnet api
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
EXPOSE 8080
WORKDIR /app
ENV IS_DOCKER=TRUE
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk update  \
	&& apk add icu


# DOTNET API
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS builder
WORKDIR /app/dotnet
COPY . .
RUN pwd
RUN ls
RUN dotnet restore ./Discord.Bot.WebUI
RUN dotnet build ./Discord.Bot.WebUI -c release -o /app/dotnet/build --property WarningLevel=0
RUN dotnet dev-certs https 

FROM builder AS publish
# install deps
RUN apk update \
	&& apk upgrade \
	&& apk add --update npm \
	&& echo "NPM INSTALLED"

RUN dotnet publish ./Discord.Bot.WebUI -c release -o /app/dotnet/publish --property WarningLevel=0

FROM base AS final
WORKDIR /app/dotnet
COPY --from=publish /app/dotnet/publish .
ENTRYPOINT ["dotnet", "Discord.Bot.WebUI.dll"]


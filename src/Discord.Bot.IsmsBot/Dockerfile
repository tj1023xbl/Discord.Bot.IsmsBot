### CONTEXT AT PROJECTS FOLDER ###
# Build the dotnet api
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
ENV IS_DOCKER TRUE
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk update  \
	&& apk add icu


# DOTNET API
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS builder
WORKDIR /app/dotnet
COPY . .
RUN dotnet restore ./Discord.Bot.IsmsBot
RUN dotnet build ./Discord.Bot.IsmsBot -c release -o /app/dotnet/build --property WarningLevel=0

FROM builder as publish
RUN dotnet publish ./Discord.Bot.IsmsBot -c release -o /app/dotnet/publish --property WarningLevel=0

FROM base as final
WORKDIR /app/dotnet
COPY --from=publish /app/dotnet/publish .
ENTRYPOINT ["dotnet", "Discord.Bot.IsmsBot.dll"]


### CONTEXT AT PROJECTS FOLDER ###
#
# 1) Set dir to C:\Users\tyler\OneDrive\FullStak Developers\IsmsBot\Discord.Bot.IsmsBot\src
# 2) docker build -t ismsbot:latest . -f .\Discord.Bot.IsmsBot\Dockerfile
# 3) ???????
# 4) profit
#


# Build the dotnet api
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
ENV IS_DOCKER=TRUE
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk update  \
	&& apk add icu


# DOTNET API
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS builder
WORKDIR /app/dotnet
COPY . .
RUN dotnet restore ./Discord.Bot.IsmsBot
RUN dotnet build ./Discord.Bot.IsmsBot -c release -o /app/dotnet/build --property WarningLevel=0

FROM builder AS publish
RUN dotnet publish ./Discord.Bot.IsmsBot -c release -o /app/dotnet/publish --property WarningLevel=0

FROM base AS final
WORKDIR /app/dotnet
COPY --from=publish /app/dotnet/publish .
ENTRYPOINT ["dotnet", "Discord.Bot.IsmsBot.dll"]


FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY ./*.sln ./NuGet.config  ./

# Copy the main source project files
COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done

# Copy the test project files
COPY test/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p test/${file%.*}/ && mv $file test/${file%.*}/; done

RUN dotnet restore

# copy and build everything else
COPY src/. ./src/
COPY test/. ./test/

RUN dotnet build

FROM build AS publish
WORKDIR /source
RUN dotnet publish --output bin/publish --configuration Release

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=publish /source/src/EventHorizon.Game.Server.Agent/bin/publish ./
ENTRYPOINT ["dotnet", "EventHorizon.Game.Server.Agent.dll"]
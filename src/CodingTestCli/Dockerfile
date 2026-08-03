FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CodingTest.sln ./
COPY Directory.Build.props ./
COPY src/CodingTest/CodingTest.csproj src/CodingTest/
COPY src/CodingTestCli/CodingTestCli.csproj src/CodingTestCli/

RUN dotnet restore src/CodingTestCli/CodingTestCli.csproj

COPY src/ src/
RUN dotnet publish src/CodingTestCli/CodingTestCli.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app

RUN groupadd --system --gid 1000 appgroup \
    && useradd --system --uid 1000 --gid appgroup --no-create-home appuser

COPY --from=build --chown=appuser:appgroup /app/publish .

USER appuser

ENTRYPOINT ["dotnet", "CodingTestCli.dll"]

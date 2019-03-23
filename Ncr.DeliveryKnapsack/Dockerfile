FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Ncr.DeliveryKnapsack.csproj", ""]
RUN dotnet restore "Ncr.DeliveryKnapsack.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Ncr.DeliveryKnapsack.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Ncr.DeliveryKnapsack.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Ncr.DeliveryKnapsack.dll"]
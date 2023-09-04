# EStore - PRN211 - Assignment 3

## Setup
In EStore folder, create `appsettings.json` with your database's username and password:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "CoachManagement": "Server=(local);uid=your-username;pwd=your-password;database=CoachManagement;TrustServerCertificate=true"
  }
}
```
Make sure `EStore.csproj` has the following lines:
```
<ItemGroup>
  <None Update="appsettings.json">
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
  </None>
</ItemGroup>
```
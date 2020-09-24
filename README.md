# IPRep 
master

![.NET Core](https://github.com/jbies121/iprep/workflows/.NET%20Core/badge.svg)

![CodeQL](https://github.com/jbies121/IPRep/workflows/CodeQL/badge.svg?branch=master)

APIKeyRing

![.NET Core](https://github.com/jbies121/IPRep/workflows/.NET%20Core/badge.svg?branch=APIKeyRing)

## IP Reputation tool for .NET Core 3.1
This tool was developed with the blue team in mind to quickly determine IP origin and reputation.
Using user supplied API keys, IP addresses and ranges can be checked against reputation services quickly and consumed in a usable way.
Supporting multiple APIs allows users to access fresh and diverse reputation sources, and provides some fault tolerance when services are interrupted.

This project aims to stay lightweight and platform independent.

### Supported APIs
- AbuseIPDB
  - CHECK endpoint

### Usage
From an executable:
```powershell
.\iprep.exe [ip] [info] [-service]
```

From within the project directory:
```powershell 
dotnet run [ip] [info] [-service]
```

### Example
```powershell
.\iprep.exe 8.8.8.8 score -AIPDB
```

### 'service' options currently available
- -AIPDB : AbuseIPDB

### 'info' options currently available
- score
- isPublic
- ipVersion
- isWhitelisted
- countryCode
- usageType
- isp
- domain
- hostnames
- countryName
- totalReports
- numDistinctUsers
- lastReportedAt

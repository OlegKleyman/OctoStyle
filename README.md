# OctoStyle - Automatic pull request reviewer using StyleCop  [![Build Status](http://teamcity.theomegoone.net/app/rest/builds/buildType:(id:OctoStyle_Build)/statusIcon)](http://teamcity.theomegoone.net/viewType.html?buildTypeId=OctoStyle_Build&guest=1)
OctoStyle is a simple .NET console application that automatically reviews GitHub pull requests using StyleCop.

## Usage

### Help
```batch
OctoStyle.Console.exe -l [GitHub login] -p [Password] -d [local path to root Git directory] -o [Repository Owner] -r [Repository Name] -pr [Pull Request Number]
```
### Example
```
OctoStyle.Console.exe -l MyUserName -p MyPassword -d "C:\OctoStyleTest" -o OlegKleyman -r OctoStyleTest -pr 1
```

## Supported Framework
.NET Framework 4.6

## Installation
### MSI Installer
You can grab the latest installer [here](http://teamcity.theomegoone.net/repository/download/OctoStyle_Build/.lastSuccessful/OctoStyle.msi).
### Nuget Package
You can install the latest binaries via nuget package manager
```
PM> Install-Package OctoStyle.Console
```
This nuget package contains the binaries needed to run the console application. Primarily created so installation on the build server would not be required

## Building the Solution
### Requirements
* Visual Studio 2013
* .NET Framework 4.6

## Requirements for Running Tests
### Running Unit Tests
An NUnit test runner is required.
### Running integration tests
* An NUnit test runner is required.
* A github account is required.
* OCTOSTYLE_LOGIN environment variable set to your GitHub username is required.
* OCTOSTYLE_PASSWORD environment variable set to your GitHub password is required.

## Requirements Modifying the Code
### Minimum
* Text Editor
* At least .NET Framework 4.6

### Suggested
* Visual Studio 2013+
* Specflow Templates located [here](http://www.specflow.org/).

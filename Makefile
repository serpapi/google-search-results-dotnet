
# all
all: build run

build:
	dotnet build

run:
	dotnet run

# Dotnet
#
# https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package-using-the-dotnet-cli
#
# Package API
pack:
	dotnet pack

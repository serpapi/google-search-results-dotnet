
.PHONY: test

# all
all: clean build test

clean:
	dotnet clean

build:
	dotnet build

test:
	dotnet test

run:
	dotnet run

# Dotnet
#
# https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package-using-the-dotnet-cli
#
# Package API
publish:
	dotnet pack
#	open serpapi/bin/Debug
#	open -a "Google\ Chrome" https://www.nuget.org/packages/manage/upload

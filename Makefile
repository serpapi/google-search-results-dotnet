
# all
all: build run

clean:
	dotnet clean

build:
	dotnet build

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

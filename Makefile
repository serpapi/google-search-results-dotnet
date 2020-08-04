
.PHONY: test

name=google-search-results-dotnet
root=`pwd`
example=google

# all
all: clean restore build test

clean:
	dotnet clean

restore:
	dotnet restore

build:
	dotnet build --configuration Release

test:
	dotnet test test/ --configuration Release

run:
	dotnet run

pack:
	dotnet pack

oobt: pack
	cd example/${example} ; \
	dotnet add package \
		--package-directory ${root}/${name} ${name} ; \
	dotnet build ; \
	dotnet run

# Dotnet
#
# https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package-using-the-dotnet-cli
#
# Package API
release: oobt
	open serpapi/bin/Debug
	open -a "Google\ Chrome" https://www.nuget.org/packages/manage/upload

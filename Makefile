
.PHONY: test

name=google-search-results-dotnet
root=`pwd`
example=google

# all
all: clean restore build test

clean:
	dotnet clean serpapi/
	dotnet clean test/

restore:
	dotnet restore

build:
	dotnet build --configuration Release

test:
	dotnet test test/ --configuration Release
#	dotnet test test/ --configuration Release --filter TestSpecialCharactersEncoding  --logger "console;verbosity=detailed"

run:
	dotnet run

pack:
	dotnet pack

oobt: pack
	$(MAKE) run_oobt example=google
	$(MAKE) run_oobt example=bing

run_oobt:
	cd example/${example} ; \
	dotnet add package --package-directory ${root}/${name} ${name} ; \
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

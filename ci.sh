#!/bin/bash

PROJECT="$1"
TEST_SUITE="$2"

function test {
    cd ./$TEST_SUITE
    dotnet test
    cd ../
}

function restore {
    dotnet restore 
}

function publish {
    dotnet publish -o Output $PROJECT
}

function copy {
    cp ./Dockerfile ./$PROJECT/Output/Dockerfile
}

function login {
    cd ./$PROJECT/Output
    docker login registry.gitlab.com -u piotrhugonow@gmail.com -p 1qaz@WSX
}

function build {
    docker build --build-arg project=$PROJECT -t registry.gitlab.com/piotrhugonow/gitlabcitest/image .
}

function push {
    docker push registry.gitlab.com/piotrhugonow/gitlabcitest/image
}

echo Restoring packages...
restore
echo Testing...
result=$(test)
echo Publishing...
publish
echo Copying Dockerfile...
copy
echo Logging to Gitlab...
login
echo Building docker image...
build
echo Pushing docker image to Gitlab...
push
echo Done!

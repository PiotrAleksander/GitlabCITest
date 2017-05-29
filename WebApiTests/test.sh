#!/bin/bash

function restore {
	dotnet restore
}
function test {
	dotnet test
}
echo "Executing test suite..."
restore
test
echo "Tests done"

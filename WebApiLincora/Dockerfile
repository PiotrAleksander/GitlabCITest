FROM microsoft/dotnet
ARG project
ENV project=${project}
WORKDIR $project/
COPY . .
ENTRYPOINT dotnet $project.dll


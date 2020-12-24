#! /bin/bash
cur_dir=$(cd "$(dirname "$0")"; pwd)

dotnet publish -r linux-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true

echo =====The End=====
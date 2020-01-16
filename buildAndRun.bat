start /wait /D "C:\SourceR\GatewayAuthentication" build.bat
echo "build done"
start /D "C:\SourceR\GrpcGreeter" build.bat
start /D "C:\SourceR\GrpcGreeter" runGrpcServer.bat
TIMEOUT /T 5
start /D "C:\SourceR\GrpcGreeter" runGrpcClient.bat

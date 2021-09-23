echo 'Building'
dotnet build
echo 'Build completed'
echo 'Launching API Backend'
dotnet watch run --project LightScadaAPI &
echo 'API Launched'
echo 'Launching Front End'
dotnet watch run --project FrontEnd &

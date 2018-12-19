$ErrorActionPreference = "Stop"

#assume you have already logged in and targeted the correct Org/Space

$serviceName = "credhub"
$servicePlan = "default"
$serviceInstanceName = "test-network-share"

$writeUser = "shareWriteUser"
$password = "thisIs1Pass!"

$shareNetworkAddress = "\\\\34.205.85.74\\test_network_share"

$serviceTags = [string]::Format('{0},test-smb-share',$serviceInstanceName) #comma delimited

$credsParamJSON = [string]::Format('{{\"share-username\":\"{0}\",\"share-password\":\"{1}\"}}',$writeUser,$password)
$addressParamJSON = [string]::Format('{{\"share-network-address\":\"{0}\"}}',$shareNetworkAddress)

#Create the service instance
cf create-service $serviceName $servicePlan $serviceInstanceName -c $credsParamJSON -t $serviceTags

#Create a user provided service with the network address of the SMB share
cf create-user-provided-service "network-address" -p $addressParamJSON  -t $serviceTags
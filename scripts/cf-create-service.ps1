#Copyright (C) 2019-Present Pivotal Software, Inc. All rights reserved. 

#This program and the accompanying materials are made available under the terms of the under 
#the Apache License, Version 2.0 (the "License”); you may not use this file except in compliance 
#with the License. You may obtain a copy of the License at

#http://www.apache.org/licenses/LICENSE-2.0

#Unless required by applicable law or agreed to in writing, software distributed under the 
#License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
#either express or implied. See the License for the specific language governing permissions 
#and limitations under the License.

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
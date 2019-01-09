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

$writeUser = "shareWriteUser"
$folderPath = "c:\test_network_share"
$shareName = "test_network_share"
$password = ConvertTo-SecureString "thisIs1Pass!" -AsPlainText -Force

#Create local user accounts
New-LocalUser $writeUser `
-Password $password `
-FullName "NetworkFull Write" `
-Description "A write account to test network shared"

echo "-----> Created user accounts"

#Add accounts to users group
Add-LocalGroupMember -Group "Users" -Member $writeUser
echo "-----> Added to group"

#Create folders to share
New-Item -ItemType directory -Path $folderPath 
echo "-----> Created folder"

#Share the folders, Users group gets read access
New-SmbShare -Name $shareName `
  -Path $folderPath `
  -ReadAccess "Everyone", "Guests" `
  -FullAccess $writeUser

#echo "-----> Share address: \\$Env:COMPUTERNAME\$shareName"
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

Write-Host "-----> Created user accounts"

#Add accounts to users group
Add-LocalGroupMember -Group "Users" -Member $writeUser
Write-Host "-----> Added to group"

#Create folders to share
New-Item -ItemType directory -Path $folderPath 
Write-Host "-----> Created folder"

#Share the folders, Users group gets read access
New-SmbShare -Name $shareName `
  -Path $folderPath `
  -ReadAccess "Everyone", "Guests" `
  -FullAccess $writeUser

Write-Host "-----> Share address: \\$Env:COMPUTERNAME\$shareName"
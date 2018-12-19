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
# dotnet-smb-network-shares
An example Steeltoe solution for consuming SMB shared network resources in a .NET application.

The solution has 2 projects within...

### NET-core2_1-win-network-smb-share
This is a .NET application written in .NET core, targeting the .NET framework, running on Pivotal Application Services for Windows. If you look in the NET-core2_1-win-network-smb-share.csproj file, you see how this combination is achieved.

Also have a look at the manifest.yml to see how the app is deployed on PAS.

### NET-framework4_6_1-win-network-smb-share
This is a .NET application written in .NET framework, running on Pivotal Application Services for Window. Similar to the other project, the .csproj and manifest will tell the story.

### Scripts
There are the powershell scripts to set up an example network share and to create the appropriate bindable services on PAS.

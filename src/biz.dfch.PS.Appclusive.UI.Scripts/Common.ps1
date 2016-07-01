#
# Common.ps1
#

$AdminUIContants = @{
	"NodeName" = "AdminUIConfiguration";

	"NodeParameters" = "{}";

	"EntityKinds" = @{
		"AdminUIConfiguration" = "biz.dfch.CS.Appclusive.UI.Configuration";

		"Configuration" = "biz.dfch.CS.Appclusive.Core.OdataServices.Core.Configuration";
		
		"Node" = "biz.dfch.CS.Appclusive.Core.OdataServices.Core.Node";
	};

	"EntityKind" = @{
		"Name" = "biz.dfch.CS.Appclusive.UI.Configuration";

		"Description" = "Admin UI Configuration to grant permissions to show actions in the UI";

		"Parameters" = "{'InitialState-Initialise':'Created'}";
	};

	"SystemTenantId" = "11111111-1111-1111-1111-111111111111";

	"SystemPermissionPrefix" = "Apc:";

	"CanReadSuffix" = "CanRead";

	"NodeAclName" = "Admin UI Permissions";

	"CloudAdminRoleName" = "CloudAdmin";
};

function GetApcServer($ServerBaseUri, $BaseUrl, $Credential)
{
	if ($ServerBaseUri -ne $null -and $BaseUrl -ne $null -and $Credential -ne $null)
	{
		return Enter-ApcServer -ServerBaseUri $ServerBaseUri -BaseUrl $BaseUrl -Credential $Credential;
	}
	
	if ($ServerBaseUri -ne $null -and $BaseUrl -ne $null)
	{
		return Enter-ApcServer -ServerBaseUri $ServerBaseUri -BaseUrl $BaseUrl;
	}
	
	if ($ServerBaseUri -ne $null)
	{
		return Enter-ApcServer -ServerBaseUri $ServerBaseUri;
	}
	
	if ($BaseUrl -ne $null -and $Credential -ne $null)
	{
		return Enter-ApcServer -BaseUrl $BaseUrl -Credential $Credential;
	}
	
	if ($BaseUrl -ne $null)
	{
		return Enter-ApcServer -BaseUrl $BaseUrl;
	}
	
	if ($Credential -ne $null)
	{
		return Enter-ApcServer-Credential $Credential;
	}

	return Enter-ApcServer;
}

function IsNullOrEmpty($param)
{
	return [string]::IsNullOrEmpty($param);
}
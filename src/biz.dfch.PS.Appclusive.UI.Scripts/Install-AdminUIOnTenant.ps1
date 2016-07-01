#Requires -Modules biz.dfch.PS.Appclusive.Client, biz.dfch.PS.System.Logging
#
# Script.ps1
#
PARAM(
	# [Optional] The ServerBaseUri such as 'https://appclusive/'. If you do not 
	# specify this value it is taken from the module configuration file.
	[Parameter(Mandatory = $false, Position = 0)]
	[Uri] $ServerBaseUri
	, 
	# [Optional] The BaseUrl such as '/Appclusive/'. If you do not specify this 
	# value it is taken from the module configuration file.
	[Parameter(Mandatory = $false, Position = 1)]
	[string] $BaseUrl
	, 
	# Encrypted credentials as [System.Management.Automation.PSCredential] with 
	# which to perform login. Default is credential as specified in the module 
	# configuration file.
	[Parameter(Mandatory = $false, Position = 2)]
	$Credential
	,
	# Guid of the Tenant for whom the AdminUI should be configured.
	[Parameter(Mandatory = $false, Position = 3)]
	$TenantId
	,
	# Guid of the Tenant for whom the AdminUI should be configured.
	[Parameter(Mandatory = $false, Position = 3)]
	$TenantName
)

BEGIN
{
	$dateTimeBegin = [datetime]::Now;
	$OutputParameter = $null;
	$fn = $MyInvocation.MyCommand.Name;
	
	Import-Module "biz.dfch.PS.System.Logging"

	Set-Variable 'biz_dfch_PS_System_Logging_Logger' 'root' -ErrorAction:SilentlyContinue;
	Log-Debug $fn ("CALL. ServerBaseUri '{0}'; BaseUrl '{1}'. Username '{2}'" -f $ServerBaseUri, $BaseUrl, $Credential.Username ) -fac 1;

	. .\Common.ps1
}

PROCESS
{
	trap 
	{ 
		$ex = $_;
		Log-Debug $fn $ex.Exception.Message;
		$PSCmdlet.ThrowTerminatingError($ex);	
	}

	if ($TenantId -and $TenantName)
	{
		Write-Error "TenantId or TenantName needed.";
		return;
	}

	$appclusive = GetApcServer($ServerBaseUri, $BaseUrl, $Credential);

	if ($TenantId)
	{
		$TenantFilter = "Id eq guid'{0}'" -f $TenantId;
		$Tenant = $appclusive.Core.Tenants.AddQueryOption('$filter', $TenantFilter);

		if (!$Tenant)
		{
			$message = ("Tenant with Id:{0} does not exist" -f $TenantId);
			Log-Error $fn $message;
			Write-Error $message;
			return;
		}
	}

	if ($TenantName)
	{
		$TenantFilter = "Name eq '{0}'" -f $TenantName;
		$Tenant = $appclusive.Core.Tenants.AddQueryOption('$filter', $TenantFilter);
		
		if (!$Tenant)
		{
			$message = ("Tenant with Name:{0} does not exist" -f $TenantName);
			Log-Error $fn $message;
			Write-Error $message;
			return;
		}

		if ($TenantId)
		{
			if ($TenantId -ne $Tenant.Id.ToString())
			{
				$message = ("TenantId does not match with Tenant with Name (Name:{0},TenantId:{1},Provided TenantId:{2})." -f $TenantName,$Tenant.Id,$TenantId);
				Log-Error $fn $message;
				Write-Error $message;
				return;
			}
		}

		$TenantId = $Tenant.Id
		Write-Host "$TenantName $TenantId"
	}

	$AdminUIConfigurationEntityKindFilter = "Version eq '{0}'" -f $AdminUIContants.EntityKinds.AdminUIConfiguration;
	$AdminUIConfigurationEntityKind = $appclusive.Core.EntityKinds.AddQueryOption('$filter', $AdminUIConfigurationEntityKindFilter);
												 
	if (!$AdminUIConfigurationEntityKind)
	{
		$message = ("EntityKind:'{0}' does not exist. Aborting, need to run 'Install-AdminUI' first." -f $AdminUIContants.EntityKinds.AdminUIConfiguration);
		Log-Error $fn $message;
		Write-Error $message;
		return;
	}

	$AdminUIConfigurationNodeFilter = "Tid eq guid'{0}' and EntityKindId eq {1}" -f $TenantId,$AdminUIConfigurationEntityKind.Id;
	$AdminUIConfigurationNode = $appclusive.Core.Nodes.AddQueryOption('$filter', $AdminUIConfigurationNodeFilter);

	if (!$AdminUIConfigurationNode)
	{
		$message = "AdminUIConfigurationNode does not exist for TenantId:{0}, start Creation." -f $TenantId;
		Log-Info $fn $message;
		Write-Information $message;

		$ConfigurationEntityKindFilter = "Version eq '{0}'" -f $AdminUIContants.EntityKinds.Configuration;
		$ConfigurationEntityKind = $appclusive.Core.EntityKinds.AddQueryOption('$filter', $ConfigurationEntityKindFilter);
		
		if (!$ConfigurationEntityKind)
		{
			$message = ("EntityKind:'{0}' does not exist. Aborting." -f $AdminUIContants.EntityKinds.AdminUIConfiguration);
			Log-Error $fn $message;
			Write-Error $message;
			return;
		}

		$ConfigurationNodeFilter = "Tid eq guid'{0}' and EntityKindId eq {1}" -f $TenantId,$ConfigurationEntityKind.Id;
		$ConfigurationNode = $appclusive.Core.Nodes.AddQueryOption('$filter', $ConfigurationNodeFilter);

		if (!$ConfigurationNode)
		{
			$message = "Configuration Node does not exist. Aborting.";
			Log-Error $fn $message;
			Write-Error $message;
			return;
		}

		$AdminUIConfigurationNode = New-Object biz.dfch.CS.Appclusive.Api.Core.Node;
		$AdminUIConfigurationNode.EntityKindId = $AdminUIConfigurationEntityKind.Id;
		$AdminUIConfigurationNode.ParentId = $ConfigurationNode.Id;
		$AdminUIConfigurationNode.Name = $AdminUIContants.NodeName;
		$AdminUIConfigurationNode.Description = $AdminUIConfigurationEntityKind.Description;
		$AdminUIConfigurationNode.Tid = $TenantId;
		$AdminUIConfigurationNode.Parameters = $AdminUIContants.NodeParameters;

		$appclusive.Core.AddToNodes($AdminUIConfigurationNode);
		$appclusive.Core.SaveChanges();
		
		$message = ("Admin UI Configuration Node with Id:{0} created." -f $AdminUIConfigurationNode.Id);
		Log-Info $fn $message;
		Write-Information $message;
	}

	$AclFilter = "EntityId eq {0}" -f $AdminUIConfigurationNode.Id;
	$Acl = $appclusive.Core.Acls.AddQueryOption('$filter', $AclFilter);

	if (!$Acl)
	{
		$CloudAdminRoleFilter = "Tid eq guid'{0}' and Name eq '{1}' and RoleType eq 'BuiltIn'" -f $TenantId,$AdminUIContants.CloudAdminRoleName;
		$CloudAdminRole = $appclusive.Core.Roles.AddQueryOption('$filter', $CloudAdminRoleFilter);

		if (!$CloudAdminRole)
		{
			$message = ("Role(Name:'{0}',TenantId:'{1}') does not exist. Aborting." -f $AdminUIContants.CloudAdminRoleName,$TenantId);
			Log-Error $fn $message;
			Write-Error $message;
			return;
		}

		$NodeEntityKindFilter = "Version eq '{0}'" -f $AdminUIContants.EntityKinds.Node;
		$NodeEntityKind = $appclusive.Core.EntityKinds.AddQueryOption('$filter', $NodeEntityKindFilter);

		$Acl = New-Object biz.dfch.CS.Appclusive.Api.Core.Acl;
		$Acl.NoInheritanceFromParent = $true;
		$Acl.EntityId = $AdminUIConfigurationNode.Id;
		$Acl.EntityKindId = $NodeEntityKind.Id;
		$Acl.Tid = $TenantId;
		$Acl.Name = $AdminUIContants.NodeAclName;

		$appclusive.Core.AddToAcls($Acl);
		$null = $appclusive.Core.SaveChanges();
		
		$message = "Acl {0} created for TenantId:{1}, start Creation." -f $Acl.Name,$TenantId;
		Log-Info $fn $message;
		Write-Information $message;

		$Ace = New-Object biz.dfch.CS.Appclusive.Api.Core.Ace;
		$Ace.AclId = $Acl.Id;
		$Ace.Tid = $TenantId;
		$Ace.Type = 2; # Allow
		$Ace.TrusteeType = 0; # Role
		$Ace.TrusteeId = $CloudAdminRole.Id;
		$Ace.PermissionId = 0; # All
		$Ace.Name = "{0} {1}" -f $AdminUIContants.NodeAclName,$CloudAdminRole.Name;

		$appclusive.Core.AddToAces($Ace);
		$null = $appclusive.Core.SaveChanges();

		$message = "Ace {0} created for TenantId:{1}, start Creation." -f $Ace.Name,$TenantId;
		Log-Info $fn $message;
		Write-Information $message;
	}

	Write-Host "SUCCEEDED" -ForegroundColor Green;
}

END
{
	$dateTimeEnd = [datetime]::Now;
	Log-Debug -fn $fn -msg ("RET. fReturn: [{0}]. Execution time: [{1}]ms. Started: [{2}]." -f $fReturn, ($dateTimeEnd - $dateTimeBegin).TotalMilliseconds, $dateTimeBegin.ToString('yyyy-MM-dd HH:mm:ss.fffzzz')) -fac 2;
}
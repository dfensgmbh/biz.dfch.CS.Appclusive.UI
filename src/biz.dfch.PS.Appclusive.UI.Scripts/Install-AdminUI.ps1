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
)

BEGIN
{
	$dateTimeBegin = [datetime]::Now;
	$OutputParameter = $null;
	$fn = $MyInvocation.MyCommand.Name;

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

	$ServerBaseUri;

	$appclusive = GetApcServer($ServerBaseUri, $BaseUrl, $Credential);
	
	$filter = "Version eq '{0}'" -f $AdminUIContants.EntityKinds.AdminUI;
	$entityKind = $appclusive.Core.EntityKinds.AddQueryOption('$filter', $filter);

	if ($entityKind -ne $null)
	{
		Log-Info $fn ("EntityKind:'{0}' already installed." -f $AdminUIContants.EntityKinds.AdminUI);
		return;
	}

	Log-Info $fn ("EntityKind:'{0}' not installed yet, creating new EntityKind." -f $AdminUIContants.EntityKinds.AdminUI);

	$newEntityKind = New-Object biz.dfch.CS.Appclusive.Api.Core.EntityKind
	$newEntityKind.Version = $AdminUIContants.EntityKinds.AdminUIConfiguration;
	$newEntityKind.Name = $AdminUIContants.EntityKind.Name;
	$newEntityKind.Description = $AdminUIContants.EntityKind.Description;
	$newEntityKind.Parameters = $AdminUIContants.EntityKind.Parameters;

	$appclusive.Core.AddToEntityKinds($newEntityKind);
	$null = $appclusive.Core.SaveChanges();

	Log-Info $fn ("EntityKind:'{0}' with Id:'{1}' created." -f $AdminUIContants.EntityKinds.AdminUI,$newEntityKind.Id);
	Write-Host "SUCCEEDED" -ForegroundColor Green;
}

END
{
	$dateTimeEnd = [datetime]::Now;
	Log-Debug -fn $fn -msg ("RET. fReturn: [{0}]. Execution time: [{1}]ms. Started: [{2}]." -f $fReturn, ($dateTimeEnd - $dateTimeBegin).TotalMilliseconds, $dateTimeBegin.ToString('yyyy-MM-dd HH:mm:ss.fffzzz')) -fac 2;
}
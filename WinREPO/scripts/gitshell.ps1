<#
.SYNOPSIS
    Sets up the Git Shell Environment
.DESCRIPTION
    Sets up the proper PATH and ENV towards git for Window's shell environment
#>
[CmdletBinding()]
param([string]$gitPath = "")

#Write-Host "Git Path passed is : $gitPath"

if ($env:gitshell -eq $null) {

  Write-Verbose "Running gitshell.ps1 to include git paths for use... "

  Push-Location (Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

  $env:gitshell = "$gitPath"
  $env:PLINK_PROTOCOL = "ssh"
  $env:TERM = "msys"
  $env:HOME = resolve-path (join-path ([environment]::getfolderpath("mydocuments")) "..\")
  $env:TMP = $env:TEMP = [system.io.path]::gettemppath()
  if ($env:EDITOR -eq $null) {
    $env:EDITOR = "GitPad"
  }

  # Setup PATH
  $pGitPath = $env:gitshell
  $msBuildPath = "$env:SystemRoot\Microsoft.NET\Framework\v4.0.30319"

  $env:Path = "$env:Path;$pGitPath\cmd;$pGitPath\bin;$msbuildPath"

  Pop-Location

} else { Write-Verbose "Git shell environment already setup" }

[CmdletBinding()]
param([string]$configPath = "")

Add-Content "$configPath" "UserKnownHostsFile=/dev/null" -Encoding ASCII
Add-Content "$configPath" "Host *" -Encoding ASCII
Add-Content "$configPath" "    StrictHostKeyChecking no" -Encoding ASCII


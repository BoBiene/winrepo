 [CmdletBinding()]
param([string]$configPath = "")

echo "`r`nUserKnownHostsFile=/dev/null" >> "$configPath"
echo "Host *" >> "$configPath"
echo "    StrictHostKeyChecking no" >> "$configPath"
Import-Module .\Build\psake
Invoke-psake -taskList Publish
if(-not $psake.build_success){
  exit(-1)
}

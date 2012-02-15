properties { 
  $base_dir  = resolve-path .
  $lib_dir = "$base_dir\SharedLibs"
  $build_dir = "$base_dir\build" 
  
  $sln_file = "$base_dir\Mammock.sln" 
  $version = "1.0.0.0"
  $humanReadableversion = "1.0"
  $tools_dir = "$base_dir\Tools"
  $release_dir = "$base_dir\Release"
  $uploadCategory = "Rhino-Mocks"
  $uploader = "..\Uploader\S3Uploader.exe"
  $copyright = "Hibernating Rhinos & Ayende Rahien 2004 - 2009, Bjørn Bouet Smith 2012"
  $company = "Bjørn Bouet Smith"
} 

include .\psake_ext.ps1
	
task default -depends Release

task Clean { 
  remove-item -force -recurse $build_dir -ErrorAction SilentlyContinue 
  remove-item -force -recurse $release_dir -ErrorAction SilentlyContinue 
} 

task Init -depends Clean { 
	
	Generate-Assembly-Info `
		-file "$base_dir\Mammock\Properties\AssemblyInfo.cs" `
		-title "Mammock $version" `
		-description "Mocking Framework for .NET" `
		-company $company `
		-product "Mammock $version" `
		-version $version `
		-copyright $copyright `
		-internalsVisibleTo "Mammock.Tests"
		
	Generate-Assembly-Info `
		-file "$base_dir\Mammock.Tests\Properties\AssemblyInfo.cs" `
		-title "Mammock Tests $version" `
		-description "Mocking Framework for .NET" `
		-company $company `
		-product "Mammock Tests $version" `
		-version $version `
		-clsCompliant "false" `
		-copyright $copyright `
		-internalsVisibleTo "Mammock.Tests"
		
	Generate-Assembly-Info `
		-file "$base_dir\Mammock.Tests.Model\Properties\AssemblyInfo.cs" `
		-title "Mammock Tests Model $version" `
		-description "Mocking Framework for .NET" `
		-company $company `
		-product "Mammock Tests Model $version" `
		-version $version `
		-clsCompliant "false" `
		-copyright $copyright `
		-internalsVisibleTo "Mammock.Tests"
	
	Generate-Assembly-Info `
		-file "$base_dir\Mammock.GettingStarted\Properties\AssemblyInfo.cs" `
		-title "Mammock Tests $version" `
		-description "Mocking Framework for .NET" `
		-company $company `
		-product "Mammock Tests $version" `
		-version $version `
		-clsCompliant "false" `
		-copyright $copyright `
		-internalsVisibleTo "Mammock.Tests"
		
	new-item $release_dir -itemType directory 
	new-item $build_dir -itemType directory 
	cp $tools_dir\xUnit\*.* $build_dir
} 

task Compile -depends Init { 
  & msbuild "$sln_file" "/p:OutDir=$build_dir\\" /p:Configuration=Release
  if ($lastExitCode -ne 0) {
        throw "Error: Failed to execute msbuild"
  }
} 

task Test -depends Compile {
  $old = pwd
  cd $build_dir
  &.\Xunit.console.clr4.x86.exe "$build_dir\Mammock.Tests.dll" /-Trait "Category=NotWorking"
  if ($lastExitCode -ne 0) {
        throw "Error: Failed to execute tests"
  }
  cd $old		
}

task Merge {
	$old = pwd
	cd $build_dir
	
	Remove-Item Rhino.Mocks.Partial.dll -ErrorAction SilentlyContinue 
	Rename-Item $build_dir\Rhino.Mocks.dll Rhino.Mocks.Partial.dll
	
	& $tools_dir\ILMerge.exe Rhino.Mocks.Partial.dll `
		Castle.DynamicProxy2.dll `
		Castle.Core.dll `
		/out:Rhino.Mocks.dll `
		/t:library `
		"/keyfile:$base_dir\ayende-open-source.snk" `
		"/internalize:$base_dir\ilmerge.exclude"
	if ($lastExitCode -ne 0) {
        throw "Error: Failed to merge assemblies!"
    }
	cd $old
}

task Release -depends Test {
	$commit = Get-Git-Commit
	& $tools_dir\zip.exe -9 -A -j `
		$release_dir\Mammock-$humanReadableversion-Build-$commit.zip `
		$build_dir\Mammock.dll `
		$build_dir\Mammock.xml `
		license.txt `
		acknowledgements.txt
	if ($lastExitCode -ne 0) {
        throw "Error: Failed to execute ZIP command"
    }
}


task Upload -depends Release {
	$commit = Get-Git-Commit
	Write-Host "Starting upload"
	if (Test-Path $uploader) {
		$log = $env:push_msg 
    if($log -eq $null -or $log.Length -eq 0) {
      $log = git log -n 1 --oneline		
    }
		&$uploader "$uploadCategory" "$release_dir\Rhino.Mocks-$humanReadableversion-Build-$commit" "$log"
		
		if ($lastExitCode -ne 0) {
      write-host "Failed to upload to S3: $lastExitCode"
			throw "Error: Failed to publish build"
		}
	}
	else {
		Write-Host "could not find upload script $uploadScript, skipping upload"
	}
}
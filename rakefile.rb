require 'albacore'
require 'fileutils'

@build_folder = Rake.application.original_dir + "/build"
@output = @build_folder + '/bin'
@nuget_dir = @build_folder + "/nuget"
@description = "Lightweight near zero-configuration library for easy conventional mapping for nhibernate."
@project = "Enhima"

task :default => [:nuspec]

msbuild :clean do |clean|
	clean.targets :Clean
	clean.solution = "Auxiliary.csproj"
	clean.verbosity = "quiet"
	clean.parameters = "/nologo"
end

desc "Generate solution version "
assemblyinfo do |asm|
	output = `git describe --abbrev=64`
	output =~ /-(\d+)-(.*)/
	revision = $1 || 0
	hash = $2
	
	@version = "0.9.#{revision}"
	@product_version = "#{@version}.#{hash}" 
	@package_version = "#{@version}-beta"
	
	asm.version = @version
	asm.file_version = @version
	asm.custom_attributes :AssemblyInformationalVersionAttribute => @product_version
	asm.output_file = "SolutionVersion.cs"
	asm.description = @description
end

desc "Build the application with msbuild"
msbuild :msbuild => [:clean, :assemblyinfo] do |msb|
	msb.properties :configuration => :Debug, :OutputPath => @output
	msb.targets :Clean, :Build
	msb.solution = "#{@project}.sln"
	msb.verbosity = "quiet"
	msb.parameters = "/nologo"
end

desc "Run fixtures"
nunit :nunit => [:msbuild] do |nunit|
	nunit.command = "packages/NUnit.2.5.10.11092/tools/nunit-console.exe" #Think how to automatically search for nunit-console
	nunit.assemblies "#{@output}/#{@project}.Tests.dll" #Automatically look up assemblies using mask *.Tests.dll
	nunit.options "/nologo"
end

desc "Build the application with msbuild"
msbuild :prepare_nuget => [:nunit] do |msb|
  msb.targets :PrepareForNuget
  msb.solution = "Auxiliary.csproj"
  msb.verbosity = "quiet"
  msb.parameters = "/nologo"
end

desc "Creating nuspec file"
nuspec :nuspec => [:prepare_nuget] do |nuspec|
   @nuspec_name = "#{@project}.nuspec"
   nuspec.id= "Enhima"
   nuspec.version = @package_version
   nuspec.authors = "Jury Soldatenkov"
   nuspec.description = @description
   nuspec.title = "NHibernate mapping by code using predefined conventions"
   nuspec.output_file = "#{@nuget_dir}/#{@nuspec_name}"
   nuspec.projectUrl = "http://github.com/solyutor/enhima"
   nuspec.dependency "NHibernate", "3.2.0"
   nuspec.tags NHibernate, MappingByCode
end

desc "create the nuget package"
nugetpack :pack => [:nuspec] do |nuget|
   nuget.command	= ".nuget/nuget.exe"
   nuget.nuspec		= "#{@nuget_dir}/#{@nuspec_name}"
   nuget.output		= @build_folder
 end

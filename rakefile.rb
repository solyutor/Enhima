require 'albacore'
require 'fileutils'

@project = "Enhima"
@description = "Lightweight near zero-configuration library for easy conventional mapping for nhibernate."

@build_folder = Rake.application.original_dir + "/build"
@output = @build_folder + '/bin'

@nuget_dir = @build_folder + "/nuget"
@nuspec_entities = "#{@project}.Entities"
@nuspec_mapping = "#{@project}.Mapping"

task :default => [:pack_mapping]

msbuild :clean do |clean|
	clean.targets :Clean
	clean.solution = "Auxiliary.csproj"
	clean.verbosity = "quiet"
	clean.parameters = "/nologo"
end

desc "Generate solution version "
assemblyinfo do |asm|
#Assuming we have tag '0.9-beta' git describe --abbrev=64 returns 0.9-beta-18-g408122de9c62e64937f5c1956a27cf4af9648c12
#If we have tag '0.9' git describe --abbrev=64 returns 0.9-18-g408122de9c62e64937f5c1956a27cf4af9648c12
#It should be parsed by  to generate correct assembly and nuget package version

	output = `git describe --abbrev=64`
	version_parts = output.match /(\d+).(\d+)-?([a-zA-Z]*)-(\d+)-(\w{7})/
	major = version_parts[1]
	minor = version_parts[2]
	revision = version_parts[4] || 0
	version_type = version_parts[3]
	hash = version_parts[5]
	
	@version = "#{major}.#{minor}.#{revision}"
	@package_version = @version
	@package_version += "-#{version_type}"  if (version_type || "").length > 0
	
	@product_version = @version
	@product_version += ("-#{version_type}"  if (version_type || "").length > 0) + "-#{hash}"
	
	asm.version = @version
	asm.file_version = @version
	asm.custom_attributes :AssemblyInformationalVersionAttribute => @product_version
	asm.output_file = "SolutionVersion.cs"
	asm.description = @description
end

desc "Build the application with msbuild"
msbuild :msbuild => [:clean, :assemblyinfo] do |msb|
	msb.properties :configuration => :Release, :OutputPath => @output
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

desc "Creating nuspec file for entities"
nuspec :nuspec_entities => [:prepare_nuget] do |nuspec|
	nuspec.id= @nuspec_entities
	nuspec.version = @package_version
	nuspec.authors = "Jury Soldatenkov"
	nuspec.description = @description
	nuspec.output_file = "#{@nuget_dir}/#{@nuspec_entities}/#{@nuspec_entities}.nuspec"
	nuspec.projectUrl = "http://github.com/solyutor/enhima"
	nuspec.tags = 'NHibernate MappingByCode MappingByConventions'
end

desc "Creating nuspec file for mappings"
nuspec :nuspec_mapping => [:prepare_nuget] do |nuspec|
	nuspec.id = @nuspec_mapping
	nuspec.version = @package_version
	nuspec.authors = "Jury Soldatenkov"
	nuspec.description = @description
	nuspec.output_file = "#{@nuget_dir}/#{@nuspec_mapping}/#{@nuspec_mapping}.nuspec"
	nuspec.projectUrl = "http://github.com/solyutor/enhima"
	nuspec.dependency @nuspec_entities, @package_version
	nuspec.dependency "NHibernate", "3.2.0"
	nuspec.tags = 'NHibernate MappingByCode MappingByConventions'
end


desc "create the nuget package"
nugetpack :pack_entities => [:nuspec_entities] do |nuget|
	nuget.command = ".nuget/nuget.exe"
	nuget.nuspec = "#{@nuget_dir}/#{@nuspec_entities}/#{@nuspec_entities}.nuspec"
	nuget.output = @build_folder
end

desc "create the nuget package"
nugetpack :pack_mapping => [:pack_entities, :nuspec_mapping] do |nuget|
	nuget.command = ".nuget/nuget.exe"
	nuget.nuspec = "#{@nuget_dir}/#{@nuspec_mapping}/#{@nuspec_mapping}.nuspec"
	nuget.output = @build_folder
end
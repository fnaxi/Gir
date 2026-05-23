-- CopyRight https://github.com/fnaxi. All Rights Reserved.


require ('vstudio')

premake.api.register
{
	name = "GlobalItems",
	scope = "workspace",
	kind = "list:string",
}
premake.override(premake.vstudio.sln2005, "projects", function(base, wks)
	if wks.GlobalItems and #wks.GlobalItems > 0 then
		local solution_folder_GUID = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}"
		premake.push("Project(\"" .. solution_folder_GUID .. "\") = \"GlobalItems\", \"GlobalItems\", \"{" .. os.uuid("GlobalItems:" .. wks.name) .. "}\"")
		premake.push("ProjectSection(SolutionItems) = preProject")
		for _, path in ipairs(wks.GlobalItems) do
			premake.w(path .. " = " .. path)
		end
		premake.pop("EndProjectSection")
		premake.pop("EndProject")
	end
	base(wks)
end)

workspace "InvaderZim"
location "../"

GlobalItems
{
	"README.md",
	".gitignore",
	"InvaderZim/InvaderZim.lua"
}

architecture "x64"

configurations
{
	"Debug"
}
platforms
{
	"x64"
}

project "InvaderZim"
location "InvaderZim"
kind "ConsoleApp"
language "C#"
csversion "11"
dotnetframework "4.8"

targetdir ("Binaries")
objdir ("Intermediate")

files
{
	"InvaderZim/**.cs",
}

links
{
	"System"
}

filter "configurations:Debug"
runtime "Debug"
symbols "on"

filter "system:windows"
systemversion "latest"

filter {}

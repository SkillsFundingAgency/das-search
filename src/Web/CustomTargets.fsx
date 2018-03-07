open Fake

let testDirectory = getBuildParamOrDefault "buildMode" "Debug"
let packageVersion = environVarOrDefault "BUILD_BUILDNUMBER" "1.0.0.0"

Target "Update WiN Assembly Info Version Numbers"(fun _ ->

    if testDirectory.ToLower() = "release" then
        trace "Update Assembly Info Version Numbers"
        BulkReplaceAssemblyInfoVersions(currentDirectory) (fun p ->
                {p with
                    AssemblyFileVersion = packageVersion
                    AssemblyVersion = packageVersion
                    })
)

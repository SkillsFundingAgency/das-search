open Fake

Target "Dotnet Restore" (fun _ ->
    DotNetCli.Restore(fun p ->
        { p with
                Project = ".\\Sfa.Das.Sas.Shared.Components" })
    DotNetCli.Restore(fun p ->
        { p with
                Project = ".\\Sfa.Das.Sas.Shared.Components.Web" })
    DotNetCli.Restore(fun p ->
        { p with
                Project = ".\\Sfa.Das.Sas.Shared.Components.UnitTests" })
)
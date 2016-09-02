#r @"tools/FAKE/tools/FakeLib.dll"
#r @"tools/FAKE/tools/Fake.IIS.dll"
#r @"tools/FAKE/tools/Microsoft.Web.Administration.dll"

open Fake
open Fake.IISHelper

let solutionFile  = "MetadataTool.Webtests.sln"
let exe = "bin/Debug"

Target "Build solution" (fun _ -> 
  trace <| sprintf "Building  %s" solutionFile
  !! solutionFile
  |> MSBuildDebug "" "Rebuild"
  |> ignore
)

Target "Test" (fun _ ->
  trace "Testing stuff..."
)

let run args =
  let result =
    ExecProcess (fun info ->
                 info.FileName <- (exe @@ "MetadataTool.Webtests.exe")
                 info.Arguments <- args
                 ) (System.TimeSpan.FromMinutes 5.)

  if result <> 0 then failwith "Failed result from unit tests"

Target "Run UI tests" (fun _ ->
  run """--browser Chrome --tag All """ 
)

"Build solution"
  ==> "Run UI tests"
  ==> "Test"

Run "Test"
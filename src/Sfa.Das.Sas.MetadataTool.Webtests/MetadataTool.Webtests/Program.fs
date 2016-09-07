open canopy
open runner
//open reporters
open System
open Common

[<EntryPoint>]
let main argv =
  let args = Args.parse argv
  let testUrl = 
    match args.Environment with 
    | Local -> "http://localhost:19932/"
    | CI -> "http://das-metadatawebapici.cloudapp.net/"

  canopy.configuration.phantomJSDir <- @".\"
  start args.Browser

  //reporter <- new LiveHtmlReporter(Chrome, configuration.chromeDir) :> IReporter

  Frameworks.DisplayFrameworks testUrl

  "first test of meta data web sites" &&& fun _ -> 
    url testUrl
    ".jumbotron h1" == "Meta data tool"

  run()

  match args.Environment with
  | Local -> 
    printfn "press [enter] to extit"
    Console.ReadLine() |> ignore
  | CI -> printfn "-- CI --"

  quit()

  canopy.runner.failedCount
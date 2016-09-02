open canopy
open runner
open reporters
open System
//open Common

[<EntryPoint>]
let main argv =
  //let args = Args.parse argv
  let testUrl = "http://das-metadatawebapici.cloudapp.net/"

  let drivers = sprintf "%s\%s" __SOURCE_DIRECTORY__ @"Tools\drivers\phantomjs\bin"
  canopy.configuration.phantomJSDir <- drivers
  start phantomJS

  printfn  "Drivers: %s" drivers


  "first test of meta data web sites" &&& fun _ -> 
    url testUrl
    ".jumbotron h1" == "Meta data tool"

  "a test that should fail 2" &&& (fun _ ->
    url testUrl
    ".jumbotron h1" == "Meta data tool fail"
  )

  run()

//  printfn "press [enter] to extit"
//  Console.ReadLine() |> ignore

  quit()

  canopy.runner.failedCount
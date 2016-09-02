open canopy
open runner
open reporters
open System
//open Common

[<EntryPoint>]
let main argv =
  //let args = Args.parse argv
  let testUrl = "http://das-metadatawebapici.cloudapp.net/"

  canopy.configuration.chromeDir <- @"Tools\drivers\"
  start chrome

  "a test that should fail" &&& (fun _ ->
    url testUrl
    ".jumbotron h1" == "Meta data tool fail"
  )

  "first test of meta data web sites" &&& fun _ -> 
    url testUrl
    ".jumbotron h1" == "Meta data tool"

  "a test that should fail" &&& (fun _ ->
    url testUrl
    ".jumbotron h1" == "Meta data tool fail"
  )

  run()

//  printfn "press [enter] to extit"
//  Console.ReadLine() |> ignore

  quit()

  canopy.runner.failedCount
module Args

open Argu
open Common

type private CLIArguments = 
  | Browser of string
  | TestType of string
  | Environment of string
  with 
    interface IArgParserTemplate with
      member s.Usage =
        match s with 
        | Browser _ ->  "specify browser, chrome or phantomjs"
        | TestType _ -> "Noting..."
        | Environment _ -> "Local or CI"

let parse cliargs =
  let parser = ArgumentParser.Create<CLIArguments>()
  let results = parser.Parse(cliargs)

  {
    Browser = defaultArg (results.TryPostProcessResult (<@ Browser @>, fromString<canopy.types.BrowserStartMode>)) canopy.types.BrowserStartMode.Chrome
    TestType = defaultArg (results.TryPostProcessResult (<@ TestType @>, fromString<TestType>)) TestType.All
    Environment = defaultArg (results.TryPostProcessResult (<@ Environment @>, fromString<Environment>)) Environment.Local
  }
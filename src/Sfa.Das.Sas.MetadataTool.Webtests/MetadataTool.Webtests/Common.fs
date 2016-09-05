module Common

open Microsoft.FSharp.Reflection

let fromString<'a> s =
  match FSharpType.GetUnionCases typeof<'a> |> Array.filter (fun case -> case.Name.ToLower() = s.ToString().ToLower() ) with
  | [|case|] -> FSharpValue.MakeUnion(case, [||]) :?> 'a
  | _ -> failwith <| sprintf "Cant convert %s to DU" s

type Environment =
  | Local
  | CI

type TestType =
  | All
  | Smoke
  | Full
  | UnderDevelopment

type Arguments =
  {
    Browser : canopy.types.BrowserStartMode
    TestType : TestType
    Environment : Environment
  }
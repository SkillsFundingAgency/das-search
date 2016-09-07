module Frameworks

open canopy

open System.Text.RegularExpressions

let DisplayFrameworks baseUrl =
  context "Displaying frameworks"
  let u = sprintf "%s/%s" baseUrl "Apprenticeship/Frameworks"

  "When listing all frameworks" &&& (fun _ -> 
    url u

    let title = read "h2"
    let _,num = 
        Regex.Match(title, "[0-9]+").Value
        |> System.Int32.TryParse

    count ".container p a" num
  )

  "When displaying a framework" &&& (fun _ -> 
    url u
    click "42924, Rail Services: Shunting "
    click ".framework-details span a"
    sleep 1
    count "#keyword-property .property-container .entry" 0
    "#keyword-property input" << "Keyword 1"
    press enter
    
    "#keyword-property input" << "Keyword 2"
    press enter

    count "#keyword-property .property-container .entry" 2

    click (first "#keyword-property .delete")

    count "#keyword-property .property-container .entry" 1
  )
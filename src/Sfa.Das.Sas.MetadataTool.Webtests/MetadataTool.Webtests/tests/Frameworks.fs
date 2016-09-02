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
    "#keyword-property input" << "Keyword 1"
    //click "#keyword-property input"
    press enter

    "#keyword-property input" << "Keyword 2"
    //click "#keyword-property input"
    press enter

    //count "#keyword-property .property-container .entry" 1

    click (first "#keyword-property .entry .delete")

    // count "#keyword-property .property-container .entry" 0
    
  )
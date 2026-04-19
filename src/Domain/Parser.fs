module Parser

open System
open System.Text.RegularExpressions

open Types
open Heuristic

let phonePattern = @"(?:\+33|0)[1-9](?:[\s.-]?[0-9]{2}){4}"

let emailPattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}"

let extract (pattern: string) (text: string) =
    let m = Regex.Match(text, pattern, RegexOptions.IgnoreCase)
    if m.Success then Some (m.Value.Trim()) else None

let extractData (text: string) =
    let (firstname, lastname) =
            match extractName text with
            | Some name -> Some name.FirstName, Some name.LastName
            | None      -> None, None

    {
        firstname = Some (firstname |> Option.defaultValue "No Values")
        lastname  = Some (lastname |> Option.defaultValue "No Values")
        email     = Some (extract emailPattern text |> Option.defaultValue "No Values")
        phone     = Some (extract phonePattern text |> Option.defaultValue "No Values")
    }
module Parser

open System
open System.Text.RegularExpressions

let phonePattern = @"(?:\+33|0)[1-9](?:[\s.-]?[0-9]{2}){4}"

let emailPattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}"

// fake
let firstnamePattern = @"(?:\+33|0)[1-9](?:[\s.-]?[0-9]{2}){4}"

let lastnamePattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}"

let extract (pattern: string) (text: string) =
    let m = Regex.Match(text, pattern, RegexOptions.IgnoreCase)
    if m.Success then Some (m.Value.Trim()) else None
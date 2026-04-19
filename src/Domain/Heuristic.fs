module Heuristic

open System
open System.Text.RegularExpressions

// ── Types ────────────────────────────────────────────────────────────────────

type ParsedName = {
    FullName  : string
    FirstName : string
    LastName  : string
}

// ── Helpers ──────────────────────────────────────────────────────────────────

let normalizeSpaces (s: string) =
    Regex.Replace(s.Trim(), @"\s+", " ")

let containsDigit (s: string) =
    s |> Seq.exists Char.IsDigit

let isEmail (s: string) =
    Regex.IsMatch(s, @"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}\b", RegexOptions.IgnoreCase)

let isPhone (s: string) =
    Regex.IsMatch(s, @"(\+33|0)[1-9](?:[\s\.\-]?\d{2}){4}")

let isLikelyNameWord (word: string) =
    Regex.IsMatch(word, @"^[A-Za-zÀ-ÖØ-öø-ÿ''-]+$")

// ── Forbiddend Words ──────────────────────────────────────────────────

let forbiddenWords =
    set [
        // FR
        "agent"; "développeur"; "developpeur"; "ingénieur"; "ingenieur"
        "consultant"; "stagiaire"; "alternant"; "étudiant"; "etudiant"
        "manager"; "technicien"; "responsable"; "fullstack"; "frontend"; "backend"
        "sécurité"; "securite"; "informatique"; "logiciel"; "système"; "systeme"
        "profil"; "objectif"; "compétences"; "competences"; "expérience"; "experience"
        "formation"; "contact"; "coordonnées"; "coordonnees"; "langues"; "certifications"
        "références"; "references"; "baccalauréat"; "baccalaureat"; "licence"; "master"
        // EN
        "developer"; "engineer"; "consultant"; "intern"; "student"
        "manager"; "technician"; "architect"; "analyst"; "designer"
        "security"; "software"; "hardware"; "network"; "cloud"
        "profile"; "objective"; "summary"; "skills"; "experience"
        "education"; "contact"; "languages"; "references"; "bachelor"
        "senior"; "junior"; "lead"; "head"; "chief"; "officer"
        "devops"; "mobile"; "fullstack"; "frontend"; "backend"
    ]

let cityWords =
    set [
        "paris"; "lyon"; "marseille"; "lille"; "bordeaux"; "toulouse"
        "mulhouse"; "strasbourg"; "nantes"; "rennes"; "montpellier"
        "nice"; "grenoble"; "dijon"; "reims"; "angers"; "tours"
        "london"; "berlin"; "madrid"; "rome"; "amsterdam"
        "brussels"; "vienna"; "zurich"; "geneva"; "dublin"
    ]

// ── Filtres de ligne ──────────────────────────────────────────────────────────

let isLikelyNameLine (line: string) =
    let line  = normalizeSpaces line
    let words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)

    if String.IsNullOrWhiteSpace(line)    then false
    elif isEmail line || isPhone line     then false
    elif containsDigit line               then false
    elif line.Length < 3 || line.Length > 40 then false
    elif words.Length < 2 || words.Length > 4 then false
    else
        let lower       = words |> Array.map (fun w -> w.ToLowerInvariant())
        let hasForbidden = lower |> Array.exists (fun w -> forbiddenWords.Contains w)
        let isCityLine   = lower |> Array.forall (fun w -> cityWords.Contains w)
        let allValid     = words |> Array.forall isLikelyNameWord

        not hasForbidden && not isCityLine && allValid

// ── Extract Name ─────────────────────────────────────────────────────────

let extractName (text: string) =
    let lines =
        text.Split('\n', StringSplitOptions.RemoveEmptyEntries)
        |> Array.map normalizeSpaces
        |> Array.filter (fun l -> not (String.IsNullOrWhiteSpace l))
        |> Array.truncate 10

    let tryMixedCase =
        lines |> Array.tryFind (fun line ->
            let words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            words.Length >= 2 && words.Length <= 4
            && not (containsDigit line)
            && not (isEmail line)
            && not (isPhone line)
            && words |> Array.forall isLikelyNameWord
            && words |> Array.exists (fun w ->
                w.Length > 1 && Char.IsUpper(w.[0]) && w.[1..] = w.[1..].ToLower())
            && words |> Array.exists (fun w ->
                w.Length > 1 && w = w.ToUpper())
        )

    let tryHeuristic =
        lines |> Array.tryFind isLikelyNameLine

    let candidate =
        match tryMixedCase with
        | Some l -> Some l
        | None   -> tryHeuristic

    match candidate with
    | None -> None
    | Some fullName ->
        let parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        if parts.Length < 2 then None
        else
            let upperWords   = parts |> Array.filter (fun w -> w.Length > 1 && w = w.ToUpper())
            let capitalWords = parts |> Array.filter (fun w ->
                w.Length > 1 && Char.IsUpper(w.[0]) && w.[1..] = w.[1..].ToLower())

            if upperWords.Length >= 1 && capitalWords.Length >= 1 then
                Some {
                    FullName  = fullName
                    FirstName = String.concat " " capitalWords
                    LastName  = String.concat " " upperWords
                }
            else
                Some {
                    FullName  = fullName
                    FirstName = parts.[0]
                    LastName  = String.concat " " parts.[1..]
                }
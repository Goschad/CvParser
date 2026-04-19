module Result

open Saturn
open Giraffe
open Giraffe.ViewEngine
open Microsoft.AspNetCore.Http

let head =
    head [] [
        title [] [ str "Resume result" ]
        link [ _rel "stylesheet"; _href "/result/style.css" ]
        link [ _rel "icon"; _type "image/png"; _href "/favicon.png" ]
    ]

let resultView (firstName: string) (lastName: string) (email: string) (phone: string) =
    html [] [
        head 
        body [] [
            div [ _class "container" ] [
                h1 [] [ str "Parsing result" ]

                div [ _class "card" ] [
                    p [] [ strong [] [ str "firstname: " ]; str firstName ]
                    p [] [ strong [] [ str "lastname: " ]; str lastName ]
                    p [] [ strong [] [ str "email: " ]; str email ]
                    p [] [ strong [] [ str "phone: " ]; str phone ]
                ]
            ]
        ]
    ]

let resultHandler next (ctx: HttpContext) =
    let get (key: string) =
        let v = ctx.Session.GetString(key)
        if isNull v || v = "" then "Non trouvé" else v

    htmlView (resultView (get "firstname") (get "lastname") (get "email") (get "phone")) next ctx
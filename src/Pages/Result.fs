module Result

open Saturn
open Giraffe
open Giraffe.ViewEngine

let head (pageTitle: string) =
    head [] [
        title [] [ str pageTitle ]
        link [ _rel "stylesheet"; _href "/result/style.css" ]
    ]

let resultView (firstName: string) (lastName: string) (email: string) (phone: string) =
    html [] [
        head "Resume result"
        body [] [
            div [ _class "container" ] [
                h1 [] [ str "Parsing result" ]

                div [ _class "card" ] [
                    p [] [ strong [] [ str "firstname: " ]; str firstName ]
                    p [] [ strong [] [ str "lastName: " ]; str lastName ]
                    p [] [ strong [] [ str "email: " ]; str email ]
                    p [] [ strong [] [ str "phone: " ]; str phone ]
                ]
            ]
        ]
    ]

let resultHandler next ctx =
    let firstName = "Jean"
    let lastName = "Dupont"
    let email = "jean.dupont@gmail.com"
    let phone = "+33 6 12 34 56 78"

    htmlView (resultView firstName lastName email phone) next ctx
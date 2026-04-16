module Index
open Saturn
open Giraffe
open Giraffe.ViewEngine

let head (pageTitle: string) =
    head [] [
        title [] [ str pageTitle ]
        link [ _rel "stylesheet"; _href "/result/style.css" ]
        script [ _src "/script.js" ] []
    ]

let indexView (pageTitle: string) =
    html [] [
        head pageTitle
        body [] [
            div [ _class "container" ] [
                h1 [] [ str "CV Parser" ]
            ]
        ]
    ]

let indexHandler next ctx =
    htmlView (indexView "CV Parser") next ctx
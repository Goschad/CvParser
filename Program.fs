open Saturn
open Giraffe
open Giraffe.ViewEngine

let head (pageTitle: string) =
    head [] [
        title [] [ str pageTitle ]
    ]

let cvForm = 
    form [ _method "POST"; _action "/upload"; _enctype "multipart/form-data" ] [
        input [ _type "file"; _name "cv" ]
        button [_type "submit"] [ str "Upload"]
    ]

let indexView (pageTitle: string) =
    html [] [
        head pageTitle
        body [] [
            h1 [] [ str "CV Parser" ]
            cvForm
        ]
    ]

let indexHandler =
    htmlView (indexView "CV Parser")

let router = router {
    get "/" indexHandler
}

let app = application {
    use_router router
}

run app
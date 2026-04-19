module Index
open Saturn
open Giraffe
open Giraffe.ViewEngine

let head =
    head [] [
        title [] [ str "F# CV Parser" ]
        link [ _rel "stylesheet"; _href "/index/style.css" ]
        link [ _rel "icon"; _type "image/png"; _href "/favicon.png" ]
        script [ _src "/index/script.js" ] []
    ]

let indexView =
    html [] [
        head
        body [] [
            div [ _class "container" ] [
                h1 [] [ str "CV Parser" ]
                form [ _method "POST"; _action "/upload"; _enctype "multipart/form-data" ] [
                    input [
                        _type "file"
                        _name "cv"
                        _id "fileInput"
                        _accept ".pdf,application/pdf"
                        attr "hidden" "hidden"
                    ]

                    label [ _for "fileInput"; _class "upload-box" ] [
                        div [ _class "upload-content" ] [
                            span [ _id "fileText"; _class "upload-text" ] [
                                str "Drop your PDF here or click to upload (max 5 MB)"
                            ]
                        ]
                    ]

                    button [ _type "submit"; _class "submit-btn" ] [
                        str "Upload"
                    ]
                ]
            ]
        ]
    ]

let indexHandler next ctx =
    htmlView indexView next ctx
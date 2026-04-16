open Saturn
open Giraffe
open Giraffe.ViewEngine

open Router

let app = application {
    use_router mainRouter
    use_static "wwwroot"
}

run app
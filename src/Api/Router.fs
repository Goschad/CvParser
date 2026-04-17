module Router

open Saturn
open Giraffe

open Index
open Result

let pageRouter = router {
    get "/" indexHandler
    get "/result" resultHandler
}

let apiRouter = router {
    get "/" (text "Hello, API!")
}

let mainRouter = router {
    forward "/api" apiRouter
    forward "" pageRouter
}
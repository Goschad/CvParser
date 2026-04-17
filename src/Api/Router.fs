module Router

open Saturn
open Giraffe

open Index
open Result
open Handlers

let pageRouter = router {
    get "/" indexHandler
    get "/result" resultHandler
    post "/upload" handleUpload
}

let apiRouter = router {
    get "/" (text "Hello, API!")
}

let mainRouter = router {
    forward "/api" apiRouter
    forward "" pageRouter
}
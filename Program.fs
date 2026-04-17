open Saturn
open Giraffe
open Giraffe.ViewEngine
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection

open Router

let app = application {
    use_router mainRouter
    use_static "wwwroot"
    service_config (fun services ->
        services.AddDistributedMemoryCache() |> ignore
        services.AddSession()
    )
    app_config (fun app ->
        app.UseSession()
    )
}

run app
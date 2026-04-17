module Handlers

open Saturn
open Giraffe
open System.IO
open UglyToad.PdfPig
open Microsoft.AspNetCore.Http

open Types
open Parser

let handleUpload : HttpHandler =
    fun next ctx -> task {
        let files = ctx.Request.Form.Files

        if files.Count = 0 then
            return! text "No file received" next ctx
        else
            let file = files.[0]

            if file.Length > 5L * 1024L * 1024L then
                return! (setStatusCode 400 >=> text "File too large (max 5 MB)") next ctx
            else
                use stream = new MemoryStream()
                do! file.CopyToAsync(stream)
                let bytes = stream.ToArray()

                use pdf = PdfDocument.Open(bytes)
                
                let texte =
                    pdf.GetPages()
                    |> Seq.collect (fun page -> page.GetWords())
                    |> Seq.map (fun word -> word.Text)
                    |> String.concat " "

                // tmp 
                let cvData = {
                    firstname = Some "Jean"
                    lastname  = Some "DUPONT"
                    email     = Some "jean.dupont@gmail.com"
                    phone     = Some "+33 6 12 34 56 78"
                }

                ctx.Session.SetString("firstname", cvData.firstname |> Option.defaultValue "")
                ctx.Session.SetString("lastname", cvData.lastname |> Option.defaultValue "")
                ctx.Session.SetString("email", cvData.email |> Option.defaultValue "")
                ctx.Session.SetString("phone", cvData.phone |> Option.defaultValue "")

                return! redirectTo false "/result" next ctx
    }


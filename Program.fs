// Learn more about F# at http://fsharp.org

open System
open Suave
open Suave.Logging
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.DotLiquid
open Suave.Routing
open DotLiquid

type Model = { title: string }

type UserModel = { userName: string option }

setTemplatesDir "./templates"

let o = { title = "Hello world" }
let user0 = { userName = Some "Dave" }
let user1 = { userName = None }
let model0 = Some "Dave"
let model1 : string option = None

let app =
  choose
    [ GET >=> rootPath "/samples" >=> choose
        [ subPath "/0" >=> page "sample0.liquid" o
          subPath "/1/some" >=> page "sample1.liquid" user0
          subPath "/1/none" >=> page "sample1.liquid" user1
          subPath "/2/some" >=> page "sample2.liquid" model0
          subPath "/2/none" >=> page "sample2.liquid" model1 ] ]

let config =
  { defaultConfig with
      bindings = [ HttpBinding.createSimple HTTP "127.0.0.1" 7000 ]
      logger = Targets.create LogLevel.Verbose [||] }

[<EntryPoint>]
let main argv =
  printfn "Listening on port 7000"

  startWebServer config app

  0 // return an integer exit code

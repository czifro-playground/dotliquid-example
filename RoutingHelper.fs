module Suave.Routing

  (*
    Code from: https://github.com/SuaveIO/suave/issues/570
  *)

  open Suave
  open Suave.Http
  open Suave.Filters

  let optionally pred value =
    if pred then Some value else None

  let getCurrentRoot (ctx:HttpContext) =
    match ctx.userState.TryFind("rootPath") with
    | None -> ""
    | Some p -> string p

  let rootPath (part:string) ctx =
    let root = getCurrentRoot ctx
    { ctx with userState = ctx.userState.Add("rootPath", root + part) }
    |> Some
    |> async.Return

  let subPath (part:string) ctx =
    let fullPath = (getCurrentRoot ctx) + part
    ctx
    |> optionally (fullPath = ctx.request.path)
    |> async.Return
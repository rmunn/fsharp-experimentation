namespace Todo
open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful

type Class1() = 
    member this.X = "F#"

module Todo =
    let app =
        choose
            [ GET >=> choose
                [ path "/hello" >=> OK "Hello GET"
                  path "/goodbye" >=> OK "Goodbye GET" ]
              POST >=> choose
                [ path "/hello" >=> OK "Hello POST"
                  path "/goodbye" >=> OK "Goodbye POST" ] ]

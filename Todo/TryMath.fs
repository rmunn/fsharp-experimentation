module Nilakantha =
    let rec piSeries start d sign =
        seq {
            let denom = d * (d + 1) * (d + 2)
            let p = start + float sign * (4.0 / float denom)
            yield p
            yield! piSeries p (d+2) (-sign)
        }

    let π = piSeries 3.0 2 +1  // NOTE: This fails to be precise eventually because of floating-point imprecision

    // TODO: Rewrite this with BigRational numbers to get real precision


type Result<'a> =
    | Success of 'a
    | Failure of string list

module Result = 

    let map f xResult = 
        match xResult with
        | Success x ->
            Success (f x)
        | Failure errs ->
            Failure errs
    // Signature: ('a -> 'b) -> Result<'a> -> Result<'b>

    // "return" is a keyword in F#, so abbreviate it
    let retn x = 
        Success x
    // Signature: 'a -> Result<'a>

    let apply fResult xResult = 
        match fResult,xResult with
        | Success f, Success x ->
            Success (f x)
        | Failure errs, Success x ->
            Failure errs
        | Success f, Failure errs ->
            Failure errs
        | Failure errs1, Failure errs2 ->
            // concat both lists of errors
            Failure (List.concat [errs1; errs2])
    // Signature: Result<('a -> 'b)> -> Result<'a> -> Result<'b>

    let bind f xResult = 
        match xResult with
        | Success x ->
            f x
        | Failure errs ->
            Failure errs
    // Signature: ('a -> Result<'b>) -> Result<'a> -> Result<'b>


module List =

    /// Map a Result producing function over a list to get a new Result 
    /// using applicative style
    /// ('a -> Result<'b>) -> 'a list -> Result<'b list>
    let rec traverseResultA f list =

        // define the applicative functions
        let (<*>) = Result.apply
        let retn = Result.Success

        // define a "cons" function
        let cons head tail = head :: tail

        // loop through the list
        match list with
        | [] -> 
            // if empty, lift [] to a Result
            retn []
        | head::tail ->
            // otherwise lift the head to a Result using f
            // and cons it with the lifted version of the remaining list
            retn cons <*> f head <*> traverseResultA f tail


    /// Map a Result producing function over a list to get a new Result 
    /// using monadic style
    /// ('a -> Result<'b>) -> 'a list -> Result<'b list>
    let rec traverseResultM f list =

        // define the monadic functions
        let (>>=) x f = Result.bind f x
        let retn = Result.Success

        // define a "cons" function
        let cons head tail = head :: tail

        // loop through the list
        match list with
        | [] -> 
            // if empty, lift [] to a Result
            retn []
        | head::tail ->
            // otherwise lift the head to a Result using f
            // then lift the tail to a Result using traverse
            // then cons the head and tail and return it
            f head                 >>= fun h -> 
            traverseResultM f tail >>= fun t ->
            retn (cons h t) 

let parseInt str =
    match (System.Int32.TryParse str) with
    | true,i -> Result.Success i
    | false,_ -> Result.Failure [str + " is not an int"]

open FSharp.Core // Unnecessary


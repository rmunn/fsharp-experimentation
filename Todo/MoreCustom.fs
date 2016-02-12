type QuoteBuilder() =
    member x.Quote(q) = q

    member x.Return v = v
    
    member x.ReturnFrom v = v

    [<CustomOperation("foo")>]
    member x.Foo(a, b) = b

    member x.Yield(()) = 0

    member x.Combine(a, b) = b

    member x.Delay(v) = v

let quote = new QuoteBuilder()

(* quote {
    foo 6
} returns:

Call (Some (ValueWithName (FSI_0155+QuoteBuilder, builder@)), Foo,
      [Call (Some (ValueWithName (FSI_0155+QuoteBuilder, builder@)), Yield,
             [Value (<null>)]), Value (6)])

*)


(* quote {
    return! 5
    foo 6
} wanted a Combine method. Then it wanted a Delay method. Then it said "can't use custom
expressions with try/with, try/finally, if/then/else, match, or use." Don't know why.
*)

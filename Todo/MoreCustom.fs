type QuoteBuilder() =
    member x.Quote(q) = q

    member x.Return v = v
    
    member x.ReturnFrom v = v

    [<CustomOperation("foo")>]
    member x.Foo(a, b) = b

    member x.Yield(()) = 0

    member x.Yield(v) = v

    member x.Bind(v, f) = f

    member x.Combine(a, b) = b

    member x.Delay(v) = v

    member x.Run(v) = v

let quote = new QuoteBuilder()

(* quote {
    foo 6
} returns:

Call (Some (ValueWithName (FSI_0155+QuoteBuilder, builder@)), Foo,
      [Call (Some (ValueWithName (FSI_0155+QuoteBuilder, builder@)), Yield,
             [Value (<null>)]), Value (6)])

*)

(* Note that below, I had to specify the kind of f that Bind() expects, because F# couldn't
figure it out (it was generic). So there was a value restriction error until I added the
type signature on "let result":

let result : Quotations.Expr<(string -> int)> = quote {
    let! x = 5
    foo 6
} returns:

Call (Some (ValueWithName (FSI_0164+QuoteBuilder, builder@)), Delay,
      [Lambda (unitVar,
               Call (Some (ValueWithName (FSI_0164+QuoteBuilder, builder@)), Foo,
                     [Call (Some (ValueWithName (FSI_0164+QuoteBuilder, builder@)),
                            Bind,
                            [Value (5),
                             Lambda (_arg1,
                                     Let (x, _arg1,
                                          Call (Some (ValueWithName (FSI_0164+QuoteBuilder,
                                                                     builder@)),
                                                Yield, [x])))]),
                      Value (6)]))])
*)


(* quote {
    return! 5
    foo 6
} wanted a Combine method. Then it wanted a Delay method. Then it said "can't use custom
expressions with try/with, try/finally, if/then/else, match, or use." Don't know why.
*)

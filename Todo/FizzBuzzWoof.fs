module FizzBuzzWoof =

    let isDivisibleBy a b = (a % b) = 0
    let contains a b = a.ToString().Contains(b.ToString())

    let fizzBuzzWoofMatcher n = fun i ->
        if isDivisibleBy i n then Some ()
        elif contains i n then Some ()
        else None

    let (|Fizz|_|) i = fizzBuzzWoofMatcher 3
    let (|Buzz|_|) i = fizzBuzzWoofMatcher 5
    let (|Woof|_|) i = fizzBuzzWoofMatcher 7


open FizzBuzzWoof

let runme () =
    for i = 1 to 100 do
        match i with
        | Fizz () -> printf "Fizz"
        | _ -> ()
        match i with
        | Buzz () -> printf "Buzz"
        | _ -> ()
        match i with
        | Woof () -> printf "Woof"
        | _ -> ()
        match i with
        | Fizz () -> printfn ""
        | Buzz () -> printfn ""
        | Woof () -> printfn ""
        | _ -> printfn "%d" i

runme()
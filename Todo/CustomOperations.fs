module PlayWithStuff =

    type FooBuilder() =
        member x.Yield(vars) =
            printfn "Yield %A" vars
            ["Yield"]

        [<CustomOperation("quux")>]
        member x.Quux (arg1,arg2,arg3) =
            printfn "Quux %A %A %A" arg1 arg2 arg3
            "Quux" :: arg1

        [<CustomOperation("item")>]
        member x.Item (itemsSoFar, newItem) =
            newItem :: itemsSoFar

    let foo = new FooBuilder()

    type MenuRecord = { Text: string; Key: string }

    and MenuItem =
       | Item of MenuRecord
       | Divider
       | SubMenu of Menu

    and Menu = MenuItem list

    type MenuBuilder() =
        member x.Yield(vars) : Menu =
            printfn "menu yield %A" vars
            []

        member x.Zero() : Menu =
            [] // TODO: Might not actually want a Zero(). Do we allow empty menus?

        [<CustomOperation("item")>]
        member x.Item (itemsSoFar : Menu, text : string, key : string) =
            printfn "Items so far: %A" itemsSoFar
            printfn "Adding item %A (%A)" text key
            itemsSoFar @ [Item { Text = text; Key = key }]

        [<CustomOperation("divider")>]
        member x.Div (itemsSoFar : Menu) =
            printfn "Items so far: %A" itemsSoFar
            printfn "Adding ---divider---"
            itemsSoFar @ [Divider]

        [<CustomOperation("submenu")>]
        member x.Submenu (itemsSoFar : Menu, subMenu : Menu) =
            printfn "Items so far: %A" itemsSoFar
            printfn "Adding submenu %A" subMenu
            itemsSoFar @ [SubMenu subMenu]

        member x.For (a, b) =
            printfn "For %A %A" a b
            a @ b()

    let menu = new MenuBuilder()

    let x = menu {
        item "Foo" "Bar"
        divider
        (*submenu menu { // This isn't working. Using "let x = menu { ... } ; submenu x" doesn't work since I don't have a For method. See http://stackoverflow.com/q/23122639
            item "Blarg" "What is this?"
            divider
            item "Foo"
        }*)
        let y = menu {
            divider
        }
        //submenu y -- that doesn't work either. Also note the "menu yield [Divider]" in the output. Huh.
        item "Baz" "Quux" // Note how this is the only item ending up in the menu.
    }
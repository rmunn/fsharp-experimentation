module PlayWithStuff =
(*
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
*)
    type MenuRecord = { Text: string; Key: string }

    and MenuItem =
       | Item of MenuRecord
       | Divider
       | SubMenu of Menu

    and Menu = MenuItem list

    type MenuBuilder() =
        member x.Yield(vars) : Menu =
            printfn "menu yield %A" vars
            vars

        member x.Yield(_ : unit) : Menu =
            printfn "menu yield unit"
            []

        member x.YieldFrom(vars) : Menu =
            printfn "menu yield from %A" vars
            vars

        member x.Zero() : Menu =
            [] // TODO: Might not actually want a Zero(). Do we allow empty menus?

        member x.Bind(a, b) : Menu =
            printfn "bind %A %A" a b
            b a

        [<CustomOperation("item")>]
        member x.Item (itemsSoFar : Menu, text : string, key : string) =
            printfn "Items so far: %A" itemsSoFar
            printfn "Adding item %A (%A)" text key
            itemsSoFar @ [Item { Text = text; Key = key }]

        [<CustomOperation("itemNoKey")>]
        member x.ItemNoKey (itemsSoFar : Menu, text : string) =
            printfn "Items so far: %A" itemsSoFar
            printfn "Adding item %A (%A)" text "no key"
            itemsSoFar @ [Item { Text = text; Key = "" }]

        [<CustomOperation("divider")>]
        member x.Div (itemsSoFar : Menu) =
            printfn "Items so far: %A" itemsSoFar
            printfn "Adding ---divider---"
            itemsSoFar @ [Divider]

        [<CustomOperation("submenu", MaintainsVariableSpace=true)>]
        member x.Submenu (itemsSoFar : Menu, subMenu : Menu) =
            printfn "Items so far: %A" itemsSoFar
            printfn "Adding submenu %A" subMenu
            itemsSoFar @ [SubMenu subMenu]

        member x.For (a, b) =
            printfn "For %A %A" a b
            a @ b()

        member x.Source (u : unit) : Menu =
            printfn "Source unit %A" u
            []

        member x.Source (m : Menu) : Menu =
            printfn "Source %A" m
            m

        member x.Source (i : MenuItem) : Menu =
            printfn "Source %A" i
            [i]

        member x.Run (e) =
            printfn "Running %A" e
            e

    let menu = new MenuBuilder()

    let x = menu {
        itemNoKey "Foo"
        divider
        submenu (menu {
            item "Blarg" "What is this?"
            divider
            itemNoKey "Foo2"
        })
        item "Bar" "b"
        (* let y = menu {   // This doesn't work yet, still needs experimenting
            item "SubFoo" "f2"
            divider
            item "SubBar" "b2"
        }
        submenu y *)
        (* let y! = menu {  // Neither does this
            item "SubFoo" "f2"
            divider
            item "SubBar" "b2"
        }
        submenu y *)
    }

(* Output from the above:
menu yield unit
Items so far: []
Adding item "Foo" ("no key")
Items so far: [Item {Text = "Foo";
       Key = "";}]
Adding ---divider---
menu yield unit
Items so far: []
Adding item "Blarg" ("What is this?")
Items so far: [Item {Text = "Blarg";
       Key = "What is this?";}]
Adding ---divider---
Items so far: [Item {Text = "Blarg";
       Key = "What is this?";}; Divider]
Adding item "Foo2" ("no key")
Running [Item {Text = "Blarg";
       Key = "What is this?";}; Divider; Item {Text = "Foo2";
                                               Key = "";}]
Items so far: [Item {Text = "Foo";
       Key = "";}; Divider]
Adding submenu [Item {Text = "Blarg";
       Key = "What is this?";}; Divider; Item {Text = "Foo2";
                                               Key = "";}]
Items so far: [Item {Text = "Foo";
       Key = "";}; Divider;
 SubMenu [Item {Text = "Blarg";
                Key = "What is this?";}; Divider; Item {Text = "Foo2";
                                                        Key = "";}]]
Adding item "Bar" ("b")
Running [Item {Text = "Foo";
       Key = "";}; Divider;
 SubMenu [Item {Text = "Blarg";
                Key = "What is this?";}; Divider; Item {Text = "Foo2";
                                                        Key = "";}];
 Item {Text = "Bar";
       Key = "b";}]

val x : MenuItem list =
 [Item {Text = "Foo";
        Key = "";};
  Divider;
  SubMenu [Item {Text = "Blarg";
                 Key = "What is this?";};
           Divider;
           Item {Text = "Foo2";
                 Key = "";}];
  Item {Text = "Bar";
        Key = "b";}]

*)
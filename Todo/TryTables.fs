module TryTables =

    /// An attempt at doing Eto table layouts as a DSL
    
    type TableRow = Eto.Forms.Control list

    type TableLayout = TableRow list // TODO: Use PersistentVector instead so we can append in O(1) time

    type PaddingDU =
       | AllSides of int
       | HV of int * int // Horiz, vert
       | LTRB of int * int * int * int // Left, top, right, bottom

    type TableLayoutRecord = {
        Padding : PaddingDU option
        Spacing : (int * int) option
        Layout : TableLayout
    }

    type AutoSizedTagRecord = {
        IsAutoSized : bool
        OldTag : obj
    }

    let controlToCell (c : Eto.Forms.Control) =
        let isAutoSized =
            match c.Tag with
            | :? AutoSizedTagRecord as r ->
                let b = r.IsAutoSized
                c.Tag <- r.OldTag
                b
            | _ -> false
        c.Tag <- null
        new Eto.Forms.TableCell(c, isAutoSized)

    let toEtoTableRow (row : TableRow) =
        new Eto.Forms.TableRow(row |> List.map controlToCell)

    type LayoutBuilder() =
        member x.Yield(()) : TableLayoutRecord = {
            Padding = None
            Spacing = None
            Layout = []
        }

        [<CustomOperation("padding")>]
        member x.Padding(record : TableLayoutRecord, n) =
            { record with Padding = Some (AllSides n) }

        [<CustomOperation("padding2")>]
        member x.PaddingHV(record : TableLayoutRecord, h, v) =
            { record with Padding = Some (HV (h,v)) }

        [<CustomOperation("padding4")>]
        member x.PaddingLTRB(record : TableLayoutRecord, l, t, r, b) =
            { record with Padding = Some (LTRB (l,t,r,b)) }

        [<CustomOperation("spacing")>]
        member x.Spacing(record : TableLayoutRecord, h, v) =
            { record with Spacing = Some (h,v) }

        // member x.Quote(v) = v

        member x.Run(record : TableLayoutRecord) =
            match record.Padding with
            | Some (AllSides n) -> printfn "Padding: %A on all sides" n
            | Some (HV (h,v)) -> printfn "Padding: %A horiz, %A vert" h v
            | Some (LTRB (l,t,r,b)) -> printfn "Padding: Lt %A, Top %A, Rt %A, Bot %A" l t r b
            | None -> printfn "Default padding"
            match record.Spacing with
            | Some (h,v) -> printfn "Spacing: %A horiz, %A vert" h v
            | None -> printfn "Default spacing"
            match record.Layout with
            | [] -> printfn "No rows"
            | _ -> printfn "Some rows, which I don't handle yet"
            new Eto.Forms.TableLayout(record.Layout |> List.map toEtoTableRow)

        [<CustomOperation("row")>]
        member x.Row(record : TableLayoutRecord, data : Eto.Forms.Control list) =
            // This is the hard part, but let's hope it's simple
            match record.Layout with
            | [] -> { record with Layout = [data]}
            | r -> { record with Layout = r @ [data]}


    let layout = new LayoutBuilder()

    let label (s:string) =
        let lbl = new Eto.Forms.Label()
        lbl.Text <- s
        lbl

    let autoSized (c: Eto.Forms.Control) =
        c.Tag <- { IsAutoSized = true; OldTag = c.Tag }
        c

    let testLayout = layout {
        padding 5
        padding2 6 8
        padding4 1 2 3 4
        spacing 10 10
        row [
            label "This is neat" |> autoSized
        ]   
    }
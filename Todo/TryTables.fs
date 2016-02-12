module TryTables =

    /// An attempt at doing Eto table layouts as a DSL

    type TableCell =
       | Foo of string
       | Bar of int
       | Panel of obj

    type TableRow =
       | Row of TableCell list

    type TableLayout =
       | Rows of TableRow list

    type PaddingDU =
       | AllSides of int
       | HV of int * int // Horiz, vert
       | LTRB of int * int * int * int // Left, top, right, bottom

    type TableLayoutRecord = {
        Padding : PaddingDU option
        Spacing : (int * int) option
        Layout : TableLayout
    }

    type LayoutBuilder() =
        member x.Yield(()) : TableLayoutRecord = {
            Padding = None
            Spacing = None
            Layout = Rows []
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

    let layout = new LayoutBuilder()

    let testLayout = layout {
        padding 5
        padding2 6 8
        padding4 1 2 3 4
        spacing 10 10
    }
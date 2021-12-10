module Day09

open Utilities

type Point(value: int) =

    let mutable top = None
    let mutable bottom = None
    let mutable left = None
    let mutable right = None
    let mutable counted = false

    member this.Val = value

    member this.Top
        with get () = top
        and set value = top <- value

    member this.Bottom
        with get () = bottom
        and set value = bottom <- value

    member this.Left
        with get () = left
        and set value = left <- value

    member this.Right
        with get () = right
        and set value = right <- value
    member this.Counted
        with get () = counted
        and set value = counted <- value

let solve () =

    let buildPoint i = Point(i)

    let pairRow (points: Point seq) =
        seq {
            use ie = points.GetEnumerator()

            if ie.MoveNext() then
                let mutable prev = ie.Current

                while ie.MoveNext() do
                    let mutable curr = ie.Current

                    prev.Right <- Some curr
                    curr.Left <- Some prev
                    yield prev

                    prev <- curr

                yield prev
        }

    let pairCol (points: Point seq) =
        seq {
            use ie = points.GetEnumerator()

            if ie.MoveNext() then
                let mutable prev = ie.Current

                while ie.MoveNext() do
                    let mutable curr = ie.Current

                    prev.Bottom <- Some curr
                    curr.Top <- Some prev
                    yield prev

                    prev <- curr

                yield prev
        }

    let isLow (p: Point) =
        (p.Right.IsNone || p.Right.Value.Val > p.Val)
        && (p.Left.IsNone || p.Left.Value.Val > p.Val)
        && (p.Bottom.IsNone || p.Bottom.Value.Val > p.Val)
        && (p.Top.IsNone || p.Top.Value.Val > p.Val)

    let rec findBasinSize (point: Point) =
        let calc (maybePoint: Point option) =
            match maybePoint with
            | Some p when p.Val = 9 -> 0
            | Some p when p.Val > point.Val && p.Val <> 9 -> findBasinSize p
            | _ -> 0

        let selfScore = if point.Val = 9 || point.Counted then 0 else 1
        let top = calc point.Top
        let left = calc point.Left
        let right = calc point.Right
        let bottom = calc point.Bottom

        point.Counted <- true
        selfScore + top + right + left + bottom

    let points =
        ReadInputLines "Day09" "input.txt"
        |> Seq.map Seq.toList
        |> mapDeep int
        |> mapDeep (fun i -> i - int '0')
        |> mapDeep buildPoint
        |> Seq.map pairRow
        |> Seq.transpose
        |> Seq.map pairCol
        |> Seq.transpose

    let lowPoints = points |> Seq.concat |> Seq.filter isLow

    let riskSum =
        lowPoints
        |> Seq.map (fun p -> p.Val + 1)
        |> Seq.sum

    let largestBasinProduct =
        lowPoints
        |> Seq.map findBasinSize
        |> Seq.sortDescending
        |> Seq.take 3
        |> Seq.reduce (*)

    printfn $"Part 1: {riskSum}"
    printfn $"Part 2: {largestBasinProduct}"

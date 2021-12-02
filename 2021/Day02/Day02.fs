module Day02

open Utilities.Ut

type MovementDirection =
    | Up
    | Down
    | Forward

type SubMovementCommand =
    { Direction: MovementDirection
      Length: int }

let solve () =
    let input = ReadInputLines "Day02" "input.txt"

    let inputParser (line: string) =
        let [| dir; len |] = line.Split(" ")
        let numLen = int len
        match dir with
        | "up" -> { Direction = Up; Length = numLen }
        | "forward" -> { Direction = Forward; Length = numLen }
        | "down" -> { Direction = Down; Length = numLen }
        | unknownDirection -> failwith $"Direction unknown: %A{unknownDirection}"

    let basicMove (x, y) (command: SubMovementCommand) =
        match command with
        | { Direction = Down; Length = len } -> (x, y - len)
        | { Direction = Up; Length = len } -> (x, y + len)
        | { Direction = Forward; Length = len } -> (x + len, y)

    let complexMove (x: int, y: int, aim: int) (command: SubMovementCommand) =
        match command with
        | { Direction = Down; Length = len } -> (x, y, aim + len)
        | { Direction = Up; Length = len } -> (x, y, aim - len)
        | { Direction = Forward; Length = len } -> (x + len, y - (aim * len), aim)

    let commands = input |> Seq.map inputParser

    let s1x, s1y = commands |> Seq.fold basicMove (0, 0)
    let s2x, s2y, _ = commands |> Seq.fold complexMove (0,0,0)

    printfn $"Part 1: %A{s1x * -s1y}"
    printfn $"Part 1: %A{s2x * -s2y}"

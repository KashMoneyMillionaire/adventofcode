
module Day03

open Utilities.Ut
open System

type Coordinate = { X: int; Y: int; } 

type Movement = { Direction: char; Distance: int;}

let origin = {X = 0; Y = 0}

let asMovement (dir::dist) = { Direction = dir; Distance = charsAsInt(dist)}

//let mapAsMovement: seq<string> -> seq<Movement> = Seq.map(asMovement)
let mapAsMovement = Seq.map(Seq.toList >> asMovement)

let getNewCoordinate (movement: Movement, co: Coordinate) =
    match movement.Direction with 
    | 'R' -> { co with X = co.X + movement.Distance } 
    | 'L' -> { co with X = co.X - movement.Distance } 
    | 'U' -> { co with Y = co.Y + movement.Distance } 
    | 'D' -> { co with Y = co.Y - movement.Distance } 
    | _ -> failwith "Bad direction"


let coordinatesFromMove (movement: Movement, co: Coordinate) =
    match movement.Direction with 
    | 'R' -> seq { for i in co.X .. co.X + movement.Distance -> {co with X = i}}
    | 'L' -> seq { for i in co.X - movement.Distance .. co.X -> {co with X = i}}
    | 'U' -> seq { for i in co.Y .. co.Y + movement.Distance -> {co with Y = i}}
    | 'D' -> seq { for i in co.Y - movement.Distance .. co.Y -> {co with Y = i}} 
    | _ -> failwith "Bad direction"

let manhattanDistance (co1: Coordinate) (co2: Coordinate) =
    Math.Abs(co1.X - co2.X) + Math.Abs(co1.Y - co2.Y)

let mapToManhattanDistanceFromOrigin = origin |> (Seq.map << manhattanDistance)

let shortestManhattanDistanceFromOrigin coordinates = coordinates |> mapToManhattanDistanceFromOrigin |> Seq.min

let findsEnd (movement, start, finish) = 
    let path = coordinatesFromMove(movement, start)
    match path |> Seq.contains(finish) with
    | true  -> (true, manhattanDistance start finish)
    | false -> (false, movement.Distance)

let rec walkingDistance (start: Coordinate, finish: Coordinate) (directions: Movement list) =
    match directions with
    | [] -> failwith "Couldn't find finish"
    | hd::tail -> 
        let r = findsEnd(hd, start, finish)

        match (r, tail) with
        | ((true, distance), _) -> distance
        | ((false, _), []) -> failwith "No more directions"
        | ((false, distance), _) -> distance + (tail |> walkingDistance(getNewCoordinate(hd, start), finish))

let walkingDistanceFromOrigin finish movements = movements |> walkingDistance(origin, finish)


let rec iterate (current: Coordinate) (hd::tail: Movement list) =
    coordinatesFromMove(hd, current) 
    |> Seq.append <|
        match tail.Length with
        | 0 -> Seq.empty<Coordinate>
        | _ -> tail |> iterate (getNewCoordinate(hd, current))

let solve =
    let input = SplitLinesSplitOn "Day03" ','
    let dir1 = input |> Seq.item(0) |> mapAsMovement |> Seq.toList
    let dir2 = input |> Seq.item(1) |> mapAsMovement |> Seq.toList
    let wire1 = dir1 |> iterate(origin) |> set
    let wire2 = dir2 |> iterate(origin) |> set
    
    // Part 1
    let intersections = wire1 |> Set.intersect(wire2) |> Set.remove(origin)
    printfn "Part 1: %A" (intersections |> shortestManhattanDistanceFromOrigin)    
    
    // Part 2
    let z = intersections |> Seq.map(fun x -> walkingDistanceFromOrigin(x)) 
    let a = z |> Seq.map(fun x -> x(dir1))
    let b = z |> Seq.map(fun x -> x(dir2))
    let lengths = Seq.zip a b
    printfn "Part 2: %A" (lengths |> Seq.map(fun (l, r) -> l + r) |> Seq.min)    
module Day24

open Utilities

type Val =
    | Reference of char
    | Value of int

type Instruction = 
    | Read of char
    | Add of char*Val
    | Multiply of char*Val
    | Divide of char*Val
    | Mod of char*Val
    | Equal of char*Val

let solve () =

    let parseInput =
        function
        | ParseRegex "inp (.)" [ Char ref ] -> Read ref
        | ParseRegex "add (.) (-?\d+)" [ Char ref; Integer value] -> Add (ref, Value value)
        | ParseRegex "add (.) (.)" [ Char ref; Char ref2 ] -> Add (ref, Reference ref2)
        | ParseRegex "mul (.) (-?\d+)" [ Char ref; Integer value] -> Multiply (ref, Value value)
        | ParseRegex "mul (.) (.)" [ Char ref; Char ref2 ] -> Multiply (ref, Reference ref2)
        | ParseRegex "div (.) (-?\d+)" [ Char ref; Integer value] -> Divide (ref, Value value)
        | ParseRegex "div (.) (.)" [ Char ref; Char ref2 ] -> Divide (ref, Reference ref2)
        | ParseRegex "mod (.) (-?\d+)" [ Char ref; Integer value] -> Mod (ref, Value value)
        | ParseRegex "mod (.) (.)" [ Char ref; Char ref2 ] -> Mod (ref, Reference ref2)
        | ParseRegex "eql (.) (-?\d+)" [ Char ref; Integer value] -> Equal (ref, Value value)
        | ParseRegex "eql (.) (.)" [ Char ref; Char ref2 ] -> Equal (ref, Reference ref2)
        | _ -> failwith "bad input"

    let getValue (w,x,y,z) =
        function
        | Value v -> v
        | Reference 'w' -> w
        | Reference 'x' -> x
        | Reference 'y' -> y
        | Reference 'z' -> z
        | _ -> failwith "bad"
    
    let set (w,x,y,z) ref value input =
        match ref with
        | 'w' -> (value, x, y, z), input
        | 'x' -> (w, value, y, z), input
        | 'y' -> (w, x, value, z), input
        | 'z' -> (w, x, y, value), input
        | _ -> failwith "bad2"
    
    let read state ref input =
        set state ref (Seq.head input) (Seq.tail input)
    
    let add state ref value input =
        set state ref ((getValue state (Reference ref)) + value) input 
    
    let mul state ref value input =
        set state ref ((getValue state (Reference ref)) * value) input 
    
    let div state ref value input =
        set state ref ((getValue state (Reference ref)) / value) input 
    
    let mdl state ref value input =
        set state ref ((getValue state (Reference ref)) %% value) input 
    
    let eql state ref value input =
        set state ref (if (getValue state (Reference ref)) = value then 1 else 0) input 
    
    let eval (state, input) =
        function
        | Read ref -> read state ref input
        | Add (ref, v) -> add state ref (getValue state v) input
        | Multiply (ref, v) -> mul state ref (getValue state v) input
        | Divide (ref, v) -> div state ref (getValue state v) input
        | Mod (ref, v) -> mdl state ref (getValue state v) input
        | Equal (ref, v) -> eql state ref (getValue state v) input
        
    let instructions = 
        ReadInputLines "Day24" "test.txt"
        |> Seq.map parseInput
        
    let build x =
        x |> string |> Seq.map (fun c -> (int c) - (int '0'))

//        Seq.initInfinite (fun i -> 99999999999999L - (int64 i))
    seq { 1 .. 9 }
//    |> Seq.filter(fun i -> not (i.ToString().Contains('0')))
    |> Seq.map build
    |> Seq.map (fun modelNum ->
        let input = (((0,0,0,0), modelNum), instructions)
        let result = input ||> Seq.fold eval
        fst result
//            if fourth4 (fst result) = 0 then Some modelNum else None
        )
    |> Seq.iter (fun r -> printf $"{r}\n")
    
//    let goodModel = result |> Seq.toArray |> Array.map (fun i -> (char i) + '0') |> System.String
//    printfn $"Part 1: {goodModel}"
    printfn $"Part 2: "

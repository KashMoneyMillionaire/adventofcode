module Intcode

//type Param = 
//    | Position of int 
//    | Immediate of int

type Operation =
    | Add of int * int * int
    | Mult of int * int * int
    | Halt

type ProgramState = { PC : int; Intcode : Map<int, int>; Halt : bool }
let get i state = state.Intcode.[i]
let getPc state = get state.PC state
let set i x state = { state with Intcode = Map.add i x state.Intcode }
let incPC state = {state with PC = state.PC + 4}



    

let getOp state =
    let opCode = getPc state
    let param x = get (x + state.PC) state
    //let params1 = (param 1)
    //let params2 = (param 1, param 2)
    let params3 = (param 1, param 2, param 3)

    match opCode with
    | 1 -> Add params3
    | 2 -> Mult params3
    | 99 -> Halt
   
let setValue dest value state =
    {state with Intcode = Map.add dest value state.Intcode}

let executeNext state =
    match getOp state with
    | Add (p1, p2, dest) -> 
        let v1 = get p1 state
        let v2 = get p2 state
        state |> setValue dest (v1 + v2)
    | Mult (p1, p2, dest) -> state
    | Halt -> { state with Halt = true }

let rec run s =
    match s.Halt with
    | true -> s
    | false -> executeNext s |> run

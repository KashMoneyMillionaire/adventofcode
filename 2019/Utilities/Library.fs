namespace Utilities

module Ut = 
    let SplitLinesSplitOn (splitBy: char) = 
        System.IO.File.ReadLines("input.txt") |> Seq.map (fun x -> x.Split(splitBy)) |> Seq.toList;

    
    let stringAsInt : string -> int = int

    let charsAsInt : char list -> int  = fun (chars: char list) -> 
        let str = new System.String(List.toArray(chars)) 
        stringAsInt(str)
namespace Utilities

module Ut = 
    let SplitLinesSplitOn (splitBy: char) = 
        System.IO.File.ReadLines("input.txt") |> Seq.map (fun x -> x.Split(splitBy));

module AdventOfCode._2021

open System

let rec prompt forever =
    printfn "Which day would you like to run?"
    
    match Console.ReadLine() with
                | "1" -> Day01.solve()
                | "2" -> Day02.solve()
                | "3" -> Day03.solve()
                | "4" -> Day04.solve()
                | "5" -> Day05.solve()
                | "6" -> Day06.solve()
                | "7" -> Day07.solve()
                | "8" -> Day08.solve()
                | "9" -> Day09.solve()
                | _ -> printfn "Could not find day"

    prompt forever
   
[<EntryPoint>] 
prompt()
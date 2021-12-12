module AdventOfCode._2021

open System
open System.Reflection

let rec prompt forever =

    let modules =
        Assembly.GetExecutingAssembly().GetTypes()
        |> Array.filter (fun t -> t.Name.StartsWith("Day"))

    try
        printfn "Which day would you like to run?"
        let read = Console.ReadLine() |> int

        let chosenModule =
            modules
            |> Seq.find (fun m -> m.Name = $"Day%02i{read}")

        let solveMethod = chosenModule.GetMethod("solve")

        solveMethod.Invoke(null, null) |> ignore
    with
    | _ -> printfn "Could not find day"

    prompt forever

[<EntryPoint>]
prompt ()

module Day21

open Utilities

let solve () =
    
    let parseInput =
        function
        | ParseRegex "Player (\d) starting position: (\d)" [Integer player; Integer position] -> (player, position, 0)
        | _ -> failwith "bad input"

    let mutable deterministicDice = Seq.initInfinite (fun i -> i %% 10) |> Seq.skip 1
    
    let rollDeterministic player =
        let roll = Seq.take 3 deterministicDice
        deterministicDice <- deterministicDice  |> Seq.skip 3
        (player, roll)
    
    let nextRound roll players iter =
//        printf $"\nRound {iter + 1}:\n"
        let mutable hasWinner = false
        players
        |> Seq.map roll
        |> Seq.map (fun ((player, position, score), movement) ->
            if hasWinner then
                (player, position, score)
            else
                let rolled = movement |> Seq.map toString |> String.concat "+"
                let moved = Seq.sum movement
                let newPosition = (position + moved) %% 10
                let newScore = score + newPosition

//                printf $"Player {player} rolls {rolled} and moves from {position} to {newPosition} for a total score of {newScore}\n"
                
                hasWinner <- newScore >= 1000
                (player, newPosition, newScore)
        )
        |> Seq.toList
        |> List.toSeq
        
    let winner players =
        players |> Seq.sortBy third3 |> Seq.head
        
    let mutable cache = Map.empty
    let rollsAndTimes = [(3,1L);(4,3L);(5,6L);(6,7L);(7,6L);(8,3L);(9,1L)]
        
    let rec play2 perm =
        let pos1, pos2, score1, score2 = perm
        if score2 >= 21 then
            (0L, 1L)
        else if Map.containsKey perm cache then
            cache.[perm]
        else
            let mutable subScoreL = 0L
            let mutable subScoreR = 0L
            for r, t in rollsAndTimes do
                let newP1 = (pos1 + r) %% 10
                let newScore1, newScore2 = play2 (pos2, newP1, score2, score1 + newP1)
                subScoreL <- subScoreL + newScore2 * t
                subScoreR <- subScoreR + newScore1 * t
                
            let score = (subScoreL, subScoreR)
            cache <- cache |> Map.add perm score
            (subScoreL, subScoreR)
        
    let players =
        ReadInputLines "Day21" "input.txt"
        |> Seq.map parseInput

    let finalScore =
        (players, Seq.initInfinite id)
        ||> Seq.scan (nextRound rollDeterministic)
        |> Seq.skipWhile (Seq.forall (fun (player, position, score) -> score < 1000))
        |> Seq.head
        |> Seq.toList
        |> winner
    
    let pos1 = players |> Seq.head |> snd3
    let pos2 = players |> Seq.skip 1 |> Seq.head |> snd3
    
    let a = play2 (pos1, pos2, 0, 0)
    
    printfn $"Part 1: {finalScore}"
    printfn $"Part 2: {a}"

// 568867175661958
// 408746284676519
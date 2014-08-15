open System
open Fish.CommandPrompt


let tomp = [ "echo \"steve\" "; "ls"; "cd Windows"] |> runCmd @"C:\"
fst tomp |> List.iter Console.WriteLine
snd tomp |> List.iter Console.WriteLine

Console.ReadLine () |> ignore
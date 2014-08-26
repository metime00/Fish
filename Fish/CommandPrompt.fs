namespace Fish

module CommandPrompt =
    open System
    open System.Text
    open System.Diagnostics
    open System.IO
    open System.Collections.Generic
    
    type CommandOutput =
        | Success of string list
        | Failure of string

    /// Allows a command line command to be run simply through F#, outputs either the output of the command or an error message
    let runCmd startLocation (cmd : string) =
        let errorString = new StringBuilder ()
        let outOut = new List<string> ()
        use p = new Process ()
 
        p.StartInfo.UseShellExecute <- false
        p.StartInfo.RedirectStandardOutput <- true
        p.StartInfo.RedirectStandardError <- true
        p.StartInfo.WorkingDirectory <- startLocation
        p.StartInfo.FileName <- Path.Combine (Environment.SystemDirectory, "cmd.exe")
        p.StartInfo.Arguments <- "/K @echo off"
    
        p.StartInfo.RedirectStandardInput <- true

        p.OutputDataReceived.Add (fun x -> outOut.Add x.Data)

        p.ErrorDataReceived.Add (fun x -> errorString.Append x.Data |> ignore)
        
        p.Start () |> ignore
        p.BeginOutputReadLine ()
        p.BeginErrorReadLine ()
 
        p.StandardInput.WriteLine cmd

        p.StandardInput.WriteLine "exit"
        p.WaitForExit ()

        outOut.RemoveAt 0 //removes the output line created by inputting the command string

        match errorString.Length with
        | 0 -> CommandOutput.Success(List.ofSeq outOut)
        | _ -> CommandOutput.Failure(errorString.ToString ())
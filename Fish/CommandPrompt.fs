namespace Fish

module CommandPrompt =
    open System
    open System.Diagnostics
    open System.IO
 
    /// Allows multiple commands to be piped to a single instance of cmd.exe,
    /// similar to the way a sequence of commands can be run from a batch file.
    let runCmd (cmds : string list) =
        use p = new Process ()
 
        p.StartInfo.UseShellExecute <- false
        p.StartInfo.RedirectStandardOutput <- true
        p.StartInfo.RedirectStandardError <- true
        p.StartInfo.WorkingDirectory <- @"C:\"
        p.StartInfo.FileName <- Path.Combine (Environment.SystemDirectory, "cmd.exe")
    
        p.StartInfo.RedirectStandardInput <- true
 
        let outputErrorHandler (outLine : DataReceivedEventArgs) =
            Console.WriteLine outLine.Data
 
        p.OutputDataReceived.Add (outputErrorHandler)
        p.ErrorDataReceived.Add (outputErrorHandler)
 
        p.Start () |> ignore
        p.BeginOutputReadLine ()
        p.BeginErrorReadLine ()
 
        cmds 
        |> List.iter p.StandardInput.WriteLine
 
        p.StandardInput.WriteLine "exit"
        p.WaitForExit ()
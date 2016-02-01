// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

// Define your library scripting code here

// Step 0. Boilerplate to get the paket.exe tool
 
open System
open System.IO
 
Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
 
if not (File.Exists "paket.exe") then
    let url = "https://github.com/fsprojects/Paket/releases/download/2.48.1/paket.exe"
    use wc = new Net.WebClient()
    let tmp = Path.GetTempFileName()
    wc.DownloadFile(url, tmp)
    File.Move(tmp,Path.GetFileName url);;
 
// Step 1. Resolve and install the packages
 
#r "paket.exe"
 
Paket.Dependencies.Install """
source https://nuget.org/api/v2
nuget Suave
""";;
 
// Step 2. Use the packages
 
#r "packages/Suave/lib/net40/Suave.dll"

open Suave // always open suave

#load "Library1.fs"
open Todo

startWebServer defaultConfig Todo.app // TODO: Use code from Library1.fs instead

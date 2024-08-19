namespace TestAPI

open System
open FsHttp

//CLIENT CALLS
//RPC

module CallRpc =

    open FsHttp
    open System.Text.Json    

    type Parameters =
        {
            a : int
            b : int 
        }

    type Results =
        {
            result : int
        }    
         
    let makeRpcCall (parameters: Parameters) =

        async 
            {
                let url = "http://localhost:8080/"             

                let payload = parameters
                                          
                let! response = 
                    http
                        {
                            POST url
                            body
                            jsonSerialize payload
                        }
                    |> Request.sendAsync
            
                return! Response.deserializeJsonAsync<Results> response
            }

    let runRpc () = 
        
        let parameters =
            {
                a = 222
                b = 444
            }

        let response = makeRpcCall parameters |> Async.RunSynchronously
        printfn "Result: %i" response.result 
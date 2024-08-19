namespace TestAPI

open System
open FsHttp
open Thoth.Json.Net

open ThothSerializationCoders
open ThothDeserializationCoders

//CLIENT CALLS
//REST API

module CallRestApiThoth =

    //************************* GET ****************************

    let getFromRestApi () =

        async
            {
                let url = "http://localhost:8080/" 

                let! responseComplete = //jen jako template
                    http 
                        {
                            GET url
                        }
                    |> Request.sendAsync

                let! response = get >> Request.sendAsync <| url  
                let! jsonString = Response.toTextAsync response 
                
                return
                    Decode.fromString decoderGet jsonString   
                    |> function
                        | Ok value ->
                                    value
                        | Error _  ->  
                                    { 
                                        Message = String.Empty
                                        Timestamp = String.Empty
                                    }                
            }

    //************************* POST ****************************
    
    let postToRestApi () =
        async
            {
                let url = "http://localhost:8080/"
                
                let payload = 
                    {
                        Name = "Alice"
                    }               
                
                let thothJsonPayload = Encode.toString 2 (encoderPost payload)

                let! response = 
                    http
                        {
                            POST url
                            body                              
                            json thothJsonPayload
                        }
                    |> Request.sendAsync            

                let! jsonString = Response.toTextAsync response 
                
                return
                    Decode.fromString decoderPost jsonString   
                    |> function
                        | Ok value ->
                                    value
                        | Error _  ->  
                                    { 
                                        Message = String.Empty     
                                    }                
            } 
    
    //************************* PUT ****************************

    let putToRestApi () =
        async
            {
                let url = "http://localhost:8080/user" // !!! /user 

                let payload = 
                    {
                        Id = 2
                        Name = "Robert"
                        Email = "robert@example.com"
                    }
 
                let thothJsonPayload = Encode.toString 2 (encoderPut payload)  

                let! response = 
                    http
                        {
                            PUT url
                            body 
                            json thothJsonPayload
                        }
                    |> Request.sendAsync            

                let! jsonString = Response.toTextAsync response 
                               
                return
                    Decode.fromString decoderPut jsonString   
                    |> function
                        | Ok value ->
                                    value
                        | Error _  ->  
                                    { 
                                        Message = String.Empty
                                        UpdatedDataTableInfo = 
                                            {
                                                Id = -1
                                                Name = String.Empty
                                                Email = String.Empty     
                                            }
                                    }                             
            } 

    let runRestApiThoth () = 

        let response = getFromRestApi () |> Async.RunSynchronously
        printfn "Message: %s\nTimestamp: %s" response.Message response.Timestamp

        let response2 = postToRestApi () |> Async.RunSynchronously
        printfn "Message: %s" response2.Message 
        
        let response3 = putToRestApi () |> Async.RunSynchronously
        printfn "Message: %s" response3.Message 
        printfn "Updated: %A" response3.UpdatedDataTableInfo
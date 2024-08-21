namespace TestAPI

open System
open FsHttp

//CLIENT CALL TEMPLATES
//REST API

module CallRestApi =

    //************************* GET ****************************

    // Define the response type that matches the structure returned by the GET endpoint
    type HelloResponseGet = 
        {
            Message : string
            Timestamp : string
        }

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
                
                return! Response.deserializeJsonAsync<HelloResponseGet> response                 
            }

    //************************* POST ****************************

    // Define the payload type as used in the POST handler    
    type HelloPayload =
        {
            Name : string
        }    

    type HelloResponsePost =
        {
            Message : string
        }
           
    let postToRestApi () =
        async
            {
                // Define the URL of the API
                //let url = "http://localhost:8080/"
                let url = "http://localhost:8080/api/greetings/greet"
                
                // Create the payload
                let payload = 
                    {
                        Name = "Alice"
                    }

                (*
                let jsonPayload =  
                    """
                    {
                        "name": "Alice"
                    }
                    """
                *)

                // Make the POST request with FsHttp
                let! response = 
                    http
                        {
                            POST url
                            body 
                            jsonSerialize payload 
                            //json jsonPayload
                            (*
                            json """
                            {
                                "name": "Alice"
                            }
                            """
                            *)  
                        }
                    |> Request.sendAsync            

                return! Response.deserializeJsonAsync<HelloResponsePost> response             
            } 

    (*
        In F#, the record fields are typically named using PascalCase (e.g., Name, Email). 
        However, many JSON serializers, including those used by libraries like FsHttp (which internally uses Newtonsoft.Json or System.Text.Json), 
        default to serializing fields in camelCase to match common JSON naming conventions.
   
       json:
        {
          "name": "Alice"
        }    
    *)  
    
    //************************* PUT ****************************
       
    type UserPayload =
        {
            Id : int
            Name : string
            Email : string
        }
   
    // Response type
    type UserResponsePut = 
        {
            Message: string
            UpdatedDataTableInfo: UserPayload
        }

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

                let! response = 
                    http
                        {
                            PUT url
                            body 
                            jsonSerialize payload
                        }
                    |> Request.sendAsync            

                return! Response.deserializeJsonAsync<UserResponsePut> response             
            } 

    let runRestApi () = 

        let response = getFromRestApi () |> Async.RunSynchronously
        printfn "Message: %s\nTimestamp: %s" response.Message response.Timestamp

        let response2 = postToRestApi () |> Async.RunSynchronously
        printfn "Message: %s" response2.Message 
        
        let response3 = putToRestApi () |> Async.RunSynchronously
        printfn "Message: %s" response3.Message 
        printfn "Updated: %A" response3.UpdatedDataTableInfo


(*
./weather-api
Usage
curl http://localhost:3000/weather/{city}
curl http://localhost:3000/weather/Curitiba
Response
{  
   "temperature":"29 °C",
   "wind":"20 km/h",
   "description":"Partly cloudy",
   "forecast":[  
      {  
         "day":1,
         "temperature":"27 °C",
         "wind":"12 km/h"
      },
      {  
         "day":2,
         "temperature":"22 °C",
         "wind":"8 km/h"
      }
   ]
}
*)
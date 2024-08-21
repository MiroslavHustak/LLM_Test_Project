namespace TestAPI

open System
open FsHttp
open Newtonsoft.Json.Linq

//CLIENT CALL TEMPLATES
//REST API

module CallRestApiWeather =

    //************************* GET ****************************

    type Forecast =
        {
            Day : int
            Temperature : string
            Wind : string
        }
    
    type WeatherResponse = 
        {
            Temperature : string
            Wind : string
            Description : string
            Forecast : Forecast list
        }
    
    // Function to parse the JSON response
    let private parseWeatherResponse (json: JObject) =

        let temperature = string json.["temperature"]
        let wind = string json.["wind"]
        let description = string json.["description"]
        
        let forecast =
            json.["forecast"]            
            |> Seq.map
                (fun item ->
                           {
                               Day = item.["day"].ToObject<int>()
                               Temperature = string item.["temperature"]
                               Wind = string item.["wind"]
                           }
                )
            |> Seq.toList
    
        {
            Temperature = temperature
            Wind = wind
            Description = description
            Forecast = forecast
        }
    
    // Function to get the weather data
    let private getWeather locality =

        let result = 
            async 
                {
                    let url = sprintf "https://goweather.herokuapp.com/weather/%s" locality
                              
                    let! response = get >> Request.sendAsync <| url                 
                    let! jsonString = Response.toTextAsync response                
                    let json = JObject.Parse(jsonString)

                    return parseWeatherResponse json
                }
            |> Async.RunSynchronously  
            
        printfn "Requested Locality: %s" locality
        printfn "Current Weather: "
        printfn "Temperature: %s" result.Temperature
        printfn "Wind: %s" result.Wind
        printfn "Description: %s" result.Description
        printfn "\nForecast: "

        result.Forecast
        |> List.iter 
            (fun f ->
                    printfn "Day %d - Temperature: %s, Wind: %s" f.Day f.Temperature f.Wind
            )      
    
    let runRestApiWeather () = getWeather "Klimkovice"


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


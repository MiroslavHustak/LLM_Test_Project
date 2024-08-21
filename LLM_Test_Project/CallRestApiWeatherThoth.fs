namespace TestAPI

open System
open System.IO

open FsHttp
open Thoth.Json.Net

open ThothDeserializationCodersWeather

module CallRestApiWeatherThoth =

    let private getWeather locality =

        let result = 
            async
                {
                    let url = sprintf "https://goweather.herokuapp.com/weather/%s" locality

                    let! response = get >> Request.sendAsync <| url  
                    let! jsonString = Response.toTextAsync response 
        
                    return
                        Decode.fromString decoderGetThoth jsonString   
                        |> function
                            | Ok value ->
                                        value
                            | Error _  ->                                       
                                        {
                                            TemperatureThoth = String.Empty
                                            WindThoth = String.Empty
                                            DescriptionThoth = String.Empty
                                            ForecastThoth = [] 
                                        }
                                             
                } 
            |> Async.RunSynchronously

        printfn "Requested Locality: %s" locality
        printfn "Current Weather: "
        printfn "Temperature: %s" result.TemperatureThoth
        printfn "Wind: %s" result.WindThoth
        printfn "Description: %s" result.DescriptionThoth
        printfn "\nForecast: "
                                      
        result.ForecastThoth
        |> Seq.iter 
            (fun f ->
                    printfn "Day %d - Temperature: %s, Wind: %s" f.DayThoth f.TemperatureThoth f.WindThoth
            )                                      
    
    let runRestApiWeatherThoth () = getWeather "Opava"

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


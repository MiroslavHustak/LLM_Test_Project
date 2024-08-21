namespace TestAPI

#if FABLE_COMPILER
open Thoth.Json
#else
open Thoth.Json.Net
#endif

//**************** GET ********************

type ForecastThoth =
    {
        DayThoth : int
        TemperatureThoth : string
        WindThoth : string
    }

type WeatherResponseThoth = 
    {
        TemperatureThoth : string
        WindThoth : string
        DescriptionThoth : string
        ForecastThoth : ForecastThoth list
    }

module ThothDeserializationCodersWeather =   

    // Decoder for ForecastThoth
    let private forecastDecoder : Decoder<ForecastThoth> =
        Decode.object
            (fun get ->
                      {
                          DayThoth = get.Required.Field "day" Decode.int
                          TemperatureThoth = get.Required.Field "temperature" Decode.string
                          WindThoth = get.Required.Field "wind" Decode.string
                      }
            )

    // Decoder for WeatherResponseThoth
    let internal decoderGetThoth: Decoder<WeatherResponseThoth> =
        Decode.object
            (fun get ->
                      {
                          TemperatureThoth = get.Required.Field "temperature" Decode.string
                          WindThoth = get.Required.Field "wind" Decode.string
                          DescriptionThoth = get.Required.Field "description" Decode.string
                          ForecastThoth = get.Required.Field "forecast" (Decode.list forecastDecoder)
                      }
            )
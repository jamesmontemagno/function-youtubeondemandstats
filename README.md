#  YouTube on Demand Stats
.NET Azure Function to get YouTube Stats for all videos on a channel


## Setup

1. Create a file in the solution called "local.settings.json"
2. Fill it in with the following:

```jxon
{
    "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "YOUTUBE_API_KEY": "",
    "YOUTUBE_APP_NAME": ""
  }
}
```

3. Create a YouTube API Key via Data v3 in Google API Console https://developers.google.com/youtube/v3/getting-started
4. Fill in your YOUTUBE_APP_NAME & YOUTUBE_API_KEY
5. Run the function and the API will be available at `http://localhost:7071/api/GetChanneVideoStats/{ids}` 
6. Call the API where the {id} is your actual youtube channel id or multiple Ids.


License under MIT

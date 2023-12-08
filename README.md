# Hacker News API Challenge

## Overview

Using ASP.NET Core, implement a RESTful API to retrieve the details of the best n stories from the Hacker News API, as determined by their score, where n is specified by the caller to the API.

The Hacker News API is documented here: https://github.com/HackerNews/API
The details for an individual story ID can be retrieved from this URI: https://hacker-news.firebaseio.com/v0/item/21233041.json (in this case for the story with ID 21233041)

The API should return an array of the best n stories as returned by the Hacker News API in descending order of score, in the form:
```json
[
    {
        "title": "A uBlock Origin update was rejected from the Chrome Web Store",
        "uri": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
        "postedBy": "ismaildonmez",
        "time": "2019-10-12T13:43:01+00:00",
        "score": 1716,
        "commentCount": 572
    },
    {...},
    {...},
    {...},
    ...
]
```

## Usage:

#### Testing
1. Clone this repository
2. Build the solution using Visual Studio, or on the [command line](https://www.microsoft.com/net/core) with `dotnet build`.
3. Run the project. The API will start up on https://localhost:7239, or http://localhost:5200 with `dotnet run`.
4. Use an HTTP client like [Postman](https://www.getpostman.com/) or [Swagger](https://localhost:7239/swagger/index.html) to `GET https://localhost:7239/best/stories?maxOfStories=10`.

#### Listing best stories descending order by score

<details>
 <summary><code>GET</code> <code><b>/</b></code> <code>(Listing best stories descending order by score)</code></summary>

##### Parameters

> | name         |  type     | data type               | description                                                           |
> |--------------|-----------|-------------------------|-----------------------------------------------------------------------|
> | maxOfStories |  required | int                     | number of stories as returned  |

##### Responses

> | http code     | content-type                     | response                                  |
> |---------------|----------------------------------|-------------------------------------------|
> | `200`         | `application/json;charset=UTF-8` |  [ {"title": "string", "uri": "string", "postedBy": "string", "time": "2023-12-08T10:17:45.657Z", "score": 0, "commentCount": 0 }] | 

##### Example cURL

> ```javascript
>  curl -X 'GET' 'https://localhost:7239/best/stories?maxOfStories=10' -H 'accept: application/json'
> ```

## Enhancements:

Use redis cache for share cache between differnts instances of API Rest.

Call the update enpoint and check if there are changes in the cached stories, if yes update it.

Add more unit testing cases
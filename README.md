# HackerNewsApi
HackerNew Api is a sample Api project to return an array of the first n "best stories" as returned by the Hacker News API, sorted by their score in a descending order
# Methods
The Api has one methed , which is GetBestStories. It takes in an integer as an input to retun the first n "best stories"
# Distuributed Cache
The Application uses distuributed Cache to store the stories which has been accessed for a day. The duration of the cache can be set in the appsettings.json file. This caching is to reduce the number of calls to the HackerNews API 
# Running the project
1) Clone the project in visual studio 22 or higher
2) In the App settings file make sure to set the HackerNew API url
3) Compile the project and run it in local IIS Express in visual studio

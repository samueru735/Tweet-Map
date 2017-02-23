# Tweet-Map

UWP app showing an interactive map to display tweets from the area.

For the app to work properly, you should add an app.config file in the Tweet_Map.Core with following code.
You also need the appropriate application-only keys from [Twitter](https://apps.twitter.com/).

```<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>
    <add key="consumerKey" value="YOURCONSUMERKEY" />
    <add key="consumerSecret" value="YOURCONSUMERSECRET" />
    <add key="accessToken" value="YOURACCESSTOKEN" />
    <add key="accessTokenSecret" value="YOURACCESSTOKENSECRET" />
  </appSettings>
</configuration> 
```

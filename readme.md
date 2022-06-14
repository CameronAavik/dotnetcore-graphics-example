# :warning: **THIS REPO IS OUTDATED**

This repo was originally written as an accompaniment to a [blog post](https://medium.com/@cameronaavik/cross-platform-graphics-in-net-core-901be29dabd7) written in January 2017, in the early days of .NET Core.  
There are now many cross-platform .NET graphics frameworks available to use rather than the approach used in this repo.  

## Cross-Platform Triangle in .NET Core

Makes use of OpenGL and GLFW.

To try this out:

1. Download .Net Core https://www.microsoft.com/net/download/core#/current/sdk (You need 1.1.0)
2. Clone this repo and run either publish.cmd or publish.sh depending on your OS
3. Go to the out directory and find the directory which relates to your OS
4. Inside that directory run `dotnet dotnetcore-graphics-example.dll`

If it all worked you should see the following:

![output](output.png)

[![Test Example, with Test containers](https://github.com/JacquesBronk/docker-101/actions/workflows/test-example.yml/badge.svg)](https://github.com/JacquesBronk/docker-101/actions/workflows/test-example.yml)  [![dotnet Basic](https://github.com/JacquesBronk/docker-101/actions/workflows/dotnet-basic.yml/badge.svg)](https://github.com/JacquesBronk/docker-101/actions/workflows/dotnet-basic.yml)

🐳 **Embarking on the Docker Journey: Empowering Your Infrastructure**

Stepping into the world of Docker might seem daunting at first, but embracing this powerful tool is essential for leveraging the comprehensive ecosystem offered by the [Cloud Native Computing Foundation (CNCF)](https://landscape.cncf.io/). In our technological era, aligning with the methodologies and tools that underpin major [Big Tech](https://en.wikipedia.org/wiki/Big_Tech) infrastructures can significantly elevate the efficiency, scalability, and robustness of your projects.

**Why Focus on Docker?**

Docker, is not just about containerization; it's about creating an environment where applications can thrive in any landscape, whether it's on a developer's laptop or scaling across a global enterprise.

This repository aims to illuminate the path for your Docker journey. 

> [!NOTE]
> 1. At any time, you are free to create an Issue, when you require one of the following 
>	    - A better explanation on a topic that is related to docker.
>	    - An example needs to be extended or included.
> 2. You can create Pull Requests to amend the documentation, examples. Contributions are always welcome.  	

------------------------------------------------------
### 🐳 **Docker Basics: Laying the Groundwork for Containerization Mastery**

Docker has revolutionized the software industry by providing a standard way to package applications and their dependencies into a single object called a container. These lightweight, portable, and self-sufficient containers can run easily and consistently on any machine that has Docker installed, eliminating the "it works on my machine" headache.

### What is a Dockerfile?

A Dockerfile is a text document that contains all the commands a user could call on the command line to assemble an image. Using `docker build`, you can create an automated build that executes several command-line instructions in succession.

#### Creating Your First Dockerfile

1. **Set Up Your Environment:**
    - Create a directory in your local workspace (e.g., `MyDocker`).
    - Inside this directory, create a file named `Dockerfile`.
2. **Understand Dockerfile Structure:**
    - Each Dockerfile is a script, composed of various commands (instructions) and arguments listed successively to automatically perform actions on a base image in order to create a new one.
    - Instructions are capitalized, e.g., `FROM`, `LABEL`, `RUN`, `ADD`, etc.
    - Example Dockerfile content:
```Dockerfile
# Use the official image as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["MyWebApp/MyWebApp.csproj", "MyWebApp/"]
RUN dotnet restore "MyWebApp/MyWebApp.csproj"

# Copy everything else and build the app
COPY . .
WORKDIR "/src/MyWebApp"
RUN dotnet build "MyWebApp.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "MyWebApp.csproj" -c Release -o /app/publish

# Final stage / image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyWebApp.dll"]
```
Here's what's happening in this Dockerfile:

1. **Parent image:** It starts from a .NET 6 ASP.NET Core runtime image, which includes the .NET runtime and ASP.NET Core libraries, ideal for running your application.
2. **Building the app:** It uses a separate SDK image to build the app with `dotnet build`. The build artifacts are output to the `/app/build` directory.
3. **Publishing the app:** The Dockerfile then publishes the app with `dotnet publish`, which puts the files in `/app/publish`.
4. **Final image:** Finally, it sets up the runtime image by starting again from the base image, copying the published files, and defining the entry point for the application. This pattern reduces the final image size by excluding the SDK and other build artifacts.

Make sure you replace `"MyWebApp/MyWebApp.csproj"` and `"MyWebApp.dll"` with the actual names of your .csproj file and the output DLL from your project.

### Building and Running the Docker Image

Once you have your Dockerfile in place, you can build and run your application in Docker with the following commands:
1. **Build your image:**
```bash
docker build -t mywebapp .
```
This command creates a Docker image named `mywebapp`.  
2. **Run your container:**
``` bash
docker run -d -p 8080:80 --name my_app mywebapp
```
This command runs your image in a container, mapping port 8080 on your host to port 80 in the container, and names the running container `my_app`.

You should now be able to access your application by navigating to `http://localhost:8080` in your web browser.

Remember, the specific commands and file names can vary depending on your application's structure, so you may need to adjust paths and names accordingly.

### Docker Compose

Docker Compose is a tool for defining and running multi-container Docker applications. It uses a YAML file to configure your application’s services, networks, and volumes, and then with a single command, you create and start all the services from your configuration.

#### Using Docker Compose

1. **Compose file:**
    - To use Docker Compose, you need to create a `docker-compose.yml` file in your project’s root directory.
    - The YAML file defines the services that make up your app so they can be run together in an isolated environment.
2. **Example `docker-compose.yml`:**  
```yaml  
version: '3'
services:
  web:
    build: .
    ports:
     - "5000:5000"
  redis:
    image: "redis:alpine"
```
- This compose file defines two services, `web` and `redis`. The `web` service uses the Dockerfile in the current directory.
3. **Running Docker Compose:**
    - To get your services up and running, you use the command:
```shell
docker-compose up
```
4. **Other Operations:**
    - Stopping services: `docker-compose down`
    - Background mode: `docker-compose up -d`
    - Viewing logs: `docker-compose logs`

### Relevant Links for Additional Context

1. [Official Docker documentation](https://docs.docker.com/)
2. [Dockerfile reference](https://docs.docker.com/engine/reference/builder/)
3. [Docker Compose documentation](https://docs.docker.com/compose/)
4. [Best practices for writing Dockerfiles](https://docs.docker.com/develop/develop-images/dockerfile_best-practices/)
5. [Docker Hub](https://hub.docker.com/) - For pre-built container images

### Basics for Specific Roles
**[As a Developer](./AsADeveloper/AsADeveloper.md):** You need to understand how to create efficient Dockerfiles, integrate Docker into their development process, and basics of Docker Compose for environment standardization.

**[As a Solutions Architects](./AsASolutionsArchitect/AsASolutionsArchitect.md):** You should focus on how Docker and container orchestration fit into larger IT environments, understanding Docker networking, volumes, and service discovery.

**[As a DevOps Engineer](./AsADevOpsEngineer/AsADevOpsEngineer.md):** You require an understanding of orchestrating Docker containers in production environments, implementing CI/CD pipelines with Docker, and strategies for logging, monitoring, and auto-scaling.

**[As a Product Owner](./AsAProductOwner/AsAProductOwner.md):** You need basic knowledge of Docker’s business value, how it affects the delivery pipeline, and understanding timelines and requirements for Docker environments.

**[As a Tester](./AsATester/AsATester.md):** You should understand how to set up isolated testing environments with Docker, integrate Docker with testing frameworks, and the concept of infrastructure as code.

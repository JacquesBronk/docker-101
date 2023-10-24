## Mastering Docker Cache for Efficient Builds

Docker tries to save time by reusing the intermediate images (also known as cache) from previous builds. This feature is incredibly beneficial, but if not handled properly, you could end up busting the cache unintentionally and slowing down the process instead of speeding it up. Here's how to take control.

### How Docker Cache Works

When Docker builds an image, it processes each instruction within the Dockerfile, creating a new layer for each step. Before executing an instruction, it checks to see if it has already executed the same command with the same context before (files in the build directory) and, if possible, reuses the existing layer from the cache.

However, there are several reasons Docker might not be able to use the cache, such as:

- The instruction in the Dockerfile changed since the last build.
- The files or directories copied in the Dockerfile changed.
- A previous build specified to ignore the cache.

### Tips for Cache-Efficient Dockerfiles

1. **Order Your Instructions Wisely**: Since the cache checks start from the top of your Dockerfile, you'll want to order your instructions from the least frequently changed to the most. For instance, installation of dependencies rarely changes and should be at the top, while copying your source code happens last.

```Dockerfile
# Good practice as dependencies don't change often 
COPY requirements.txt . 
RUN pip install -r requirements.txt  

# This part changes more frequently 
COPY src/ .
```
2. **Minimize Context Transfers with .dockerignore**: Docker sends your entire application context (i.e., all the files in your application directory) to the Docker daemon during the build. By defining unnecessary files or directories in a `.dockerignore` file, you reduce the amount of data that needs to move (which can bust the cache and slow down the build).
 
```dockerignore
# Sample .dockerignore 
.git
.vscode
logs/
temp/
```
3. **Avoid Cache Busting with ADD/COPY**: If you use `COPY` or `ADD` instructions for local files, the cache will bust as soon as those files change. However, if you're sure the changes don’t affect the build, you can use a trick: before the primary `COPY`, add a `COPY` for just the files that influence the build (like dependency manifests) and then perform the necessary installations.

  ```Dockerfile
# Only copy the package.json initially, not all source files 
COPY package.json . 
RUN npm install  
  
# Now copy in the rest of the context 
COPY . .
```

4. **Use Build Args for Dynamic Commands**: If you have dynamic elements in commands (like timestamps or git hashes), they can bust the cache. Instead, use build arguments and set them to different values only when necessary.

```Dockerfile
ARG BUILD_VERSION=unknown 
LABEL BuildVersion=$BUILD_VERSION
```
Then, only provide the `--build-arg` when you want to update the version:
 ```Dockerfile
docker build --build-arg BUILD_VERSION=1.0.1 .
```

### Proactively Bust the Cache When Needed

Sometimes, you actually want to bust the cache to ensure that you’re pulling the latest packages and resources, particularly for security patches or major updates.

- **Use a Unique Cache Buster**: If you need to force Docker to ignore the cache for a specific instruction/set of instructions, a common pattern involves introducing an arbitrary build argument with a new value each time.

```Dockerfile
ARG CACHE_BUST=1 
RUN apt-get update && apt-get install -y my-dependency
```
   And when you want to bust the cache:
   ```shell
docker build --build-arg CACHE_BUST=$(date +%s) .
```
- **Use the `--no-cache` Option**: This is a sledgehammer approach that ignores all cached layers.
```shell
docker build --no-cache .
```
